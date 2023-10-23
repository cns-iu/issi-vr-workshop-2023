using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndreasBueckle.Assets.Scripts.Tutorial
{
    /// <summary>
    /// A class to implement the skip and replay functionalities of the tutorial.
    /// </summary>
    public class SkipTutorial : MonoBehaviour
    {
        public GameObject TutorialAudio;
        public GameObject IntroNavMarker;
        public GameObject VizNavMarker;
        // Start is called before the first frame update
        void Start()
        {
            TutorialAudio = GameObject.Find("TutorialAudio");
            IntroNavMarker = GameObject.Find("IntroNavMarker");
            VizNavMarker = GameObject.Find("VizualizationNavMarker");
        }

    public void SkipChapter()
        {
            if (TutorialManager.Instance.audioSource.isActiveAndEnabled)
            {
                Debug.Log("Skip Chapter");
                TutorialManager.Instance.audioSource.Stop();
            }
        }

        public void SkipToEnd()
        {

            if (TutorialAudio.activeSelf)
            {
                Debug.Log("Skip To End");
                TutorialAudio.SetActive(false);
            }
        }

        public void ReplayTutorial()
        {
            if (!TutorialAudio.activeSelf)
            {
                Debug.Log("Replay Tutorial");
                TutorialAudio.SetActive(true);  
            }
            if(IntroNavMarker.GetComponent<Collider>().enabled == false)
            {
                IntroNavMarker.GetComponent<Collider>().enabled = true;
            }
            if (VizNavMarker.GetComponent<Collider>().enabled == false)
            {
                VizNavMarker.GetComponent<Collider>().enabled = true;
            }
        }
    }
}


//FIXED
// bug: if user skips the chapter, for ex user skips through intro, chap1, chap2, but again collides with the intronavmarker box collider, the audio starts playing from intro.(same will happen with viznavmarker)

// possile solution: when intro audio starts playing disable the box collider on intronavmarker (do the same for viz nav marker)


//next to-dos:

//1. add animations to the particle system, basic ones so that user has something to look at and not just stare at the wall
//2. when chapter 2 ends, the particle system should start moving towards the viz nav marker
//3. 
