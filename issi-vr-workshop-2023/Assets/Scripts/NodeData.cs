using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class NodeData : MonoBehaviour
{
    public string Id;
    public string EntityType;
    public Vector3 DefaultPosition;
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public List<GameObject> OutgoingEdges = new List<GameObject>();
    public List<GameObject> IncomingEdges = new List<GameObject>();
    public int activeUsers;
    public int Messages;
    public float Latitude;
    public float Longitude;
    public string Location;

    public void Init(Node node)
    {
        this.Id = node.Id;
        this.EntityType = node.EntityType;
        this.DefaultPosition = node.Position;
        this.activeUsers = node.ActiveUsers;
        this.Latitude = node.Latitude;
        this.Longitude = node.Longitude;
        this.Location = node.Location;
        this.Messages = node.Messages;
    }
}
