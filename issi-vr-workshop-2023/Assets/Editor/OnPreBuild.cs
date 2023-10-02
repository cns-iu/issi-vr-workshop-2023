using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace AndreasBueckle
{
    public class OnPreBuild : BuildPlayerProcessor
    {
        public int GetcallbackOrder()
        {
            return 0;
        }

        public GameObject xrSimulator = GameObject.Find("XR Device Simulator");
        public GameObject xrSimulatorUI = GameObject.Find("XR Device Simulator UI");
        
        public override void PrepareForBuild(BuildPlayerContext buildPlayerContext)
        {
            //if (xrSimulator != null)
            //{
            //    xrSimulator.SetActive(false);
            //}

            //if (xrSimulatorUI != null)
            //{
            //    xrSimulatorUI.SetActive(false);
            //}
            Debug.Log("Toggled Off");
            //xrSimulator.SetActive(false);
            //xrSimulatorUI.SetActive(false);
        }
    }
}
