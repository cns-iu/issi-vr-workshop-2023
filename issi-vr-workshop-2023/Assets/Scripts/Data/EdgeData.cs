using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndreasBueckle.Assets.Scripts.Data
{
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