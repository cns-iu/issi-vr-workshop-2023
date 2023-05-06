using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class NodeData : MonoBehaviour
{
    public string id;
    public string entityType;
    public Vector3 defaultPosition;
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public List<GameObject> OutgoingEdges = new List<GameObject>();
    public List<GameObject> IncomingEdges = new List<GameObject>();
    public int activeUsers;
    public int messages;
    public float latitude;
    public float longitude;
    public string location;
    public Layout layout;

    public void Init(Node node, Layout layout)
    {
        this.id = node.Id;
        this.entityType = node.EntityType;
        this.defaultPosition = node.Position;
        this.activeUsers = node.ActiveUsers;
        this.latitude = node.Latitude;
        this.longitude = node.Longitude;
        this.location = node.Location;
        this.messages = node.Messages;
        this.layout = layout;
    }
}
