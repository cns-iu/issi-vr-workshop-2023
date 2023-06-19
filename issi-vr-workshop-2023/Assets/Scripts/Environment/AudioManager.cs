using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace AndreasBueckle.Assets.Scripts.Environment
{
    /// <summary>
    /// A class to handle ambient audio. Allow the user to turn the ambient audio on and off
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        [SerializeField] private Toggle _muteButton;

        private void Awake()
        {
            // If there is an instance, and it's not me, delete me.
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

        //runs during first frame of application
        private void Start()
        {
            _ambience.Play();
        }

        private void OnEnable()
        {
            //subscribes to the onValueChanged event of the mute button to turn audio on and off accoordingly
            _muteButton.onValueChanged.AddListener(
                (isOn) =>
                {
                    if (isOn) { _ambience.Stop(); } else { _ambience.Play(); }
                });
        }
    }
}