using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;


namespace AndreasBueckle
{
    /// <summary>
    /// A class to toggle the controller schema and information panel on the right controller
    /// </summary>
    public class ControllerDetails : MonoBehaviour
    {
        [SerializeField] private InputActionReference primaryButtonAction;

        public GameObject imageObject;
        public GameObject panelObject;

        private void OnEnable()
        {
            //subscribe to the primary button pressed event to toggle the controller schema and information panel
            primaryButtonAction.action.performed += (context) =>
            {
                if (imageObject != null)
                {
                    imageObject.SetActive(!imageObject.activeSelf);
                }
                if (panelObject != null)
                {
                    panelObject.SetActive(!panelObject.activeSelf);
                }
            };
        }

    }
}
