#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

public class RuntimeScript : MonoBehaviour
#if UNITY_EDITOR
    , IPreprocessBuildWithReport
    , IPostprocessBuildWithReport
#endif
{
#if UNITY_EDITOR
    public int callbackOrder { get { return 0; } }

    private GameObject[] objectsToDisable;

    //public void Start()
    //{
    //    objectsToDisable = GameObject.FindGameObjectsWithTag("EditorOnly");
    //}

    public void OnPreprocessBuild(BuildReport report)
    {
        objectsToDisable = GameObject.FindGameObjectsWithTag("EditorOnly");
        Debug.Log("Disabling objects before building...");
        SetObjectState(false);
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        Debug.Log("Enabling objects after building...");
        SetObjectState(true);
    }

    //use this to turn off entire game objects. Note that FindGameObjectsWithTag will not find inactive game objects
    void SetObjectState(bool newState)
    {
        //GameObject[] objectsToDisable = GameObject.FindGameObjectsWithTag("EditorOnly");

        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(newState);
        }
    }
#endif
}
