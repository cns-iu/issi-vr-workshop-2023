using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;
using System;
using UnityEngine.UI;
using Assets.Scripts.Data;
using AndreasBueckle.Assets.Scripts.Data;
using AndreasBueckle.Assets.Scripts.Visualization;

public enum Layout { Network, Geospatial };

/// <summary>
/// Builds the geospatial and network visualiations at the start of the application
/// </summary>
public class Visualizer : MonoBehaviour
{
    public static Visualizer Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [Header("Entities")]
    [field: SerializeField] public List<GameObject> EdgeObjectsNetwork = new List<GameObject>();
    [field: SerializeField] public List<GameObject> NodeObjectsNetwork = new List<GameObject>();
    [field: SerializeField] public List<GameObject> EdgeObjectsGeospatial = new List<GameObject>();
    [field: SerializeField] public List<GameObject> NodeObjectsGeospatial = new List<GameObject>();

    [field: SerializeField] private List<Node> nodes;
    [field: SerializeField] private List<Edge> edges;

    [field: SerializeField] private List<GameObject> groupSymbols = new List<GameObject>();
    [field: SerializeField] private List<GameObject> channelSymbols = new List<GameObject>();

    private Dictionary<float, List<GameObject>> DictLatNode = new Dictionary<float, List<GameObject>>();

    [Header("Scene Setup")]
    [field: SerializeField] private Transform nodeParentNetwork;
    [field: SerializeField] private Transform edgeParentNetwork;
    [field: SerializeField] private Transform nodeParentGeo;
    [field: SerializeField] private Transform edgeParentGeo;
    [field: SerializeField] private Vector3 offsetNetwork = new Vector3(0f, 3.4f, 5f);
    [field: SerializeField] private float scalingFactor = 1f;
    [field: SerializeField] private float edgeScaleFactor = 3f;
    [SerializeField] private List<GameObject> _corners = new List<GameObject>();
    [SerializeField] private float minLat;
    [SerializeField] private float maxLat;
    [SerializeField] private float minLon;
    [SerializeField] private float maxLon;
    [SerializeField] private float _geoOffset = .15f;
    [field: SerializeField] public Button ResetButton { get; private set; }

    [Header("Prefabs")]
    [field: SerializeField] private GameObject pre_Node;
    [field: SerializeField] private GameObject pre_Edge;

    [Header("Visual Encoding")]

    [field: SerializeField] private float _maxStartWidth;
    [field: SerializeField] private float _maxEndWidth;
    [field: SerializeField] private float _minStartWidth;
    [field: SerializeField] private float _minEndWidth;
    [SerializeField] private float _maxWeight;
    [field: SerializeField] public Color GroupColor { get; private set; }
    [field: SerializeField] public Color ChannelColor { get; private set; }
    [field: SerializeField] private Color edgeEndColor;
    [field: SerializeField] private Color edgeStartColor;
    [field: SerializeField] private float minNodeSize;
    [field: SerializeField] private float maxNodeSize;

    /// <summary>
    /// Driver code, runs during the first frame of the application
    /// </summary>
    void Start()
    {
        GetLists();

        NodeObjectsNetwork = CreateNodeObjects(Layout.Network);
        NodeObjectsGeospatial = CreateNodeObjects(Layout.Geospatial);

        LayOutNodes(Layout.Network, NodeObjectsNetwork);
        LayOutNodes(Layout.Geospatial, NodeObjectsGeospatial);
        GetNodeDefaultPositions();

        EdgeObjectsNetwork = CreateEdges(Layout.Network);
        EdgeObjectsGeospatial = CreateEdges(Layout.Geospatial);

        ForNodesAndEdgesFillConnectionProperties(NodeObjectsNetwork, EdgeObjectsNetwork);
        ForNodesAndEdgesFillConnectionProperties(NodeObjectsGeospatial, EdgeObjectsGeospatial);

        SetEdgePositionsandWidth(EdgeObjectsNetwork);
        SetEdgePositionsandWidth(EdgeObjectsGeospatial);

        SizeNodes(NodeObjectsNetwork, Layout.Network);
        SizeNodes(NodeObjectsGeospatial, Layout.Geospatial);

        RotateNodeParent();
    }

    /// <summary>
    /// Creates edges given the specified layout
    /// </summary>
    /// <param name="layout"></param>
    /// <returns></returns>
    List<GameObject> CreateEdges(Layout layout)
    {
        List<GameObject> result = new List<GameObject>();
        foreach (var edge in edges)
        {
            GameObject line = Instantiate(pre_Edge);
            EdgeData data = line.AddComponent<EdgeData>();
            data.Source = edge.SourceID;
            data.Target = edge.TargetID;
            data.Weight = edge.Weight;
            data.TimeStep = edge.TimeStep;

            result.Add(line);
            line.AddComponent<EdgeSetter>();
        }

        switch (layout)
        {
            case Layout.Network:
                Parent(result, edgeParentNetwork);
                break;
            case Layout.Geospatial:
                Parent(result, edgeParentGeo);
                break;
            default:
                break;
        }

        return result;
    }

