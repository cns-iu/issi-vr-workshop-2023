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
            VizNavMarker = GameObject.Find("VisualizationNavMarker");
        }
        void Update()
        {
            //if (TutorialManager.Instance.finishedViz == 2)
            //{
            //    //if(TutorialAudio.activeSelf == true && TutorialManager.Instance.replay == false)
            //    if (TutorialAudio.activeSelf == true)
            //    {
            //        Debug.Log("Skipped all viz, Setting Audio inactive");
            //        TutorialAudio.SetActive(false);
            //    }
            //    //if (TutorialAudio.activeSelf == false && TutorialManager.Instance.replay == true)
            //    //{
            //    //    Debug.Log("Should REPLAY");
            //    //    TutorialAudio.SetActive(true);
            //    //}
            //}

            if(TutorialManager.Instance.finishedViz == 2 && TutorialAudio.activeSelf == true)
            {
                Debug.Log("Skipped all viz, Setting Audio inactive");
                TutorialAudio.SetActive(false);
            }
        }

        public void SkipChapter()
        {
            if (TutorialManager.Instance.audioSource.isActiveAndEnabled)
            {
                TutorialManager.Instance.audioSource.Stop();
                //if(TutorialManager.Instance.chapPlaying == "intro")
                //{
                //    TutorialManager.Instance.finishedIntro += 1;
                //}
                //else if (TutorialManager.Instance.chapPlaying == "viz")
                //{
                //    TutorialManager.Instance.finishedViz += 1;
                //}
                Debug.Log("Skip Chapter, intro: " + TutorialManager.Instance.finishedIntro + " viz: " + TutorialManager.Instance.finishedViz);
            }
        }

        public void SkipToEnd()
        {
            if (TutorialAudio.activeSelf)
            {
                Debug.Log("Skip To End");
                TutorialAudio.SetActive(false);
            }
            TutorialManager.Instance.finishedIntro = 3;
            TutorialManager.Instance.finishedViz = 2;
            //if (TutorialManager.Instance.chapPlaying == "intro")
            //{
            //    TutorialManager.Instance.finishedIntro = 3;
            //}
            //if(TutorialManager.Instance.chapPlaying == "viz")
            //{
            //    TutorialManager.Instance.finishedViz = 2;
            //}
        }

        public void ReplayTutorial()
        {
            if(TutorialManager.Instance.replay == false)
            {
                if (TutorialAudio.activeSelf == false)
                {
                    Debug.Log("Replay Tutorial");
                    TutorialAudio.SetActive(true);

                    if (TutorialManager.Instance.chapPlaying == "viz")
                    {
                        Debug.Log("Inside replay= true");
                        //TutorialManager.Instance.finishedViz = 2;
                        //TutorialManager.Instance.finishedIntro = 3;
                        TutorialManager.Instance.finishedViz = 0;
                        TutorialManager.Instance.finishedIntro = 0;
                        TutorialManager.Instance.replay = true;
                    }
                    //else if(TutorialManager.Instance.chapPlaying == "intro")
                    //{

                    //}
                }
                if (IntroNavMarker.GetComponent<Collider>().enabled == false)
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
}

