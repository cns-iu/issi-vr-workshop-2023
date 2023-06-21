using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AndreasBueckle.Assets.Scripts.Interaction
{
    /// <summary>
    /// A test class to print the currently performed action (from a list) to the console
    /// </summary>
    public class ShowDetailsOnDemand : MonoBehaviour
    {
        [SerializeField] private List<InputActionReference> references = new List<InputActionReference>();
        

        private void OnEnable()
        {
            ///subscribe to all actions with a lambda function that prints the name of the performed action
            foreach (var r in references)
            {
                r.action.performed += (context) => { Debug.Log(r.action.name); };
            }
        }
    }
}