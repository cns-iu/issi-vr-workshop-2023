using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float _degreesPerSecond = 6f;
    private void Update()
    {
        transform.Rotate(-_degreesPerSecond * Time.deltaTime, 0f, 0f);
    }
}
