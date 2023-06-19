using AndreasBueckle.Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndreasBueckle.Assets.Scripts.Visualization
{
    public class EdgeSetter : MonoBehaviour
    {
        [SerializeField] private EdgeData data;
        [SerializeField] private LineRenderer line;

        private void Awake()
        {
            data = GetComponent<EdgeData>();
            line = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if (data.TargetNode == null) return;
            line.SetPositions(
                new Vector3[2] { data.SourceNode.transform.position, data.TargetNode.transform.position });
        }

    }
}