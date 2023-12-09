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

        //private float introDistance;
        //private float vizDistance;

        //[SerializeField]
        //private Transform user;
        //[SerializeField]
        GameObject introMarker;
        //[SerializeField]
        //private Transform vizMarker;

        // Start is called before the first frame update
        void Start()
        {
            ////user = GameObject.FindWithTag("User");
            //user = GameObject.FindWithTag("MainCamera");

            introMarker = GameObject.FindWithTag("IntroNavMarker");
            OnTriggerEnter(introMarker.GetComponent<Collider>());
            //vizMarker = GameObject.FindWithTag("VizNavMarker");
            //userPosition = new Vector2(user.transform.position.x, user.transform.position.z);
            //introPosition = new Vector2(introMarker.transform.position.x, introMarker.transform.position.z);
            //vizPosition = new Vector2(vizMarker.transform.position.x, vizMarker.transform.position.z);

            //Debug.Log("Intro Position" + introPosition);
            //Debug.Log("Viz Position" + vizPosition);
            //ProximityChecker();
        }

        ////// Update is called once per frame
        void Update()
        {
            //    userPosition = new Vector2(user.transform.position.x, user.transform.position.z);
            //Debug.Log("User to intro distance" + Vector2.Distance(userPosition, vizPosition));
            //Debug.Log($"User to Intro Distance{Vector3.Distance(UnityEngine.XR.InputTracking.GetLocalPosition(0), introMarker.transform.position)}");
            //    Debug.Log("User Position" + userPosition);
            //ProximityChecker();
        }


        void OnTriggerEnter(Collider colliderObj)
        {
            Debug.Log("In Trigger method");
            if (colliderObj.CompareTag("IntroNavMarker"))
            {
                TutorialManager.Instance.ChooseAudio("IntroNavMarker");
                colliderObj.enabled = false;
            }
            else if (colliderObj.CompareTag("VizNavMarker"))
            {
                TutorialManager.Instance.ChooseAudio("VizNavMarker");
                colliderObj.enabled = false;
            }
        }


        //void OnTriggerEnter(Collider colliderObj)
        //{
        //    //checks if the user is on the Introduction Marker, plays Intro Chapters
        //    if (colliderObj.CompareTag("IntroNavMarker"))
        //    {
        //        TutorialManager.Instance.ChooseAudio("IntroNavMarker");
        //        colliderObj.enabled = false;
        //    }
        //    //checks if user is on the Visualization Marker, plays Visualization Chapters
        //    else if (colliderObj.CompareTag("VizNavMarker"))
        //    {
        //        TutorialManager.Instance.ChooseAudio("VizNavMarker");
        //        colliderObj.enabled = false;
        //    }
        //}

        //private void ProximityChecker()
        //{

        //    introDistance = (user.position - introMarker.position).magnitude;
        //    vizDistance = (user.position - vizMarker.position).magnitude;

        //    Debug.Log($"User to Intro Distance{introDistance}");
        //    Debug.Log($"User to Viz Distance{vizDistance}");

        //    if (introDistance < 1.25)
        //    {
        //        TutorialManager.Instance.ChooseAudio("IntroNavMarker");
        //        //ChooseAudio("IntroNavMarker");
        //    }

        //    else if (vizDistance < 1.25)
        //    {
        //        TutorialManager.Instance.ChooseAudio("VizNavMarker");
        //        //ChooseAudio("VizNavMarker");
        //    }
        //}
    }
}
