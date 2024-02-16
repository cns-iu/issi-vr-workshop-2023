using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndreasBueckle.Assets.Scripts.Tutorial
{
    /// <summary>
    /// A class to manage animations of the Tutorial guide
    /// </summary>
    public class AnimateParticleSystem : MonoBehaviour
    {
        //[SerializeField]
        //public GameObject particleSystem;
        public float moveSpeed= 2;
        Vector3 originPosition;
        Vector3 currentPosition;
        Vector3 targetPosition;
        Vector3 newPosition;
        public float moveDuration = 1f;

        public float amplitude = 20f;
        public float amplitudeOffset = 15f;

        void Awake()
        {
            StartCoroutine(MoveParticleSystem());
        }

        // Start is called before the first frame update
        void Start()
        {
            originPosition = currentPosition = this.transform.position;
            StartCoroutine(MoveParticleSystem());
        }

        // Update is called once per frame
        void Update()
        {
            //condition to start playing particle system animation
            if (TutorialManager.Instance.animTrigger == true)
            {
                StartCoroutine(MoveParticleSystem());
                TutorialManager.Instance.animTrigger = false;
            }
        }

        /// <summary>
        /// Animates the parical system
        /// </summary> 
        IEnumerator MoveParticleSystem()
        {
            //generates random points inside a sphere of radius 0.7 centered at the particle system's position
            //and smoothly interpolates the particle system between those points
            while (TutorialManager.Instance.idleAnim == true)
            {
                targetPosition = Random.insideUnitSphere * 0.7f + originPosition;

                float elapsedTime = 0f;
                while (elapsedTime < moveDuration)
                {
                    float t = elapsedTime / moveDuration;
                    this.transform.position = Vector3.Slerp(currentPosition, targetPosition, t);
                    elapsedTime += Time.deltaTime;
                    currentPosition = this.transform.position;
                    yield return null;
                }

                yield return new WaitForSeconds(moveDuration);
            }
        }
    }
}