using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using AndreasBueckle.Assets.Scripts.Data;

namespace AndreasBueckle.Assets.Scripts.Interaction
{
    /// <summary>
    /// A class with functionality to get node data from a clicked node
    /// </summary>
    public class OnClickGetDetails : MonoBehaviour
    {
        [SerializeField] private TMP_Text _prompt;
        [SerializeField] private TMP_Text _idText;
        [SerializeField] private TMP_Text _messagesText;
        private XRRayInteractor _controller;

        /// <summary>
        /// Runs before the first frame. Allows the user to subscribe to the selectEntered event of the controller (added to left hand). Retrieves name and number of messages of the clicked node 
        /// and displays those via a text
        /// </summary>
        private void Awake()
        {
            _controller = GetComponent<XRRayInteractor>();
            _controller.selectEntered.AddListener(

                (args) =>
                {
                    _prompt.enabled = false;

                    NodeData data = args.interactableObject.transform.gameObject.GetComponent<NodeData>();
                    string id = data.id;
                    int numMessages = data.messages;
                    _idText.text = $"Node ID: {id}";
                    _messagesText.text = $"Messages: {numMessages}";
                }
                );
        }
    }
}