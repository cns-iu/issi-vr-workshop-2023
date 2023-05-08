using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
