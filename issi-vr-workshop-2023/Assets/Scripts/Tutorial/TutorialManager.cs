using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndreasBueckle.Assets.Scripts.Tutorial
{
    /// <summary>
    /// A class to manage all the components related to the Tutorial
    /// </summary>
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance;
        public AudioSource audioSource;
        public AudioClip[] introChapters;
        public AudioClip[] vizChapters;
        public int finishedIntro = 0;
        public int finishedViz = 0;
        public string chapPlaying;
        public bool replay = false;
        public bool idleAnim = true;
        public bool animTrigger = true;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this; 
            }
        }

        /// <summary>
        /// Plays the given list of audio chapters sequentially
        /// </summary>
        /// <returns>an IEnumerator yielding between playing each audio clip</returns>
        public IEnumerator playAudioSequentially(AudioClip[] audioClips)
        {
            yield return null;

            if (audioSource.isPlaying == false) {
                for (int i = 0; i < audioClips.Length; i++)
                {
                    audioSource.clip = audioClips[i];

                    audioSource.PlayDelayed(0.5f);
                    //audioSource.Play();
                                        
                    while (audioSource.isPlaying)
                    {
                        yield return null;
                    }

                    //if intro is playing, increment intro count, same for viz
                    if (chapPlaying == "intro" && finishedIntro < 3)
                    {
                        finishedIntro += 1;
                    }
                    if (chapPlaying == "viz" && finishedViz < 2)
                    {
                        finishedViz += 1;
                    }

                }
            }
            else
            {
                while (audioSource.isPlaying)
                {
                    yield return null;
                }

                ////if intro is playing, inc intro count, same for viz
                //if (chapPlaying == "intro" && finishedIntro < 3)
                //{
                //    finishedIntro += 1;
                //    Debug.Log("Increment finishedIntro, = " + finishedIntro);
                //}
                //if (chapPlaying == "viz" && finishedViz < 2)
                //{
                //    finishedViz += 1;
                //    Debug.Log("Increment finishedviz, = " + finishedViz);
                //}

                StartCoroutine(playAudioSequentially(audioClips));
            }

        }

        /// <summary>
        /// Method to choose which chapters to play based on where the user is standing
        /// </summary>
        public void ChooseAudio(string collided)
        {
            if(collided == "IntroNavMarker"){
                chapPlaying = "intro";
                StartCoroutine(playAudioSequentially(introChapters));
            }
            else if (collided == "VizNavMarker"){
                chapPlaying = "viz";
                StartCoroutine(playAudioSequentially(vizChapters));
            }
        }
    }
}
