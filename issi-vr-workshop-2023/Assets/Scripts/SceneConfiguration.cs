using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public enum AppState { Network, Geospatial }

public class SceneConfiguration : MonoBehaviour
{

    public static event Action<SceneConfigurationChange> SceneConfigChange;

    [Header("Scene configuration")]
    [field: SerializeField] public AppState AppState;
    [field: SerializeField] public int StartTimeStep { get; set; } = 0;
    [field: SerializeField] public int EndTimeStep { get; set; } = 19;

    [Header("References in scene")]
    [SerializeField] private Visualizer visualizer;
    [SerializeField] private List<GameObject> edgeObjects;
    [SerializeField] private Slider[] sliders = new Slider[2];
    [SerializeField] private Button filterButton = null;
    [SerializeField] private Button resetButton = null;

    [Header("Debug")]
    [SerializeField] private InputActionReference filterByKeyboard = null;

    private void OnEnable()
    {
        filterButton.onClick.AddListener(
           () => { FilterByTimeStep((int)sliders[0].value, (int)sliders[1].value); }
            );

       

        //filterByKeyboard.action.performed += (ctx) =>
        //{
        //    StartTimeStep = (int)sliders[0].value; EndTimeStep = (int)sliders[1].value;
        //    FilterByTimeStep();
        //};
    }

    private void Start()
    {
        edgeObjects = visualizer.EdgeObjectsNetwork;
    }

    public void FilterByTimeStep(float start, float end)
    {
        StartTimeStep = (int)start;
        EndTimeStep = (int)end;
        for (int i = 0; i < edgeObjects.Count; i++)
        {
            EdgeData e = edgeObjects[i].GetComponent<EdgeData>();
            e.gameObject.SetActive(e.TimeStep >= StartTimeStep && e.TimeStep <= EndTimeStep);
        }
    }

    ////delete when not using keyboard for debugging any longer
    //public void FilterByTimeStep(InputAction.CallbackContext context)
    //{
    //    Debug.Log("pressed" + context.action + " " + context.control);
    //    Filter();
    //}
}

public class SceneConfigurationChange
{
    [field: SerializeField] public AppState NewState { get; set; }
    [field: SerializeField] public int StartTimeStep { get; set; }
    [field: SerializeField] public int EndTimeStep { get; set; }
}