    /// <summary>
    /// Adjust the edge's position and width, given a list with all edge GameObjects
    /// </summary>
    /// <param name="e">A list of all edges (as instantiated GameObjects</param>
    void SetEdgePositionsandWidth(List<GameObject> e)
    {
        //First, we get the maximum weight of all edges
        _maxWeight = GetMaxWeight(edges);

        //Then, we loop through all edge objects and set the start and end
        for (int i = 0; i < e.Count; i++)
        {
            GameObject line = e[i];
            EdgeData data = line.GetComponent<EdgeData>();
            e[i].GetComponent<LineRenderer>().SetPositions(
                new Vector3[2]{
                data.SourceNode.transform.position,
                data.TargetNode.transform.position
                }
            );

            //Then, we use a utility function to determine the raw start and end width given the edge's weight
            float startWidth = data.Weight.Remap(
                0f, _maxWeight, _minStartWidth, _maxStartWidth
                );
            float endWidth = data.Weight.Remap(
                0f, _maxWeight, _minEndWidth, _maxEndWidth
                );

            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();

            //We set the width with a scale factor
            lineRenderer.startWidth = startWidth * edgeScaleFactor;
            lineRenderer.endWidth = endWidth * edgeScaleFactor;

            //We set the start and end color to denote source and target
            float alpha = data.Weight.Remap(
                0f, _maxWeight, .3f, 1f
                );
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(edgeStartColor, 0.0f), new GradientColorKey(edgeEndColor, 0.2f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
            lineRenderer.colorGradient = gradient;

            line.transform.parent = edgeParentNetwork.transform;
        }
    }

    /// <summary>
    /// We assign parent transforms for easier handling in the editor, and to be able to move all edges together more easily
    /// </summary>
    /// <param name="objects"></param>
    /// <param name="parent"></param>
    void Parent(List<GameObject> objects, Transform parent)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.parent = parent;
        }
    }

    /// <summary>
    /// Capture all nodes' default position 
    /// </summary>
    void GetNodeDefaultPositions()
    {
        for (int i = 0; i < NodeObjectsNetwork.Count; i++)
        {
            NodeData data = NodeObjectsNetwork[i].GetComponent<NodeData>();
            data.defaultPosition = NodeObjectsNetwork[i].transform.position;
        }
    }

    /// <summary>
    /// Gets references to the Nodes and Edges lists (structs) from the DataReader
    /// </summary>
    void GetLists()
    {
        nodes = DataReader.Instance.Nodes;
        edges = DataReader.Instance.Edges;
    }

    /// <summary>
    /// Gets the max weight from a list of edges
    /// </summary>
    /// <param name="edges">All the edges from the DataReader</param>
    /// <returns>The maximum weight of all edges</returns>
    float GetMaxWeight(List<Edge> edges)
    {
        List<float> weights = new List<float>();
        foreach (var item in edges)
        {
            weights.Add(item.Weight);
        }

        return Mathf.Max(weights.ToArray());
    }

    /// <summary>
    /// Adds connection properties for node objects, i.e., adds incoming and outgoing edges to dedicated lists on their NodeData components
    /// </summary>
    /// <param name="nodes"></param>
    /// <param name="edges"></param>
    void ForNodesAndEdgesFillConnectionProperties(List<GameObject> nodes, List<GameObject> edges)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            NodeData n = nodes[i].GetComponent<NodeData>();
            for (int k = 0; k < edges.Count; k++)
            {
                EdgeData e = edges[k].GetComponent<EdgeData>();

                if (e.Source == n.id)
                {
                    n.OutgoingEdges.Add(e.gameObject);
                    e.SourceNode = n.gameObject;
                }

                if (e.Target == n.id)
                {
                    n.IncomingEdges.Add(e.gameObject);
                    e.TargetNode = n.gameObject;
                };
            }
        }
    }

    /// <summary>
    /// Lays out nodes in 3D, given a Layout option (geospatial or network)
    /// </summary>
    /// <param name="layout"></param>
    /// <param name="objects"></param>
    void LayOutNodes(Layout layout, List<GameObject> objects)
    {
        switch (layout)
        {
            case Layout.Network:
                for (int i = 0; i < objects.Count; i++)
                {
                    objects[i].transform.position = objects[i].GetComponent<NodeData>().defaultPosition * scalingFactor + offsetNetwork;
                }
                break;

            case Layout.Geospatial:
                for (int i = 0; i < objects.Count; i++)
                {
                    objects[i].transform.position = GetCorrectedLatLonForWorld(
                        objects[i].GetComponent<NodeData>().latitude,
                        objects[i].GetComponent<NodeData>().longitude
                        );
                }
                StackNodes(NodeObjectsGeospatial);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// When laying nodes out geospatially, this function ensures that those with identical lat/lon are stacked on top of each other to avoid occlusion
    /// </summary>
    /// <param name="nodes"></param>
    void StackNodes(List<GameObject> nodes)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            NodeData data = nodes[i].GetComponent<NodeData>();

            if (DictLatNode.ContainsKey(data.latitude))
            {
                DictLatNode[data.latitude].Add(data.gameObject);
            }
            else
            {
                DictLatNode.Add(data.latitude, new List<GameObject> { data.gameObject });
            }
        }
        foreach (var kvp in DictLatNode)
        {
            for (int i = 0; i < kvp.Value.Count; i++)
            {
                kvp.Value[i].transform.Translate(0f, _geoOffset * i, 0f);
            }
        }
    }

    /// <summary>
    /// Adjust lat/lon values from coordinates to a position on the terrain table, given the Transforms of confining GameObjects
    /// </summary>
    /// <param name="originalLat"></param>
    /// <param name="originalLon"></param>
    /// <returns>A Vector3 with a corrected position to fit on the table</returns>
    Vector3 GetCorrectedLatLonForWorld(float originalLat, float originalLon)
    {
        if (originalLat == 0) return _corners[4].transform.position;

        float maxDiffLat = maxLat - minLat;
        float maxDiffLon = maxLon - minLon;

        Vector3 corrected = new Vector3(
            Mathf.Lerp(
            _corners[2].transform.position.x,
            _corners[3].transform.position.x,
             (originalLon - minLon) / maxDiffLon

            ),

            _corners[0].transform.position.y,

            Mathf.Lerp(
            _corners[0].transform.position.z,
            _corners[1].transform.position.z,
           (originalLat - minLat) / maxDiffLat

            )
        );
        return corrected;
    }

    /// <summary>
    /// Creates node objects for a desired Layout
    /// </summary>
    /// <param name="layout"></param>
    /// <returns>A list of GameObjects for the nodes</returns>
    List<GameObject> CreateNodeObjects(Layout layout)
    {
        List<GameObject> result = new List<GameObject>();
        foreach (var node in nodes)
        {
            GameObject mark = Instantiate(pre_Node);
            NodeData data = mark.GetComponent<NodeData>();
            
            data.Init(node, layout);

            if (node.EntityType == "Group")
            {
                mark.GetComponent<Renderer>().material.color = GroupColor;
                groupSymbols.Add(mark);
            }
            else
            {
                mark.GetComponent<Renderer>().material.color = ChannelColor;
                channelSymbols.Add(mark);
            }

            result.Add(mark);

        }

        switch (layout)
        {
            case Layout.Network:
                Parent(result, nodeParentNetwork);
                break;
            case Layout.Geospatial:
                Parent(result, nodeParentGeo);
                break;
            default:
                break;
        }

        return result;
    }

    /// <summary>
    /// Rotates a node parent by a specified amount of degrees
    /// </summary>
    private void RotateNodeParent()
    {
        nodeParentNetwork.Rotate(new Vector3(0, 90, 0));
    }

    /// <summary>
    /// Sets the size of each node given the number of sent messages in the entity
    /// </summary>
    /// <param name="nodes">A list of all the node objects</param>
    /// <param name="layout">A desired Layout</param>
    void SizeNodes(List<GameObject> nodes, Layout layout)
    {
        List<float> messages = new List<float>();
        foreach (var n in nodes)
        {
            if (layout == Layout.Geospatial)
            {
                if (n.GetComponent<NodeData>().latitude != 0) continue;

            }
            messages.Add(n.GetComponent<NodeData>().messages);
        }
        float max = Mathf.Max(messages.ToArray());

        foreach (var n in nodes)
        {
            n.gameObject.transform.localScale = new Vector3(
                Mathf.Lerp(minNodeSize, maxNodeSize, n.GetComponent<NodeData>().messages / max),
                Mathf.Lerp(minNodeSize, maxNodeSize, n.GetComponent<NodeData>().messages / max),
                Mathf.Lerp(minNodeSize, maxNodeSize, n.GetComponent<NodeData>().messages / max)
            );
        }
    }
}
