using AndreasBueckle.Assets.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace AndreasBueckle.Assets.Scripts.Visualization
{
    public class CorrelationVisualizer : MonoBehaviour
    {
        [Header("Controllers")]
        [SerializeField] private XRRayInteractor _interactor;

        [Header("Data")]
        [SerializeField] private Visualizer _visualizer;
        [SerializeField] private string _timestep;
        [SerializeField] private SimilarityMatrix _matrix;
        [SerializeField] private NodeData _data;
        //[SerializeField] private List<GameObject> _nodes = new List<NodeData>();

        [Header("Colors")]
        [SerializeField] private Color _lowCorrelation;
        [SerializeField] private Color _highCorrelation;
        // example for subscribing with delegate
        //private UnityAction<SelectEnterEventArgs> _selectEnter;

        private void OnEnable()
        {
            _interactor.selectEntered.AddListener(GetSimilaritiesForSelected);

            ////example for subscribing with lambda
            //_interactor.selectEntered.AddListener((SelectEnterEventArgs args) => { });
            ////example for subscribing with delegate
            //_interactor.selectEntered.AddListener(_selectEnter);
        }

        private void GetSimilaritiesForSelected(SelectEnterEventArgs args)
        {
            _data = args.interactableObject.transform.gameObject.GetComponent<NodeData>();
            _matrix = Resources.Load<SimilarityMatrix>($"SimilarityMatrices/{_data.id}");
            ColorNodes(_matrix, _data);
        }

        private void ColorNodes(SimilarityMatrix matrix, NodeData data)
        {
            for (int i = 0; i < Visualizer.Instance.NodeObjectsGeospatial.Count; i++)
            {
                for (int k = 0; k < matrix.rows.Count; k++)
                {
                    if (matrix.rows[k].id == Visualizer.Instance.NodeObjectsGeospatial[i].GetComponent<NodeData>().id)
                    {
                        MeshRenderer renderer = Visualizer.Instance.NodeObjectsGeospatial[i].GetComponent<MeshRenderer>();
                        float saturation = matrix.rows[k].simValue;
                        renderer.material.color = Color.Lerp(_lowCorrelation, _highCorrelation, saturation);
                    }
                }
            }
        }

        private void Start()
        {

            //_interactor = GetComponent<XRRayInteractor>();


            // example for subscribing with delegate
            //_selectEnter = MyFunc;
        }

        private void MyFunc(SelectEnterEventArgs args)
        {

        }

    }
}