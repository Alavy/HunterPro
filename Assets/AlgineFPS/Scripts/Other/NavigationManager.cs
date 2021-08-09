using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Algine
{


    public class NavigationManager : MonoBehaviour
    {
        public RawImage Compass_Bar;
        public TextMeshProUGUI Compass_text;
        public Transform rotationReference;

        void Start()
        {
            if (Compass_Bar == null || Compass_text == null)
            {
                Debug.LogError("We can not find the Compasbar and Text ");
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            ///< for compass bar 
            ///
            UpdateCompassBar();
        }

        private void UpdateCompassBar()
        {
            // set compass bar texture coordinates
            Compass_Bar.uvRect = new Rect(
                (rotationReference.eulerAngles.y / 360f) - .5f,
                0f, 1f, 1f);
            if (rotationReference.eulerAngles.y <= 0 && rotationReference.eulerAngles.y >= -180)
            {
                Compass_text.text = (rotationReference.eulerAngles.y + 360).ToString("f0");
            }
            else
            {
                Compass_text.text = (rotationReference.eulerAngles.y).ToString("f0");
            }
        }
        public void UpdateIndicatorElement(RectTransform element, Vector3 screenPos)
        {
            float offset = 100;
            if (screenPos.z < 0f)
            {
                element.gameObject.SetActive(false);
               
            }
            else
            {
                element.gameObject.SetActive(true);
                Vector3 pos = new Vector3
                {
                    x = Mathf.Clamp(screenPos.x, offset, Screen.width- offset),
                    y = Mathf.Clamp(screenPos.y, offset, Screen.height- offset),
                    z = screenPos.z
                };
                element.position = pos;
            }
           
        }

    }
}