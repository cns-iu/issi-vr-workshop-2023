using AndreasBueckle.Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace AndreasBueckle.Assets.Scripts.Interaction
{
    public class HighlightCorrespondingNode : MonoBehaviour
    {
        [SerializeField] private XRRayInteractor _interactor;
        [SerializeField] private List<Outline[]> _activeOutlines = new List<Outline[]>();
        [SerializeField] List<Color> _colors = new List<Color>();

        private void OnEnable()
        {
            _interactor.hoverEntered.AddListener(SetHighlights);
        }

        void SetHighlights(HoverEnterEventArgs args)
        {

            GameObject node = args.interactableObject.transform.gameObject;
            if (!node.TryGetComponent<NodeData>(out NodeData nodeData)) return;

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

        void TurnOffOtherNodes(List<GameObject> list, string id)
        {
            foreach (var item in list)
            {
                item.GetComponent<Outline>().enabled = item.GetComponent<NodeData>().id == id;
            }
        }


        IEnumerable<GameObject> GetOtherNode(List<GameObject> list, string id)
        {
            IEnumerable<GameObject> result = list.Where(n => n.GetComponent<NodeData>().id == id).ToList();

            return result;

        }
    }
}