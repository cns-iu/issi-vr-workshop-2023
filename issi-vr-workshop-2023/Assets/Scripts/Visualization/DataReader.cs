using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

namespace AndreasBueckle.Assets.Scripts.Visualization
{
    /// <summary>
    /// A class to read data from a CSV file and make the data available to the rest of the application
    /// </summary>
    public class DataReader : MonoBehaviour
    {
        public static DataReader Instance;
        public List<Node> Nodes { get; set; }
        public List<Edge> Edges { get; set; }

        private HashSet<Node> nodesTemp = new HashSet<Node>();

        [Header("Files")]
        [SerializeField] private string edgeList = "";
        [SerializeField] private string activity = "";

        [Header("Counts")]
        [field: SerializeField] private Dictionary<string, string> nameToEntityTypeMapping = new Dictionary<string, string>();

        /// <summary>
        /// This function is called before the first frame of the application
        /// </summary>
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
            ReadEdgeList();
            Nodes = nodesTemp.ToList();
            ReadNodeList();
        }

        /// <summary>
        /// Reads an edge list from a specified path, parses lines, creates edges, and stores them in a collection
        /// </summary>
        void ReadEdgeList()
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

        /// <summary>
        /// Reads an node list from a specified path, parses lines, creates nodes, and stores them in a collection
        /// </summary>
        void ReadNodeList()
        {
            int messages = 0, activeUsers = 0;
            float lat = 0, lon = 0;
            string id = "", location = "", type = "";

            using (var reader = Utils.ReadCsv(activity))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (line.Split(',')[0] != "node")
                    {
                        id = line.Split(',')[0];
                        type = line.Split(',')[1];

                        if (!nameToEntityTypeMapping.ContainsKey(id))
                        {
                            nameToEntityTypeMapping.Add(id, type);
                        };

                        //int.TryParse(line.Split(',')[2], out activeUsers);
                        location = line.Split(',')[2];
                        float.TryParse(line.Split(',')[4], out lat);
                        float.TryParse(line.Split(',')[3], out lon);
                        int.TryParse(line.Split(',')[5], out messages);
                    }
                    for (int i = 0; i < Nodes.Count; i++)
                    {
                        Node n = Nodes[i];
                        if (n.Id == id)
                        {
                            n.EntityType = type; n.Latitude = lat; n.Longitude = lon; n.Messages = messages; n.Location = location;
                        }
                        Nodes[i] = n;
                    }
                }
            }
        }


    }

    /// <summary>
    /// A <c>struct</c> to describe a node and its properties
    /// </summary>
    [Serializable]
    public struct Node
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string EntityType { get; set; }

        [field: SerializeField] public Vector3 Position { get; private set; }
        [field: SerializeField] public int ActiveUsers { get; set; }
        [field: SerializeField] public int Messages { get; set; }
        [field: SerializeField] public float Latitude { get; set; }
        [field: SerializeField] public float Longitude { get; set; }
        [field: SerializeField] public string Location { get; set; }

        /// <summary>
        /// Constructs a new node
        /// </summary>
        /// <param name="id">node ID</param>
        /// <param name="position">pre-computed3D position</param>
        public Node(string id, Vector3 position)
        {
            Id = id;
            Position = position;
            EntityType = "";
            Messages = 0;
            ActiveUsers = 0;
            Latitude = 0f;
            Longitude = 0f;
            Location = "";
        }
    }

    /// <summary>
    /// A <c>struct</c> to describe an edge and its properties
    /// </summary>  
    [Serializable]
    public class Edge
    {
        [field: SerializeField] public string SourceID { get; private set; }
        [field: SerializeField] public string TargetID { get; private set; }
        [field: SerializeField] public float Weight { get; private set; }
        [field: SerializeField] public int TimeStep { get; private set; }

        /// <summary>
        /// A constructor for an edge
        /// </summary>
        /// <param name="sourceID">ID of the source node</param>
        /// <param name="targetID">ID of the target node</param>
        /// <param name="sentAtQuarter">time step of the edge</param>
        /// <param name="weight">weight of the edge (strength of reference)</param>
        public Edge(string sourceID, string targetID, int sentAtQuarter, float weight)
        {
            SourceID = sourceID;
            TargetID = targetID;
            TimeStep = sentAtQuarter;
            Weight = weight;
        }
    }
}