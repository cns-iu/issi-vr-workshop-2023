using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace AndreasBueckle.Assets.Scripts.Visualization
{
    /// <summary>
    /// Adds a brush and link functionality, where a node that a user brushes with their ray of on their right hand is highlighted in both visualizations
    /// </summary>
    public class HighlightCorrespondingNode : MonoBehaviour
    {
        [SerializeField] private XRRayInteractor _interactor;
        [SerializeField] private List<Outline[]> _activeOutlines = new List<Outline[]>();
        [SerializeField] List<Color> _colors = new List<Color>();

        private void OnEnable()
        {
            _interactor.hoverEntered.AddListener(SetHighlights);
        }

        /// <summary>
        /// A function to activate the Outline components in both the brushed and the linked node.
        /// </summary>
        /// <param name="args">arguments that need to be taken to match signature of delegate for hover enter event</param>
        void SetHighlights(HoverEnterEventArgs args)
        {

            GameObject node = args.interactableObject.transform.gameObject;
            if (!node.TryGetComponent(out NodeData nodeData)) return;

            node.GetComponent<Outline>().enabled = true;

            switch (nodeData.layout)
            {
                case Layout.Network:
                    IEnumerable<GameObject> network = GetOtherNode(Visualizer.Instance.NodeObjectsGeospatial, nodeData.id);
                    foreach (var o in network)
                    {
                        o.GetComponent<Outline>().enabled = true;
                    }
                    break;
                case Layout.Geospatial:
                    IEnumerable<GameObject> geo = GetOtherNode(Visualizer.Instance.NodeObjectsNetwork, nodeData.id);
                    foreach (var o in geo)
                    {
                        o.GetComponent<Outline>().enabled = true;
                    }
                    break;
                default:
                    break;
            }

            TurnOffOtherNodes(Visualizer.Instance.NodeObjectsGeospatial, nodeData.id);
            TurnOffOtherNodes(Visualizer.Instance.NodeObjectsNetwork, nodeData.id);
        }

        /// <summary>
        /// Turns off nodes that are not highlighted anymore
        /// </summary>
        /// <param name="list">List of GameObjects (nodes)</param>
        /// <param name="id">id of currently highlighted node</param>
        void TurnOffOtherNodes(List<GameObject> list, string id)
        {
            foreach (var item in list)
            {
                item.GetComponent<Outline>().enabled = item.GetComponent<NodeData>().id == id;
            }
        }

        /// <summary>
        /// Finds the corresponding node in the other visualization
        /// </summary>
        /// <param name="list">List of all nodes (GameObjects)</param>
        /// <param name="id">the ID of the highlighted node</param>
        /// <returns>The result of a LINQ query as an IEnumerable with GameObjects</returns>
        IEnumerable<GameObject> GetOtherNode(List<GameObject> list, string id)
        {
            IEnumerable<GameObject> result = list.Where(n => n.GetComponent<NodeData>().id == id).ToList();

            return result;

        }
    }
}