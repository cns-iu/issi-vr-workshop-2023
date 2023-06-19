using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AndreasBueckle.Assets.Scripts.Data
{
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
            id = node.Id;
            entityType = node.EntityType;
            defaultPosition = node.Position;
            activeUsers = node.ActiveUsers;
            latitude = node.Latitude;
            longitude = node.Longitude;
            location = node.Location;
            messages = node.Messages;
            this.layout = layout;
        }
    }
}