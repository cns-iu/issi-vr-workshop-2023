using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndreasBueckle.Assets.Scripts.Visualization
{
    /// <summary>
    /// This class can be added to a GameObject. It holds public fields describing an edge
    /// </summary>
    public class EdgeData : MonoBehaviour
    {
        public string Source;
        public string Target;
        public float Weight;
        public int TimeStep;
        public GameObject SourceNode;
        public GameObject TargetNode;
    }
}