using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;

public enum Layout { Network, Geospatial };

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

    [Header("Prefabs")]
    [field: SerializeField] private GameObject pre_Node;
    [field: SerializeField] private GameObject pre_Edge;

    [Header("Visual Encoding")]
    [field: SerializeField] private Color groupColor;
    [field: SerializeField] private Color channelColor;
    [field: SerializeField] private float _maxStartWidth;
    [field: SerializeField] private float _maxEndWidth;
    [field: SerializeField] private float _minStartWidth;
    [field: SerializeField] private float _minEndWidth;
    [SerializeField] private float _maxWeight;
    [field: SerializeField] private Color edgeEndColor;
    [field: SerializeField] private Color edgeStartColor;
    [field: SerializeField] private float minNodeSize;
    [field: SerializeField] private float maxNodeSize;


    void Start()
    {
        GetLists();

        NodeObjectsNetwork = CreateNodeObjects(Layout.Network);
        NodeObjectsGeospatial = CreateNodeObjects(Layout.Geospatial);

        LayOutNodes(Layout.Network, NodeObjectsNetwork);
        LayOutNodes(Layout.Geospatial, NodeObjectsGeospatial);
        GetNodeDefaultPositions();

        EdgeObjectsNetwork = CreateEdges();
        EdgeObjectsGeospatial = CreateEdges();

        ForNodesAndEdgesFillConnectionProperties(NodeObjectsNetwork, EdgeObjectsNetwork);
        ForNodesAndEdgesFillConnectionProperties(NodeObjectsGeospatial, EdgeObjectsGeospatial);

        SetEdgePositionsandWidth(EdgeObjectsNetwork);
        SetEdgePositionsandWidth(EdgeObjectsGeospatial);

        SizeNodes(NodeObjectsNetwork, Layout.Network);
        SizeNodes(NodeObjectsGeospatial, Layout.Geospatial);

        RotateNodeParent();
    }

    List<GameObject> CreateEdges()
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
        Parent(result, edgeParentNetwork);
        return result;

    }

    void SetEdgePositionsandWidth(List<GameObject> e)
    {
        _maxWeight = GetMaxWeight(edges);
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

            float startWidth = data.Weight.Remap(
                0f, _maxWeight, _minStartWidth, _maxStartWidth
                );
            float endWidth = data.Weight.Remap(
                0f, _maxWeight, _minEndWidth, _maxEndWidth
                );

            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();

            //set width
            lineRenderer.startWidth = startWidth * edgeScaleFactor;
            lineRenderer.endWidth = endWidth * edgeScaleFactor;

            //set color
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

    void Parent(List<GameObject> objects, Transform parent)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.parent = parent;
        }
    }

    void GetNodeDefaultPositions()
    {
        for (int i = 0; i < NodeObjectsNetwork.Count; i++)
        {
            NodeData data = NodeObjectsNetwork[i].GetComponent<NodeData>();
            data.defaultPosition = NodeObjectsNetwork[i].transform.position;
        }
    }

    void GetLists()
    {
        nodes = DataReader.Instance.Nodes;
        edges = DataReader.Instance.Edges;
    }

    float GetMaxWeight(List<Edge> edges)
    {
        List<float> weights = new List<float>();
        foreach (var item in edges)
        {
            weights.Add(item.Weight);
        }

        return Mathf.Max(weights.ToArray());
    }

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
                    //objects[i].transform.position =
                    //    new Vector3(
                    //        objects[i].GetComponent<NodeData>().latitude,
                    //        0f,
                    //        objects[i].GetComponent<NodeData>().longitude
                    //        ) * .1f;

                    objects[i].transform.position = GetCorrectedLatLonForWorld(
                        objects[i].GetComponent<NodeData>().latitude,
                        objects[i].GetComponent<NodeData>().longitude
                        );
                }
                break;
            default:
                break;
        }

    }

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

    List<GameObject> CreateNodeObjects(Layout layout)
    {
        List<GameObject> result = new List<GameObject>();
        foreach (var node in nodes)
        {
            GameObject mark = Instantiate(pre_Node);
            NodeData data = mark.AddComponent<NodeData>();
            data.Init(node);

            if (node.EntityType == "Group")
            {
                mark.GetComponent<Renderer>().material.color = groupColor;
                groupSymbols.Add(mark);
            }
            else
            {
                mark.GetComponent<Renderer>().material.color = channelColor;
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
                break;
            default:
                break;
        }

        return result;
    }

    private void RotateNodeParent()
    {
        nodeParentNetwork.Rotate(new Vector3(0, 90, 0));
    }

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
