using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour
{
    [SerializeField] private Toggle _muteButton;
    [SerializeField] private Sprite _mute;
    [SerializeField] private Sprite _play;

    private void OnEnable()
    {
        _muteButton.onValueChanged.AddListener(
            (bool doMute) =>
            {
                if (doMute) { GetComponent<Image>().sprite = _play; } else { GetComponent<Image>().sprite = _mute; };
            });
    }
}
