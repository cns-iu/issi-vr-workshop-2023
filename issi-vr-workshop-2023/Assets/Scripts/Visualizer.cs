using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;

public class Visualizer : MonoBehaviour
{
    [Header("Entities")]
    [field: SerializeField] public List<GameObject> EdgeObjects = new List<GameObject>();
    [field: SerializeField] public List<GameObject> NodeObjects = new List<GameObject>();

    [field: SerializeField] private List<Node> nodes;
    [field: SerializeField] private List<Edge> edges;

    [field: SerializeField] private List<GameObject> groupSymbols = new List<GameObject>();
    [field: SerializeField] private List<GameObject> channelSymbols = new List<GameObject>();


    [Header("Scene Setup")]
    [field: SerializeField] private Transform nodeParent;
    [field: SerializeField] private Transform edgeParent;
    [field: SerializeField] private Vector3 offsetNetwork = new Vector3(0f, 3.4f, 5f);
    [field: SerializeField] private float scalingFactor = 1f;
    [field: SerializeField] private float edgeScaleFactor = 3f;

    [Header("Prefabs")]
    [field: SerializeField] private GameObject pre_Node;
    [field: SerializeField] private GameObject pre_Edge;

    [Header("Visual Encoding")]
    [field: SerializeField] private Color groupColor;
    [field: SerializeField] private Color channelColor;
    [field: SerializeField] private float defaultStartWidth;
    [field: SerializeField] private float defaultEndWidth;
    [field: SerializeField] private Color edgeEndColor;
    [field: SerializeField] private Color edgeStartColor;
    [field: SerializeField] private float minNodeSize;
    [field: SerializeField] private float maxNodeSize;


    void Start()
    {
        GetLists();
        CreateNodeObjects();
        LayOutNodes();
        GetNodeDefaultPositions();
        CreateEdges();
        ForNodesAndEdgesFillConnectionProperties();
        SetEdgePositionsandWidth();
        SizeNodes();
    }


    void CreateEdges()
    {
        foreach (var edge in edges)
        {
            GameObject line = Instantiate(pre_Edge);
            EdgeData data = line.AddComponent<EdgeData>();
            data.Source = edge.SourceID;
            data.Target = edge.TargetID;
            data.Weight = edge.Weight;
            data.TimeStep = edge.TimeStep;

            EdgeObjects.Add(line);
            line.AddComponent<EdgeSetter>();
        }

        Parent(EdgeObjects, edgeParent);
    }

    void SetEdgePositionsandWidth()
    {
        for (int i = 0; i < EdgeObjects.Count; i++)
        {
            GameObject line = EdgeObjects[i];
            EdgeData data = line.GetComponent<EdgeData>();
            EdgeObjects[i].GetComponent<LineRenderer>().SetPositions(
                new Vector3[2]{
                data.SourceNode.transform.position,
                data.TargetNode.transform.position
                }
            );

            float maxWeight = GetMaxWeight(edges);
            float currentWeight = data.Weight / maxWeight;
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();

            //set width
            lineRenderer.startWidth = defaultStartWidth * currentWeight * edgeScaleFactor;
            lineRenderer.endWidth = defaultEndWidth * currentWeight * edgeScaleFactor;

            //set color
            float alpha = 1.0f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(edgeStartColor, 0.1f), new GradientColorKey(edgeEndColor, 0.5f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
            lineRenderer.colorGradient = gradient;

            line.transform.parent = edgeParent.transform;
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
        for (int i = 0; i < NodeObjects.Count; i++)
        {
            NodeData data = NodeObjects[i].GetComponent<NodeData>();
            data.DefaultPosition = NodeObjects[i].transform.position;
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

    void ForNodesAndEdgesFillConnectionProperties()
    {
        for (int i = 0; i < NodeObjects.Count; i++)
        {
            NodeData n = NodeObjects[i].GetComponent<NodeData>();
            for (int k = 0; k < EdgeObjects.Count; k++)
            {
                EdgeData e = EdgeObjects[k].GetComponent<EdgeData>();

                if (e.Source == n.Id)
                {
                    n.OutgoingEdges.Add(e.gameObject);
                    e.SourceNode = n.gameObject;
                }

                if (e.Target == n.Id)
                {
                    n.IncomingEdges.Add(e.gameObject);
                    e.TargetNode = n.gameObject;
                };
            }
        }
    }

    void LayOutNodes()
    {
        for (int i = 0; i < NodeObjects.Count; i++)
        {
            NodeObjects[i].transform.position = NodeObjects[i].GetComponent<NodeData>().DefaultPosition * scalingFactor + offsetNetwork;
        }
    }

    void CreateNodeObjects()
    {
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

            NodeObjects.Add(mark);

        }
        Parent(NodeObjects, nodeParent);
    }

    void SizeNodes()
    {
        List<float> degrees = new List<float>();
        foreach (var item in NodeObjects)
        {
            degrees.Add(item.GetComponent<NodeData>().IncomingEdges.Count + item.GetComponent<NodeData>().OutgoingEdges.Count);
        }
        float max = Mathf.Max(degrees.ToArray());

        foreach (var item in NodeObjects)
        {
            item.gameObject.transform.localScale = new Vector3(
                Mathf.Lerp(minNodeSize, maxNodeSize, item.GetComponent<NodeData>().IncomingEdges.Count / max),
                Mathf.Lerp(minNodeSize, maxNodeSize, item.GetComponent<NodeData>().IncomingEdges.Count / max),
                Mathf.Lerp(minNodeSize, maxNodeSize, item.GetComponent<NodeData>().IncomingEdges.Count / max)
            );
        }
    }
}
