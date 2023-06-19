using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndreasBueckle.Assets.Scripts.Environment
{
    /// <summary>
    /// Rotates a GameObject's Transform by the specified degrees per second
    /// </summary>
    public class Rotate : MonoBehaviour
    {
        [SerializeField] private float _degreesPerSecond = 6f;
        private void Update()
        {
            transform.Rotate(-_degreesPerSecond * Time.deltaTime, 0f, 0f);
        }
    }
}