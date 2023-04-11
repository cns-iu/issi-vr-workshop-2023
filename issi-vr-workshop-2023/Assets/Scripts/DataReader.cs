using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class DataReader : MonoBehaviour
{
    public static DataReader Instance;
    public List<Node> Nodes { get; set; }
    public List<Edge> Edges { get; set; }

    [field: SerializeField] private HashSet<Node> nodesTemp = new HashSet<Node>();

    [Header("Files")]
    [SerializeField] private string edgeList = "";
    [SerializeField] private string activity = "";

    [Header("Counts")]
    [field: SerializeField] private Dictionary<string, string> nameToEntityTypeMapping = new Dictionary<string, string>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        Nodes = new List<Node>();
        Edges = new List<Edge>();
        ReadCSV();
        ConvertToList();
        ForNodesGetEntityType();
        ForNodesGetMonthlyActionsAndLatLon();
    }

    void ReadCSV()
    {
        using (var reader = Utils.ReadCsv(edgeList))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                if (line.Split(',')[0] != "from_name")
                {
                    Node newNodeCol1 = new Node(line.Split(',')[0], new Vector3(float.Parse(line.Split(',')[4]), float.Parse(line.Split(',')[5]), float.Parse(line.Split(',')[6])));
                    Node newNodeCol2 = new Node(line.Split(',')[1], new Vector3(float.Parse(line.Split(',')[7]), float.Parse(line.Split(',')[8]), float.Parse(line.Split(',')[9])));
                    nodesTemp.Add(newNodeCol1);
                    nodesTemp.Add(newNodeCol2);

                    Edge newEdge = new Edge(line.Split(',')[0], line.Split(',')[1], int.Parse(line.Split(',')[2]), float.Parse(line.Split(',')[3]));
                    Edges.Add(newEdge);
                }
            }
        }
    }

    void ConvertToList()
    {
        foreach (var item in nodesTemp)
        {
            Nodes.Add(item);
        }
    }

    void ForNodesGetEntityType()
    {
        using (var reader = Utils.ReadCsv(activity))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                if (line.Split(',')[0] != "HANDLE")
                {
                    string id = line.Split(',')[0];
                    if (!nameToEntityTypeMapping.ContainsKey(id))
                    {
                        nameToEntityTypeMapping.Add(line.Split(',')[0], line.Split(',')[2]);
                    };
                }
            }
        }

        for (int i = 0; i < Nodes.Count; i++)
        {
            Node n = Nodes[i];
            if (nameToEntityTypeMapping.ContainsKey(Nodes[i].Id)) n.EntityType = nameToEntityTypeMapping[Nodes[i].Id];
            Nodes[i] = n;
        }
    }



    void ForNodesGetMonthlyActionsAndLatLon()
    {
        using (var reader = Utils.ReadCsv(activity))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                if (line.Split(',')[0] != "HANDLE")
                {
                    for (int i = 0; i < Nodes.Count; i++)
                    {
                        Node n = Nodes[i];
                        if (n.MonthlyActions == null)
                        {
                            n.MonthlyActions = new MonthlyActionWrapper();
                            n.MonthlyActions.Wrapper = new List<Activity>();
                        }

                        if (line.Split(',')[0] == n.Id)
                        {
                            n.MonthlyActions.Wrapper.Add(new Activity(line.Split(',')[0], line.Split(',')[1], line.Split(',')[2], Convert.ToInt32(line.Split(',')[3]), Convert.ToInt32(line.Split(',')[4]), line.Split(',')[6], line.Split(',')[5]));

                            string value = n.MonthlyActions.Wrapper[0].Latitude;
                            if (value != "")
                            {
                                n.Latitude = float.Parse(n.MonthlyActions.Wrapper[0].Latitude);
                                n.Longitude = float.Parse(n.MonthlyActions.Wrapper[0].Longitude);
                            }
                            else
                            {
                                n.Latitude = 0f;
                                n.Longitude = 0f;
                            }

                        }
                        Nodes[i] = n;
                    }
                }
            }
        }
    }
}

[Serializable]
public class Activity
{
    [field: SerializeField] public string SentAt { get; private set; }
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public string Group { get; private set; }
    [field: SerializeField] public int PostsTotal { get; private set; }
    [field: SerializeField] public int ActiveUsers { get; private set; }
    [field: SerializeField] public string Latitude { get; private set; }
    [field: SerializeField] public string Longitude { get; private set; }

    public Activity(string id, string sentAt, string group, int postsTotal, int activeUsers, string latitude, string longitude)
    {
        this.Id = id;
        this.SentAt = sentAt;
        this.Group = group;
        this.PostsTotal = postsTotal;
        this.ActiveUsers = activeUsers;
        this.Latitude = latitude;
        this.Longitude = longitude;
    }

}

[Serializable]
public class MonthlyActionWrapper
{
    [SerializeField] public List<Activity> Wrapper;
}

[Serializable]
public struct Node
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public string EntityType { get; set; }

    [field: SerializeField] public Vector3 Position { get; private set; }
    [field: SerializeField] public MonthlyActionWrapper MonthlyActions { get; set; }
    [field: SerializeField] public float Latitude { get; set; }
    [field: SerializeField] public float Longitude { get; set; }

    public Node(string id, Vector3 position)
    {
        this.Id = id;
        this.Position = position;
        this.EntityType = "";
        this.MonthlyActions = null;
        this.Latitude = 0f;
        this.Longitude = 0f;
    }
}

[Serializable]
public class Edge
{
    [field: SerializeField] public string SourceID { get; private set; }
    [field: SerializeField] public string TargetID { get; private set; }
    [field: SerializeField] public float Weight { get; private set; }
    [field: SerializeField] public int TimeStep { get; private set; }

    public Edge(string sourceID, string targetID, int sentAtQuarter, float weight)
    {
        this.SourceID = sourceID;
        this.TargetID = targetID;
        this.TimeStep = sentAtQuarter;
        this.Weight = weight;
    }
}

[Serializable]
public struct City
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public float Latitude { get; private set; }
    [field: SerializeField] public float Longitude { get; private set; }
    [field: SerializeField] public int Population { get; private set; }

    public City(string Name, float lat, float lon, int pop)
    {
        this.Name = Name;
        this.Latitude = lat;
        this.Longitude = lon;
        this.Population = pop;
    }
}



