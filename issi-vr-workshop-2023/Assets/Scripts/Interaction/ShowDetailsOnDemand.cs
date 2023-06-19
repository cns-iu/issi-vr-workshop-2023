using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AndreasBueckle.Assets.Scripts.Interaction
{
    public class ShowDetailsOnDemand : MonoBehaviour
    {
        [SerializeField] private List<InputActionReference> references = new List<InputActionReference>();
        // Start is called before the first frame update

        private void OnEnable()
        {
            foreach (var r in references)
            {
                r.action.performed += (context) => { Debug.Log(r.action.name); };
            }
        }
    }
}