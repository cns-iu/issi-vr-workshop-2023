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
            Debug.Log("Playing Audio");
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
                StartCoroutine(playAudioSequentially(audioClips));
            }
            
        }

        public void ChooseAudio(string collided)
        {
            if(collided == "IntroNavMarker"){
                Debug.Log("On Intro Marker");
                StartCoroutine(playAudioSequentially(introChapters));
            }
            else if (collided == "VizNavMarker"){
                Debug.Log("On Viz Marker");
                StartCoroutine(playAudioSequentially(vizChapters));
            }
        }
    }
}
