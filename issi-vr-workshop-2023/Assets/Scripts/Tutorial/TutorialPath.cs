using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*To-Do: Include these functions in Tutorial manager Script for better functionality*/


namespace AndreasBueckle.Assets.Scripts.Tutorial
{
    public class TutorialPath : MonoBehaviour
    {

        public Transform[] pathTransform;
        public Vector3[] path;
        LTBezierPath tutorialPath;

        private GameObject tutorialGuide;
        float moveDuration = 2f;

        // Start is called before the first frame update
        void Start()
        {
            tutorialGuide = GameObject.Find("TutorialGuide");

            //LeanTween.move(tutorialGuide, pathTransform[2].position, 4f).setEase(LeanTweenType.easeInOutSine);

            //path = new Vector3[] { pathTransform[0].position, pathTransform[1].position, pathTransform[2].position, pathTransform[3].position, pathTransform[4].position, pathTransform[5].position, pathTransform[6].position, pathTransform[7].position};         
            //IntroToViz(path);
            //StartCoroutine(VizToIntro(path));
            //StartCoroutine(IntroToViz(path));

            //LeanTween BezierPath
            //LeanTween.move(tutorialGuide, tutorialPath.pts, 20f).setEase(LeanTweenType.easeInOutSine).setOrientToPath(true).setDelay(2f);
        }

        // Update is called once per frame
        void Update()
        {
            if(TutorialManager.Instance.finishedIntro == 3 && TutorialManager.Instance.chapPlaying == "intro")
            {
                TutorialManager.Instance.finishedIntro = 0;
                TutorialManager.Instance.finishedViz = 0;
                TutorialManager.Instance.chapPlaying = "viz";
                StartCoroutine(IntroToViz());
            }
            if (TutorialManager.Instance.finishedViz == 0 && TutorialManager.Instance.finishedIntro == 0 && TutorialManager.Instance.chapPlaying == "viz" && TutorialManager.Instance.replay == true)
            {
                Debug.Log("Finished Viz, update of Tut path");
                ////TutorialManager.Instance.finishedViz = 0;
                //if (TutorialManager.Instance.replay == true)
                //{
                    Debug.Log("Start Replay, tut to intro");
                //TutorialManager.Instance.finishedIntro = 0;
                //TutorialManager.Instance.finishedViz = 0;
                StartCoroutine(VizToIntro());
                    //TutorialManager.Instance.replay = false;
                //}
            }
        }

        IEnumerator IntroToViz()
        {
            path = new Vector3[] { pathTransform[0].position, pathTransform[1].position, pathTransform[2].position, pathTransform[3].position, pathTransform[4].position, pathTransform[5].position, pathTransform[6].position, pathTransform[7].position };
            //path = new Vector3[] { tutorialGuide.transform.position, pathTransform[1].position, pathTransform[2].position, pathTransform[3].position, pathTransform[4].position, pathTransform[5].position, pathTransform[6].position, pathTransform[7].position };
            Debug.Log("In IntroToViz");
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

            //LeanTween BezierPath
            tutorialPath = new LTBezierPath(path);
            LeanTween.move(tutorialGuide, tutorialPath.pts, 25f).setEase(LeanTweenType.easeInOutSine).setOrientToPath(true).setDelay(5f);
            tutorialGuide.transform.rotation = Quaternion.Euler(0, 90, 0);
            yield return new WaitForSeconds(2f);
        }

        IEnumerator VizToIntro()
        {
            path = new Vector3[] { tutorialGuide.transform.position, pathTransform[6].position, pathTransform[5].position, pathTransform[4].position, pathTransform[3].position, pathTransform[2].position, pathTransform[1].position, pathTransform[0].position };
            Debug.Log("In ViztoIntro");
            //int i = path.Length;
            //while (i > 0)
            //{
            //    //Vector3 currentPosition = tutorialGuide.transform.position;
            //    //Vector3 targetPosition = pathTransform[i].position;
            //    Debug.Log("targetPosition" + path[i] + "    Guide Position: " + tutorialGuide.transform.position);
            //    //float elapsedTime = 0f;
            //    //while (elapsedTime < moveDuration)
            //    //{
            //    //    //float t = elapsedTime / moveDuration;
            //    //    tutorialGuide.transform.position = Vector3.Lerp(currentPosition, targetPosition, 0.9f);
            //    //    elapsedTime += Time.deltaTime;
            //    //}

            //    LeanTween.move(tutorialGuide, path[i], 5f).setEaseInOutSine().setDelay(1f);
            //    i -= 1;

            //    yield return new WaitForSeconds(6f);
            //}

            //LeanTween BezierPath
            tutorialPath = new LTBezierPath(path);
            LeanTween.move(tutorialGuide, tutorialPath.pts, 25f).setEase(LeanTweenType.easeInOutSine).setOrientToPath(true).setDelay(5f);
            tutorialGuide.transform.rotation = Quaternion.Euler(0, 90, 0);
            TutorialManager.Instance.replay = false;
            //TutorialManager.Instance.finishedIntro = 0;
            //TutorialManager.Instance.finishedViz = 0;
            yield return new WaitForSeconds(2f);


        }

    }
}
