using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace AndreasBueckle.Assets.Scripts.Environment
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        [SerializeField] private Toggle _muteButton;

        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        [Header("Audio Clips")]
        [SerializeField] private AudioSource _ambience;

        private void Start()
        {
            _ambience.Play();
        }

        private void OnEnable()
        {
            _muteButton.onValueChanged.AddListener(
                (isOn) =>
                {
                    if (isOn) { _ambience.Stop(); } else { _ambience.Play(); }
                });
        }
    }
}