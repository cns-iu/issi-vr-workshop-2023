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
        //public GameObject user;
        //public Vector2 userPosition;

        // Start is called before the first frame update
        void Start()
        {
            //user = GameObject.FindWithTag("MainCamera");
            //audioSource.PlayDelayed(2);
            //StartCoroutine(playAudioSequentially(chapters));

        }

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

        // Update is called once per frame
        void Update()
        {
            //userPosition = new Vector2(user.transform.position.x, user.transform.position.z);
            //Debug.Log("User Position" + userPosition);
        }

        IEnumerator playAudioSequentially(AudioClip[] audioClips)
        {
            Debug.Log("Playing Audio, finishedIntro: " + finishedIntro + " finishedViz: "+ finishedViz);
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

                    

                }
            }
            else
            {
                while (audioSource.isPlaying)
                {
                    yield return null;
                }
                //if intro is playing, inc intro count, same for viz
                if (chapPlaying == "intro" && finishedIntro < 3)
                {
                    finishedIntro += 1;
                    Debug.Log("Increment finishedIntro, = " + finishedIntro);
                }
                if (chapPlaying == "viz" && finishedViz < 2)
                {
                    finishedViz += 1;
                    Debug.Log("Increment finishedviz, = " + finishedViz);
                }
                StartCoroutine(playAudioSequentially(audioClips));
            }

            //if (chapPlaying == "intro")
            //{
            //    finishedIntro = 3;
            //    Debug.Log("End Intro Chapters, finishedIntro= " + finishedIntro);
            //}
            //else if (chapPlaying == "viz")
            //{
            //    finishedViz = 2;
            //    Debug.Log("End Viz Chapters, finishedViz= " + finishedViz);
            //}

            if (chapPlaying == "intro" && finishedIntro == 3)
            {
                Debug.Log("End Intro Chapters, finishedIntro= " + finishedIntro);
            }
            else if (chapPlaying == "viz" && finishedViz == 2)
            {
                Debug.Log("End Viz Chapters, finishedViz= " + finishedViz);
            }

        }

        public void ChooseAudio(string collided)
        {
            if(collided == "IntroNavMarker"){
                Debug.Log("On Intro Marker");
                chapPlaying = "intro";
                StartCoroutine(playAudioSequentially(introChapters));
            }
            else if (collided == "VizNavMarker"){
                Debug.Log("On Viz Marker");
                chapPlaying = "viz";
                StartCoroutine(playAudioSequentially(vizChapters));
            }
        }
    }
}
