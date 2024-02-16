using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AndreasBueckle.Assets.Scripts.Tutorial
{
    /// <summary>
    /// A class to manage the tutorial slide show
    /// </summary>
    public class SlideShow : MonoBehaviour
    {
        public List<GameObject> panels;
        //public Image slideImage;
        public Button nextSlide;
        public Button prevSlide;
        private int currentSlide;

        // Start is called before the first frame update
        void Start()
        {
            currentSlide = 0;
            nextSlide = GameObject.Find("NextSlide").GetComponent<Button>();
            prevSlide = GameObject.Find("PrevSlide").GetComponent<Button>();

            //disabling all other slides except the first one
            panels[0].SetActive(true);
            for(int i = 1; i < panels.Count; i++)
            {
                panels[i].SetActive(false);
            }
        }

        // Update is called once per frame
        void Update() 
        {
            //disabling prev/next slide arrow based on the index of slide being displayed
            if(currentSlide == 0)
            {
                prevSlide.gameObject.SetActive(false);
            }
            else
            {
                if(prevSlide.gameObject.activeSelf == false)
                {
                    prevSlide.gameObject.SetActive(true);
                }
            }
            if(currentSlide == panels.Count - 1)
                {
                nextSlide.gameObject.SetActive(false);
            }
            else
            {
                if (nextSlide.gameObject.activeSelf == false)
                {
                    nextSlide.gameObject.SetActive(true);
                }
            }
            //for (int i = 0; i < panels.Count && i != currentSlide; i++)
            //{
            //    if (panels[i].activeSelf == true)
            //    {
            //        panels[i].SetActive(false);
            //    }
            //}
        }


        /// <summary>
        /// Disabling the current slide, enabling the nect slide in the list when next button is pressed.
        /// </summary>
        public void NextSlide()
        {
            if( currentSlide < panels.Count)
            {
                currentSlide += 1;
                panels[currentSlide].SetActive(true);
                for (int i = 0; i < panels.Count && i != currentSlide; i++)
                {
                    panels[i].SetActive(false);
                }
            }
        }

        /// <summary>
        /// Disabling the current slide, enabling the prev slide in the list when previous button is pressed
        /// </summary>
        public void PrevSlide()
        {
            if (currentSlide > 0)
            {
                currentSlide -= 1;
                panels[currentSlide].SetActive(true);
                for (int i = 0; i < panels.Count; i++)
                {
                    if( i == currentSlide)
                    {
                        continue;
                    }
                    panels[i].SetActive(false);
                }
            }
        }
    }
}
