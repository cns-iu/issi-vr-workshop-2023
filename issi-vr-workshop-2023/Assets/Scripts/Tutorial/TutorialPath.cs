using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndreasBueckle.Assets.Scripts.Tutorial
{
    /// <summary>
    /// A class to control movement of tutorial guide on the path
    /// </summary>
    public class TutorialPath : MonoBehaviour
    {
        //list of all waypoints which tutorial guide follows
        public Transform[] pathTransform;
        public Vector3[] path;
        LTBezierPath tutorialPath;
        Vector3 originPosition;
        public GameObject IntroNavMarker;
        public GameObject VizNavMarker;

        public GameObject particleSystem;
        private GameObject tutorialGuide;
        //float moveDuration = 2f;

        // Start is called before the first frame update
        void Start()
        {
            IntroNavMarker = GameObject.Find("IntroNavMarker");
            VizNavMarker = GameObject.Find("VisualizationNavMarker");
            tutorialGuide = GameObject.Find("TutorialGuide");
            particleSystem = GameObject.Find("Particle System");
            originPosition = particleSystem.transform.position;
            particleSystem.GetComponent<AnimateParticleSystem>().enabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            //once introduction chapters of tutorial are finished, tutorial guide should guide user into the living room
            if(TutorialManager.Instance.finishedIntro == 3 && TutorialManager.Instance.chapPlaying == "intro")
            {
                TutorialManager.Instance.finishedIntro = 0;
                TutorialManager.Instance.finishedViz = 0;
                TutorialManager.Instance.chapPlaying = "viz";
                TutorialManager.Instance.idleAnim = false;
                particleSystem.GetComponent<AnimateParticleSystem>().enabled = false;

                StartCoroutine(IntroToViz());

            }
            //if user wants to replay tutorial, tutorial guide should guide user to the introduction chapters
            if (TutorialManager.Instance.finishedViz == 0 && TutorialManager.Instance.finishedIntro == 0 && TutorialManager.Instance.chapPlaying == "viz" && TutorialManager.Instance.replay == true)
            {
                StartCoroutine(VizToIntro());
            }
        }

        /// <summary>
        /// Moves the tutorial guide from Introduction to Visualization
        /// </summary>
        /// <returns>an IEnumerator allowing delays in execution until the tutorial guide reaches the destination</returns>
        IEnumerator IntroToViz()
        {
            
            //using lerp
            //int i = 0;
            //while(i < path.Length)
            //{
            //    //Vector3 currentPosition = tutorialGuide.transform.position;
            //    //Vector3 targetPosition = pathTransform[i].position;
            //    Debug.Log("targetPosition" + path[i] + "    Guide Position: "+ tutorialGuide.transform.position);
            //    //float elapsedTime = 0f;
            //    //while (elapsedTime < moveDuration)
            //    //{
            //    //    //float t = elapsedTime / moveDuration;
            //    //    tutorialGuide.transform.position = Vector3.Lerp(currentPosition, targetPosition, 0.9f);
            //    //    elapsedTime += Time.deltaTime;
            //    //}

            //    LeanTween.move(tutorialGuide, path[i], 5f).setEaseInOutSine().setDelay(1f);
            //    i += 1;

            //    yield return new WaitForSeconds(6f);
            //}

            //usine LeanTween
            //bring particle system back to original position before following path
            LeanTween.move(particleSystem, originPosition, 2f);

            //creates a Vector3 array of waypoints for the tutorial guide
            path = new Vector3[] { tutorialGuide.transform.position, pathTransform[1].position, pathTransform[2].position, pathTransform[3].position, pathTransform[3].position, pathTransform[4].position, pathTransform[5].position, pathTransform[6].position, pathTransform[6].position, pathTransform[7].position, pathTransform[8].position, pathTransform[9].position };

            //creates a LeanTween Bezier Path based on the array of waypoints
            tutorialPath = new LTBezierPath(path);

            //tweening between waypoints to make the tutorial guide move along the path
            LeanTween.move(tutorialGuide, tutorialPath.pts, 30f).setEase(LeanTweenType.easeInOutSine).setOrientToPath(true).setDelay(3f);

            yield return new WaitForSeconds(34f);
            tutorialGuide.transform.Rotate(0f, -86f, 0f);

        }

        /// <summary>
        /// Moves the tutorial guide from Visualization to Introduction
        /// </summary>
        /// <returns>an IEnumerator allowing delays in execution until the tutorial guide reaches the destination</returns>
        IEnumerator VizToIntro()
        {
            //LeanTween BezierPath
            path = new Vector3[] { tutorialGuide.transform.position, pathTransform[8].position, pathTransform[7].position, pathTransform[6].position, pathTransform[6].position, pathTransform[5].position, pathTransform[4].position, pathTransform[10].position, pathTransform[10].position, pathTransform[11].position, pathTransform[12].position, pathTransform[0].position };
            tutorialPath = new LTBezierPath(path);

            //tweening between waypoints to make the tutorial guide move along the path
            LeanTween.move(tutorialGuide, tutorialPath.pts, 30f).setEase(LeanTweenType.easeInOutSine).setOrientToPath(true).setDelay(3f);

            //disabling replay boolean to avoid infinite updates in other scripts
            TutorialManager.Instance.replay = false;
            yield return new WaitForSeconds(33f);

            //enabling particle system animations, colliders on nav markers after intiating replay tutorial.
            particleSystem.GetComponent<AnimateParticleSystem>().enabled = true;
            TutorialManager.Instance.idleAnim = true;
            TutorialManager.Instance.animTrigger = true;
            tutorialGuide.transform.Rotate(0f, 30f, 0f);
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
