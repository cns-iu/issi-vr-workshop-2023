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
        [SerializeField]
        public GameObject particleSystem;
        public float moveSpeed= 2;
        Vector3 originPosition;
        Vector3 currentPosition;
        Vector3 targetPosition;
        Vector3 newPosition;
        public float moveDuration = 1f;

        public float amplitude = 20f;
        public float amplitudeOffset = 15f;

        // Start is called before the first frame update
        void Start()
        {
            originPosition = currentPosition = this.transform.position;
            //newPosition = Random.insideUnitSphere * 0.5f;
            //newPosition = Vector3.Lerp(originPosition, originPosition + Random.insideUnitSphere * 2, 0.5f);
            StartCoroutine(MoveParticleSystem());
            //Debug.Log("Origin Position" + originPosition);
        }

        //private void FixedUpdate()
        //{
        //    Debug.Log("Origin Coord"+ originPosition);

        //    StartCoroutine(MoveParticleSystem());

        //    this.transform.position = newPosition;
        //}

        IEnumerator MoveParticleSystem()
        {
            while (true)
            {
                targetPosition = Random.insideUnitSphere * 0.7f + originPosition;
                //smaller bounding area, and sphere, sinusoidal
                //Vector3 targetPosition = new Vector3(
                //    Random.Range(-3f,2f),
                //    Random.Range(-0.2f,2f),
                //    Random.Range(5f,8f)                    
                //    );
                //targetPosition.z = 0f;

                float elapsedTime = 0f;
                while (elapsedTime < moveDuration)
                {
                    float t = elapsedTime / moveDuration;
                    this.transform.position = Vector3.Slerp(currentPosition, targetPosition, t);
                    elapsedTime += Time.deltaTime;
                    currentPosition = this.transform.position;
                    //this.transform.position = Vector3.Lerp(this.transform.position, originPosition, t);
                    yield return null;
                }

                //lean tween- sinousoidal
                //float elapsedTime = 0f;
                //while (elapsedTime < moveDuration)
                //{

                //    float t = elapsedTime / moveDuration;
                //    newPosition = Vector3.Lerp(currentPosition, targetPosition, t);

                //    //LeanTween.move(particleSystem, newPosition, moveDuration).setEase(LeanTweenType.easeOutElastic).setDelay(0f);
                //    LeanTween.move(particleSystem, newPosition, 0f).setEase(LeanTweenType.easeIOutSine).setDelay(0f);
                //    //LeanTween.move(gameObject, targetPosition, moveDuration)
                //    //.setEase(LeanTweenType.easeInOutSine)
                //    //.setOnUpdate((float t) =>
                //    //{
                //    //    // Calculate the position along the sine wave
                //    //    float sineOffset = Mathf.Sin(t * Mathf.PI) * amplitude;
                //    //    newPosition = Vector3.Lerp(currentPosition, targetPosition, t);
                //    //    newPosition.x += sineOffset;  // Modify X position with the sine wave
                //    //    newPosition.z += sineOffset;
                //    //    transform.position = newPosition;
                //    //});

                //    elapsedTime += Time.deltaTime;
                //    currentPosition = this.transform.position;
                //    //this.transform.position = Vector3.Lerp(this.transform.position, originPosition, t);
                //    yield return null;
                //}

                //sinusoidal interpolation
                //float elapsedTime = 0f;
                //while (elapsedTime < moveDuration)
                //{
                //    elapsedTime += Time.deltaTime;
                //    //var yOffset = new Vector3(0, Mathf.Sin(Time.time * ySpeed) * yAmplitude + yAmplitude, 0);

                //    this.transform.position = Vector3.Lerp(currentPosition, targetPosition, Mathf.Sin(elapsedTime * moveSpeed) * amplitude + amplitudeOffset);
                //    currentPosition = this.transform.position;
                //    yield return null;
                //}


                //float elapsedTime = 0f;
                //while (elapsedTime < moveDuration)
                //{
                //    float t = elapsedTime / moveDuration;

                //    // Calculate the sine wave motion using Mathf.Sin
                //    float waveFactor = Mathf.Sin(t * 1.5f) * amplitude;

                //    // Apply the wave factor to the position
                //    Vector3 wavePosition = Vector3.Lerp(currentPosition, targetPosition, t);
                //    wavePosition.z += waveFactor;
                //    this.transform.position = wavePosition;

                //    elapsedTime += Time.deltaTime;
                //    currentPosition = this.transform.position;

                //    yield return null;
                //}



                //yield return new WaitForSeconds(moveDuration);
            }
        }

        // Update is called once per frame
        void Update()
        {
            //transform.position = originPosition;
            //transform.position = Random.insideUnitSphere * 0.2f;
            //Mathf.PerlinNoise(transform.position.x, transform.position.y);

        }
    }
}

/* Already tried methods:
 * https://www.youtube.com/watch?v=2Y3Y9-Az7oE
 * 
 * Try this for Sinusoidal movement: 
 * https://github.com/shamanland/unity-sinusoidal-moving/blob/master/Assets/Mover.cs 
 * https://stackoverflow.com/questions/24493395/move-a-game-object-between-two-positions-in-a-sin-wave
 * 
 * If this doesn't work go for LeanTween. Documentation: https://dentedpixel.com/LeanTweenDocumentation/classes/LeanTween.html
 * LeanTween Function syntax and explanation: https://www.youtube.com/watch?v=FQTghntprPY&list=PLxHlboWUiQxJl3iNsukMwF5C3vJblazIQ&index=3
 
 */