using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using AndreasBueckle.Assets.Scripts.Data;

namespace AndreasBueckle.Assets.Scripts.Interaction
{
    public class OnClickGetDetails : MonoBehaviour
    {
        [SerializeField] private TMP_Text _prompt;
        [SerializeField] private TMP_Text _idText;
        [SerializeField] private TMP_Text _messagesText;
        private XRRayInteractor _controller;

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