using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        public AudioSource audioSource;
        public AudioClip followMe;
        public AudioClip feedbackOnClick;
        public TMP_Text displayChapter; 
        public TMP_Dropdown chapterDropdown;
        private string chapterSelected;
        public bool isMuted;

        public GameObject slideShow;

        // Start is called before the first frame update
        void Start()
        {
            TutorialAudio = GameObject.Find("TutorialAudio");
            IntroNavMarker = GameObject.Find("IntroNavMarker");
            VizNavMarker = GameObject.Find("VisualizationNavMarker");
        }

        //Code from Stack Overflow: https://stackoverflow.com/questions/7170909/trim-string-from-the-end-of-a-string-in-net-why-is-this-missing
        public string StringTrimEnd(string input, string suffixToRemove)
        {
            while (input != null && suffixToRemove != null && input.EndsWith(suffixToRemove))
            {
                input = input.Substring(0, input.Length - suffixToRemove.Length);
            }
            return input;
        }
        
        //void Update()
        //{

        //    //Debug.Log("Chapter Playing Now is:"+ TutorialManager.Instance.audioSource.clip.ToString());
        //    if(TutorialManager.Instance.audioSource.isPlaying == true)
        //    {
        //        string chapterName = StringTrimEnd(TutorialManager.Instance.audioSource.clip.ToString(), "(UnityEngine.AudioClip)");
        //        //Debug.Log("Now Playing:" + chapterName);
        //        if (chapterName.StartsWith("Chapter") == true)
        //        {
        //            chapterName = chapterName.Insert(9, ":");
        //        }
        //        displayChapter.SetText(chapterName);
        //    }
        //    else
        //    {
        //        displayChapter.SetText("");
        //    }

        //    //if (TutorialManager.Instance.finishedViz == 2)
        //    //{
        //    //    //if(TutorialAudio.activeSelf == true && TutorialManager.Instance.replay == false)
        //    //    if (TutorialAudio.activeSelf == true)
        //    //    {
        //    //        Debug.Log("Skipped all viz, Setting Audio inactive");
        //    //        TutorialAudio.SetActive(false);
        //    //    }
        //    //    //if (TutorialAudio.activeSelf == false && TutorialManager.Instance.replay == true)
        //    //    //{
        //    //    //    Debug.Log("Should REPLAY");
        //    //    //    TutorialAudio.SetActive(true);
        //    //    //}
        //    //}

        //    if(TutorialManager.Instance.finishedViz == 2 && TutorialAudio.activeSelf == true)
        //    {
        //        Debug.Log("Skipped all viz, Setting Audio inactive");

        //        // Use tutorial path conditions here so that when the guide prepares to moves, it will say follow me and then audio set to inactive and then move.

        //        if (TutorialManager.Instance.chapPlaying == "intro")
        //        {
        //            StartCoroutine(PlayFollowMe());
        //        }

        //        TutorialAudio.SetActive(false);
        //    }
        //    if(TutorialManager.Instance.finishedIntro == 3 && TutorialManager.Instance.chapPlaying == "intro")
        //    {
        //        StartCoroutine(PlayFollowMe());
        //    }
        //}

        public void SkipChapter()
        {
            if (TutorialManager.Instance.audioSource.isActiveAndEnabled)
            {
                TutorialManager.Instance.audioSource.Stop();
                //If I skip to end when second chapter is playing/ right after intro is played, follow me is not working. Fix it here!
                if (TutorialManager.Instance.chapPlaying == "intro" && TutorialManager.Instance.finishedIntro == 2)
                {
                    //TutorialManager.Instance.finishedIntro += 1;
                    Debug.Log("In skip chapter follow me");
                    StartCoroutine(PlayFollowMe());
                }
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

                if(TutorialManager.Instance.chapPlaying == "intro")
                {
                    StartCoroutine(PlayFollowMe());
                }
                TutorialAudio.SetActive(false);
                if (IntroNavMarker.GetComponent<Collider>().enabled == true)
                {
                    IntroNavMarker.GetComponent<Collider>().enabled = false;
                }
                if (VizNavMarker.GetComponent<Collider>().enabled == true)
                {
                    VizNavMarker.GetComponent<Collider>().enabled = false;
                }
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

        IEnumerator PlayFollowMe()
        {
            Debug.Log("In Follow Me Coroutine");
            //TutorialManager.Instance.audioSource.Stop();
            //TutorialManager.Instance.audioSource.clip = TutorialManager.Instance.followMe;
            //TutorialManager.Instance.audioSource.Play();
            yield return new WaitForSeconds(1f);

            audioSource.clip = null;
            audioSource.clip = followMe;
            //audioSource.Play();
            audioSource.PlayDelayed(1f);

            yield return new WaitForSeconds(10f);
        }

        public void PlayFeedbackOnClick()
        {
            if(isMuted == false)
            {
                //Royalty Free Audio from Pixaby: https://pixabay.com/sound-effects/interface-124464/
                audioSource.clip = feedbackOnClick;
                audioSource.Play();
            }
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
                        StartCoroutine(PlayFollowMe());
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
                //if (IntroNavMarker.GetComponent<Collider>().enabled == false)
                //{
                //    IntroNavMarker.GetComponent<Collider>().enabled = true;
                //}
                //if (VizNavMarker.GetComponent<Collider>().enabled == false)
                //{
                //    VizNavMarker.GetComponent<Collider>().enabled = true;
                //}
            }
        }


        public void SelectChapter()
        {
            Debug.Log("Chapter Selected: " + chapterDropdown.options[chapterDropdown.value].text);
            chapterSelected = chapterDropdown.options[chapterDropdown.value].text;
            TutorialManager.Instance.audioSource.enabled = true;
            //if tutorial audio is inactive, set it to active. if it is active, make it inactive and active again.
            if (TutorialAudio.activeSelf == false)
            {
                TutorialAudio.SetActive(true);
            }
            else if(TutorialAudio.activeSelf == true)
            {
                TutorialAudio.SetActive(false);
                TutorialAudio.SetActive(true);
            }

            TutorialManager.Instance.audioSource.Stop();
            //TutorialAudio.SetActive(false); 
            //TutorialAudio.SetActive(true);
            switch (chapterSelected)
            {
                case "Introduction":
                    //Insert conditions from tutorial path so that when guide is near viz, it will go to intro and play the first three chapters once again? Also, should the guide only play the selected chapter or all the chapters that come after it?
                    //TutorialManager.Instance.audioSource.Stop();
                    //if (TutorialManager.Instance.finishedViz <= 2 && TutorialManager.Instance.finishedIntro == 0 && TutorialManager.Instance.chapPlaying == "viz")
                    //{
                    //    Tutorial
                    //}
                        TutorialManager.Instance.finishedIntro = 0;
                        Debug.Log("Introduction Selected, finishedIntro: " + TutorialManager.Instance.finishedIntro);
                        StartCoroutine(TutorialManager.Instance.playAudioSequentially(TutorialManager.Instance.introChapters[0..]));
 
                    break;

                case "Chapter 1: The Data":
                    //TutorialManager.Instance.audioSource.Stop();
                    TutorialManager.Instance.finishedIntro = 1;
                    Debug.Log("Chapter 1 Selected, finishedIntro: " + TutorialManager.Instance.finishedIntro);
                    StartCoroutine(TutorialManager.Instance.playAudioSequentially(TutorialManager.Instance.introChapters[1..]));
                    break;

                case "Chapter 2: Controls Part 1":
                    //TutorialManager.Instance.audioSource.Stop();
                    TutorialManager.Instance.finishedIntro = 2;
                    Debug.Log("Chapter 2 Selected, finishedIntro: " + TutorialManager.Instance.finishedIntro);
                    StartCoroutine(TutorialManager.Instance.playAudioSequentially(TutorialManager.Instance.introChapters[2..]));
                    break;

                case "Chapter 3: The Visualization":
                    //insert condition that will check what the last played chapter is. if it is intro, Intro to Viz. else, don't use tut path.
                    VizNavMarker.GetComponent<Collider>().enabled = false;
                    TutorialManager.Instance.audioSource.Stop();
                    TutorialManager.Instance.finishedViz = 0;
                    Debug.Log("Chapter 3 Selected, finishedViz: " + TutorialManager.Instance.finishedViz);
                    if (TutorialManager.Instance.chapPlaying == "intro")
                    {
                        TutorialManager.Instance.finishedIntro = 3;
                        TutorialManager.Instance.finishedViz = 0;
                        StartCoroutine(PlayFollowMe());
                        StartCoroutine(WaitUntilViz(TutorialManager.Instance.vizChapters[0..]));
                        Debug.Log("Chap Playing Switch: " + TutorialManager.Instance.chapPlaying);
                        //StartCoroutine(TutorialManager.Instance.playAudioSequentially(TutorialManager.Instance.vizChapters[0..]));
                    }
                    else if(TutorialManager.Instance.chapPlaying == "viz")
                    {
                        TutorialManager.Instance.finishedViz = 0;
                        Debug.Log("Else if: chapPlaying: " + TutorialManager.Instance.chapPlaying + "  finishedViz: " + TutorialManager.Instance.finishedViz);
                        StartCoroutine(TutorialManager.Instance.playAudioSequentially(TutorialManager.Instance.vizChapters[0..]));
                    }
                    //TutorialManager.Instance.finishedIntro = 3;
                    //TutorialManager.Instance.finishedViz = 0;
                    //TutorialManager.Instance.chapPlaying = "intro";
                    //StartCoroutine(PlayFollowMe());
                    //StartCoroutine(TutorialManager.Instance.playAudioSequentially(TutorialManager.Instance.vizChapters[0..]));
                    break;

                case "Chapter 4: Controls Part 2":
                    VizNavMarker.GetComponent<Collider>().enabled = false;
                    TutorialManager.Instance.audioSource.Stop();
                    TutorialManager.Instance.finishedViz = 1;
                    if (TutorialManager.Instance.chapPlaying == "intro")
                    {
                        TutorialManager.Instance.finishedIntro = 3;
                        TutorialManager.Instance.finishedViz = 1;
                        StartCoroutine(PlayFollowMe());
                        StartCoroutine(WaitUntilViz(TutorialManager.Instance.vizChapters[1..]));
                        Debug.Log("Chap Playing Switch: " + TutorialManager.Instance.chapPlaying + " finishedViz: "+ TutorialManager.Instance.finishedViz);
                        //StartCoroutine(TutorialManager.Instance.playAudioSequentially(TutorialManager.Instance.vizChapters[1..]));
                    }
                    else if (TutorialManager.Instance.chapPlaying == "viz")
                    {
                        TutorialManager.Instance.finishedViz = 1;
                        Debug.Log("Else if: chapPlaying: " + TutorialManager.Instance.chapPlaying + "  finishedViz: " + TutorialManager.Instance.finishedViz);
                        StartCoroutine(TutorialManager.Instance.playAudioSequentially(TutorialManager.Instance.vizChapters[1..]));
                    }
                    //TutorialManager.Instance.finishedIntro = 3;
                    //TutorialManager.Instance.finishedViz = 1;
                    //TutorialManager.Instance.chapPlaying = "intro";
                    //Debug.Log("Chapter 4 Selected, finishedViz: " + TutorialManager.Instance.finishedViz);
                    //StartCoroutine(PlayFollowMe());
                    //StartCoroutine(TutorialManager.Instance.playAudioSequentially(TutorialManager.Instance.vizChapters[1..]));
                    break;
            }
        }

        IEnumerator WaitUntilViz(AudioClip[] chaptersToPlay)
        {
            yield return new WaitForSeconds(33f);
            TutorialManager.Instance.chapPlaying = "viz";
            StartCoroutine(TutorialManager.Instance.playAudioSequentially(chaptersToPlay));
            Debug.Log("WaitUntilViz: chapPlaying: " + TutorialManager.Instance.chapPlaying + "  finishedViz: "+ TutorialManager.Instance.finishedViz);
        }

        public void MuteUnmuteFeedback()
        {
            if(isMuted == false)
            {
                isMuted = true;
            }
            else
            {
                isMuted = false;
            }
        }

        public void CloseTutorial()
        {
            if(slideShow.activeSelf == true)
            {
                slideShow.SetActive(false);
            }
        }

        public void OpenTutorial()
        {
            if (slideShow.activeSelf == false)
            {
                slideShow.SetActive(true);

            }
        }

    }
}