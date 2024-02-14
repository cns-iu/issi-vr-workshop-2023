using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AndreasBueckle.Assets.Scripts.Tutorial
{
    public class SlideShow : MonoBehaviour
    {
        public Sprite[] slides;
        public List<GameObject> panels;
        //public Image slideImage;
        public Button nextSlide;
        public Button prevSlide;
        private int currentSlide;

        // Start is called before the first frame update
        void Start()
        {
            //slideImage.sprite = slides[0];
            currentSlide = 0;
            nextSlide = GameObject.Find("NextSlide").GetComponent<Button>();
            prevSlide = GameObject.Find("PrevSlide").GetComponent<Button>();
            panels[0].SetActive(true);
            for(int i = 1; i < panels.Count; i++)
            {
                panels[i].SetActive(false);
            }
        }

        // Update is called once per frame
        void Update() 
        {
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
            //if(currentSlide == slides.Length - 1)
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

        public void NextSlide()
        {
            if( currentSlide < panels.Count)
            //if( currentSlide < slides.Length)
            {
                currentSlide += 1;
                //slideImage.sprite = slides[currentSlide];
                panels[currentSlide].SetActive(true);
                //Debug.Log("Displaying Panel"+ panels[currentSlide].name);
                //Debug.Log("Current Slide" + currentSlide);
                for (int i = 0; i < panels.Count && i != currentSlide; i++)
                {
                    //Debug.Log("In next slide inactive");
                    panels[i].SetActive(false);
                }
            }
        }

        public void PrevSlide()
        {
            //for (int i = 0; i < panels.Count && i != currentSlide; i++)
            //{
            //    panels[i].SetActive(false);
            //}
            if (currentSlide > 0)
            {
                currentSlide -= 1;
                //slideImage.sprite = slides[currentSlide];
                
                panels[currentSlide].SetActive(true);
                //Debug.Log("Displaying Panel" + panels[currentSlide].name); 
                //Debug.Log("Prev SLide" + panels.Count + " " + currentSlide);
                for (int i = 0; i < panels.Count; i++)
                {
                    //Debug.Log("Debug: " + i);
                    if( i == currentSlide)
                    {
                        continue;
                    }
                    //Debug.Log("in prev slide inactive");
                    panels[i].SetActive(false);
                }
            }
        }
    }
}
