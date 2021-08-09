using Algine.FPS.MobileInput;
using UnityEngine;

namespace Algine
{
    public class Sway : MonoBehaviour
    {
        
        [Tooltip("Sway smooth amount")]
        [SerializeField]
        [Range(.1f,1f)]
        private float SmoothAmount = 1.0f;
        [SerializeField]
        [Range(.01f, 1f)]
        private float MaxAmount = .08f;
        [SerializeField]
        [Range(.01f,0.5f)]
        private float SwayAmount = .04f;

        private Vector3 m_init_pos;

        private Vector2 m_touchDir;


        private void Start()
        {
            m_init_pos = transform.localPosition;
            InputEvents.Current.OnTouchLook += onTouchLook;
        }
        private void onTouchLook(Vector2 dir)
        {
            m_touchDir = dir;
        }
        private void OnDestroy()
        {
            InputEvents.Current.OnTouchLook -= onTouchLook;

        }
        private void Update()
        {
            float fx = -m_touchDir.x * SwayAmount;
            float fy = -m_touchDir.y * SwayAmount;

            fx = Mathf.Clamp(fx, -MaxAmount, MaxAmount);
            fy = Mathf.Clamp(fy, -MaxAmount, MaxAmount);

            //Calculating sway vector 
            Vector3 swayVector = new Vector3(fx, 
                fy, 0);

            transform.localPosition = Vector3.Lerp(transform.localPosition,
                swayVector + m_init_pos,Time.deltaTime*SmoothAmount);
        }
        
    }
}
