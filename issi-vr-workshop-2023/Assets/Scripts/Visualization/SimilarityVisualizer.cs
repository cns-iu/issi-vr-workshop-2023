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
    /// <summary>
    /// A class to handle coloring all geospatial nodes depending on their cosine similarity with the selected node
    /// </summary>
    public class SimilarityVisualizer : MonoBehaviour
    {
        [Header("Controllers")]
        [SerializeField] private XRRayInteractor _interactor;

        [Header("Data")]
        [SerializeField] private Visualizer _visualizer;
        [SerializeField] private string _timestep;
        [SerializeField] private SimilarityMatrix _matrix;
        [SerializeField] private NodeData _data;

        [Header("Colors")]
        [SerializeField] private Color _lowCorrelation;
        [SerializeField] private Color _highCorrelation;

        private void OnEnable()
        {
            _interactor.selectEntered.AddListener(GetSimilaritiesForSelected);
        }

        /// <summary>
        /// Method to subscribe to selectEntered of right hand controller, gets a reference to the NodeData of the selected node,
        /// loads the corresponding cosine similarity matrix, and then calls a method to color the node.
        /// </summary>
        /// <param name="args">Select enter event arguments to pass (must take those to subscribe to selectEntered event raised)</param>
        private void GetSimilaritiesForSelected(SelectEnterEventArgs args)
        {
            _data = args.interactableObject.transform.gameObject.GetComponent<NodeData>();
            _matrix = Resources.Load<SimilarityMatrix>($"SimilarityMatrices/{_data.id}");
            ColorNodes(_matrix, _data);
        }

        /// <summary>
        /// Loops through all the nodes and colors them based on their cosine similarity 
        /// </summary>
        /// <param name="matrix">A pre-computed similarity matrix</param>
        /// <param name="data">The NodeData of the selected node</param>
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
    }
}