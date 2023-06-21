using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AndreasBueckle.Assets.Scripts.Visualization
{
    /// <summary>
    /// A class to hold data for a node. Can be added to a GameObject
    /// </summary>
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

        /// <summary>
        /// A helper function to set the fields of this NodeData using properties for a Node struct. This should be done after
        /// calling the <c>Instantiate()</c> method with the <c>Node</c> prefab.
        /// </summary>
        /// <param name="node">Node struct with data for node to be created</param>
        /// <param name="layout">desired layout to which this node belongs</param>
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