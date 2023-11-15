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
        public Vector3[] path1;
        public Vector3[] path2;
        LTBezierPath tutorialPath;
        Vector3 originPosition;

        public GameObject particleSystem;
        private GameObject tutorialGuide;
        //float moveDuration = 2f;

        // Start is called before the first frame update
        void Start()
        {
            tutorialGuide = GameObject.Find("TutorialGuide");
            particleSystem = GameObject.Find("Particle System");
            originPosition = particleSystem.transform.position;
            particleSystem.GetComponent<AnimateParticleSystem>().enabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            if(TutorialManager.Instance.finishedIntro == 3 && TutorialManager.Instance.chapPlaying == "intro")
            {
                TutorialManager.Instance.finishedIntro = 0;
                TutorialManager.Instance.finishedViz = 0;
                TutorialManager.Instance.chapPlaying = "viz";
                TutorialManager.Instance.idleAnim = false;
                particleSystem.GetComponent<AnimateParticleSystem>().enabled = false;

                StartCoroutine(IntroToViz());

            }
            if (TutorialManager.Instance.finishedViz == 0 && TutorialManager.Instance.finishedIntro == 0 && TutorialManager.Instance.chapPlaying == "viz" && TutorialManager.Instance.replay == true)
            {
                Debug.Log("Finished Viz, update of Tut path");
                    Debug.Log("Start Replay, tut to intro");
                StartCoroutine(VizToIntro());
                
            }
        }

        IEnumerator IntroToViz()
        {
            
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

            //bring particle system back to original position before following path
            LeanTween.move(particleSystem, originPosition, 2f);

            ////LeanTween BezierPath
            //tutorialPath = new LTBezierPath(path1);
            //LeanTween.move(tutorialGuide, tutorialPath.pts, 20f).setEase(LeanTweenType.easeInOutSine).setOrientToPath(true).setDelay(3f);

            //LeanTween.move(tutorialGuide, pathTransform[4].position, 3f).setDelay(23f);
            ////LeanTween.move(tutorialGuide, pathTransform[5].position, 3f).setDelay(26f);

            //tutorialPath = new LTBezierPath(path2);
            //LeanTween.move(tutorialGuide, tutorialPath.pts, 20f).setEase(LeanTweenType.easeInOutSine).setOrientToPath(true).setDelay(24f);


            path = new Vector3[] { tutorialGuide.transform.position, pathTransform[1].position, pathTransform[2].position, pathTransform[3].position, pathTransform[3].position, pathTransform[4].position, pathTransform[5].position, pathTransform[6].position, pathTransform[6].position, pathTransform[7].position, pathTransform[8].position, pathTransform[9].position };

            tutorialPath = new LTBezierPath(path);
            LeanTween.move(tutorialGuide, tutorialPath.pts, 30f).setEase(LeanTweenType.easeInOutSine).setOrientToPath(true).setDelay(3f);


            yield return new WaitForSeconds(34f);
            Debug.Log("After reaching viz");
            tutorialGuide.transform.Rotate(0f, -86f, 0f);
            //tutorialGuide.transform.Rotate(0f, 90f, 0f);

        }

        IEnumerator VizToIntro()
        {
            Debug.Log("In ViztoIntro");
            //LeanTween BezierPath
            path = new Vector3[] { tutorialGuide.transform.position, pathTransform[8].position, pathTransform[7].position, pathTransform[6].position, pathTransform[6].position, pathTransform[5].position, pathTransform[4].position, pathTransform[10].position, pathTransform[10].position, pathTransform[11].position, pathTransform[12].position, pathTransform[0].position };
            tutorialPath = new LTBezierPath(path);
            LeanTween.move(tutorialGuide, tutorialPath.pts, 30f).setEase(LeanTweenType.easeInOutSine).setOrientToPath(true).setDelay(3f);

            TutorialManager.Instance.replay = false;
            yield return new WaitForSeconds(33f);

            particleSystem.GetComponent<AnimateParticleSystem>().enabled = true;
            TutorialManager.Instance.idleAnim = true;
            TutorialManager.Instance.animTrigger = true;
            tutorialGuide.transform.Rotate(0f, 30f, 0f);

        }

    }
}
