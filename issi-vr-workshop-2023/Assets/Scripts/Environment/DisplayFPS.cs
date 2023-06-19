using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AndreasBueckle.Assets.Scripts.Environment
{
    /// <summary>
    /// Displays the frames per second in a specified interval
    /// </summary>
    public class DisplayFPS : MonoBehaviour
    {
        [SerializeField] private float updateDelay = 0.1f;
        private float _targetFPS = 72f;
        private float _currentFPS = 0f;
        private float _deltaTime = 0f;
        private TextMeshProUGUI _textFPS;

        // Start is called before the first frame update
        void Start()
        {
            _textFPS = GetComponent<TextMeshProUGUI>();
            StartCoroutine(DisplayFramesPerSecond());
        }

        // Update is called once per frame
        void Update()
        {
            GenerateFramesPerSecond();
        }

        /// <summary>
        /// Computes frames per second
        /// </summary
        private void GenerateFramesPerSecond()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * .1f;
            _currentFPS = 1.0f / _deltaTime;
        }

        /// <summary>
        /// Displays the current frames per second in a soecified interval
        /// </summary>
        /// <returns>an IEnumerator (WaitForSeconds) with the specified update interval</returns>
        private IEnumerator DisplayFramesPerSecond()
        {
            while (true)
            {
                if (_currentFPS >= _targetFPS)
                {
                    _textFPS.color = new Color32(0, 177, 215, 255);
                }
                else
                {
                    _textFPS.color = new Color32(200, 68, 124, 255);
                }
                _textFPS.text = "FPS: " + _currentFPS.ToString(".0");
                yield return new WaitForSeconds(updateDelay);
            }
        }
    }
}