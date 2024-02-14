using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;


namespace AndreasBueckle
{
    public class ControllerDetails : MonoBehaviour
    {
        [SerializeField] private InputActionReference primaryButtonAction;

        public GameObject imageObject;
        public GameObject panelObject;

        //private void OnEnable()
        //{
        //    primaryButtonAction.action.Enable();
        //    primaryButtonAction.action.performed += ToggleImageVisibility;
        //}

        //private void OnDisable()
        //{
        //    primaryButtonAction.action.performed -= ToggleImageVisibility;
        //    primaryButtonAction.action.Disable();
        //}

        //private void ToggleImageVisibility(InputAction.CallbackContext context)
        //{
        //    if (imageObject != null)
        //    {
        //        imageObject.SetActive(!imageObject.activeSelf);
        //    }
        //    if (panelObject != null)
        //    {
        //        panelObject.SetActive(!panelObject.activeSelf);
        //    }
        //}

        private void OnEnable()
        {
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
