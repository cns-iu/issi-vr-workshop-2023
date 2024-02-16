using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndreasBueckle.Assets.Scripts.Tutorial
{
    /// <summary>
    /// A class to control the tutorial narration.
    /// </summary>
    public class PlayAudio : MonoBehaviour
    {
        //[SerializeField]
        GameObject introMarker;
        //[SerializeField]
        //private Transform vizMarker;

        // Start is called before the first frame update
        void Start()
        {

            introMarker = GameObject.FindWithTag("IntroNavMarker");
            OnTriggerEnter(introMarker.GetComponent<Collider>());
            //vizMarker = GameObject.FindWithTag("VizNavMarker");
            
        }
        /// <summary>
        /// Checks where user is standing, plays corresponding tutorial chapters by passing position to ChooseAudio
        /// </summary>
        void OnTriggerEnter(Collider colliderObj)
        {
            //checks if the user is on the Introduction Marker, plays Intro Chapters
            if (colliderObj.CompareTag("IntroNavMarker"))
            {
                TutorialManager.Instance.ChooseAudio("IntroNavMarker");
                colliderObj.enabled = false;
            }
            //checks if user is on the Visualization Marker, plays Visualization Chapters
            else if (colliderObj.CompareTag("VizNavMarker"))
            {
                TutorialManager.Instance.ChooseAudio("VizNavMarker");
                colliderObj.enabled = false;
            }
        }

    }
}
