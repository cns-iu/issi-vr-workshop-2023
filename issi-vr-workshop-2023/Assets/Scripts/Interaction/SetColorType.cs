using AndreasBueckle.Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AndreasBueckle.Assets.Scripts.Interaction
{
    /// <summary>
    /// A class to handle resetting the color type of the geospatial nodes when a button has been clicked
    /// </summary>
    public class SetColorType : MonoBehaviour
    {
        private void OnEnable()
        {
            Visualizer.Instance.ResetButton.onClick.AddListener(
            () =>
            {
                if (GetComponent<NodeData>().entityType == "Group")
                {
                    GetComponent<Renderer>().material.color = Visualizer.Instance.GroupColor;
                }
                else
                {
                    GetComponent<Renderer>().material.color = Visualizer.Instance.ChannelColor;
                }
            }
                );
        }
    }
}