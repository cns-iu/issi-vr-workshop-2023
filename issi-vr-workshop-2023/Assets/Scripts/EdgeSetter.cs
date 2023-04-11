using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeSetter : MonoBehaviour
{
    [SerializeField] private EdgeData data;
    [SerializeField] private LineRenderer line;

    private void Start()
    {
        data = GetComponent<EdgeData>();
        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        line.SetPositions(
            new Vector3[2] { data.SourceNode.transform.position, data.TargetNode.transform.position });
    }

}
