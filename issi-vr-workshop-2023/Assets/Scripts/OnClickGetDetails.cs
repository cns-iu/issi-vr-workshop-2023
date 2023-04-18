using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class OnClickGetDetails : MonoBehaviour
{
    [SerializeField] private TMP_Text _detailText;
    private XRRayInteractor _controller;
    
    private void Awake()
    {
        _controller = GetComponent<XRRayInteractor>();
        _controller.selectEntered.AddListener(
            (SelectEnterEventArgs args) =>
            {
                string id = args.interactableObject.transform.gameObject.GetComponent<NodeData>().Id;
                _detailText.text = id;
            }
            );
    }
}
