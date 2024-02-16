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

        /// <summary>
        /// Method to skip an individual chapter of the audio tutorial
        /// </summary>
        public void SkipChapter()
        {
            if (TutorialManager.Instance.audioSource.isActiveAndEnabled)
            {
                TutorialManager.Instance.audioSource.Stop();
                if (TutorialManager.Instance.chapPlaying == "intro" && TutorialManager.Instance.finishedIntro == 2)
                {
                    StartCoroutine(PlayFollowMe());
                }
            }
        }

        /// <summary>
        /// Method to skip the whole audio tutorial
        /// </summary>
        public void SkipToEnd()
        {
            if (TutorialAudio.activeSelf)
            {
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
        }

        /// <summary>
        /// Method to play "Please follow me" audio when tutorial guide is moving
        /// </summary>
        IEnumerator PlayFollowMe()
        {
            yield return new WaitForSeconds(1f);

            audioSource.clip = null;
            audioSource.clip = followMe;
            audioSource.PlayDelayed(1f);

            yield return new WaitForSeconds(10f);
        }

        /// <summary>
        /// Method to play feedback sound when a button is clicked
        /// </summary>
        public void PlayFeedbackOnClick()
        {
            if(isMuted == false)
            {
                //Royalty Free Audio from Pixaby: https://pixabay.com/sound-effects/interface-124464/
                audioSource.clip = feedbackOnClick;
                audioSource.Play();
            }
        }

        /// <summary>
        /// Method to replay audio tutorial
        /// </summary>
        public void ReplayTutorial()
        {
            if(TutorialManager.Instance.replay == false)
            {
                if (TutorialAudio.activeSelf == false)
                {
                    TutorialAudio.SetActive(true);

                    if (TutorialManager.Instance.chapPlaying == "viz")
                    {
                        StartCoroutine(PlayFollowMe());
                        TutorialManager.Instance.finishedViz = 0;
                        TutorialManager.Instance.finishedIntro = 0;
                        TutorialManager.Instance.replay = true;
                    }
                }
            }
        }

        /// <summary>
        /// Method to control chapter dropdown
        /// </summary>
        public void SelectChapter()
        {
            chapterSelected = chapterDropdown.options[chapterDropdown.value].text;
            TutorialManager.Instance.audioSource.enabled = true;
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
            switch (chapterSelected)
            {
                case "Introduction":
                        TutorialManager.Instance.finishedIntro = 0;
                        StartCoroutine(TutorialManager.Instance.playAudioSequentially(TutorialManager.Instance.introChapters[0..]));
                    break;

                case "Chapter 1: The Data":
                    TutorialManager.Instance.finishedIntro = 1;
                    StartCoroutine(TutorialManager.Instance.playAudioSequentially(TutorialManager.Instance.introChapters[1..]));
                    break;

                case "Chapter 2: Controls Part 1":
                    TutorialManager.Instance.finishedIntro = 2;
                    StartCoroutine(TutorialManager.Instance.playAudioSequentially(TutorialManager.Instance.introChapters[2..]));
                    break;

                case "Chapter 3: The Visualization":
                    VizNavMarker.GetComponent<Collider>().enabled = false;
                    TutorialManager.Instance.audioSource.Stop();
                    TutorialManager.Instance.finishedViz = 0;
                    if (TutorialManager.Instance.chapPlaying == "intro")
                    {
                        TutorialManager.Instance.finishedIntro = 3;
                        TutorialManager.Instance.finishedViz = 0;
                        StartCoroutine(PlayFollowMe());
                        StartCoroutine(WaitUntilViz(TutorialManager.Instance.vizChapters[0..]));
                    }
                    else if(TutorialManager.Instance.chapPlaying == "viz")
                    {
                        TutorialManager.Instance.finishedViz = 0;
                        StartCoroutine(TutorialManager.Instance.playAudioSequentially(TutorialManager.Instance.vizChapters[0..]));
                    }
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
                    }
                    else if (TutorialManager.Instance.chapPlaying == "viz")
                    {
                        TutorialManager.Instance.finishedViz = 1;
                        StartCoroutine(TutorialManager.Instance.playAudioSequentially(TutorialManager.Instance.vizChapters[1..]));
                    }
                    break;
            }
        }


        /// <summary>
        /// Coroutine that pauses tuorial audio until tutorial guide reaches the viz
        /// </summary>
        IEnumerator WaitUntilViz(AudioClip[] chaptersToPlay)
        {
            yield return new WaitForSeconds(33f);
            TutorialManager.Instance.chapPlaying = "viz";
            StartCoroutine(TutorialManager.Instance.playAudioSequentially(chaptersToPlay));
        }

        /// <summary>
        /// Method to mute/unmute on-click feedback sounds
        /// </summary>
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

        /// <summary>
        /// Method to close tutorial slides on click
        /// </summary>
        public void CloseTutorial()
        {
            if(slideShow.activeSelf == true)
            {
                slideShow.SetActive(false);
            }
        }

        /// <summary>
        /// Method to open tutorial slides on click
        /// </summary>
        public void OpenTutorial()
        {
            if (slideShow.activeSelf == false)
            {
                slideShow.SetActive(true);

            }
        }

    }
}