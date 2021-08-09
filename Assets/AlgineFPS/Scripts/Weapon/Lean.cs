
using Algine.FPS.MobileInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Algine
{

    public class Lean : MonoBehaviour
    {
        [Header("Lean Settings")]
        public float LeanRotationSpeed = 80f;
        public float LeanPositionSpeed = 3f;
        public float MaxAngle = 30f;
        public float LeanPositionShift = 0.1f;
        public float CheckCollisionDistance = 0.1f;

        private float m_leanCurrentAngle = 0f;
        private Vector3 m_leanVelocity = Vector3.zero;

        private bool m_leanLeft = false;
        private bool m_leanRight = false;

        public void LeanToDefaultState()
        {
            m_leanLeft = false;
            m_leanRight = false;
        }

        private void Start()
        {
            InputEvents.Current.OnLeanLeft += OnLeanLeft;
            InputEvents.Current.OnLeanRight += OnLeanRight;
        }
        private void OnDestroy()
        {
            InputEvents.Current.OnLeanLeft -= OnLeanLeft;
            InputEvents.Current.OnLeanRight -= OnLeanRight;
        }
        private void OnLeanLeft(bool state)
        {
            if (state)
            {
                if (!m_leanRight)
                {
                    m_leanLeft = !m_leanLeft;
                }
                
            }
            
        }
        private void OnLeanRight(bool state)
        {
            if (state)
            {
                if (!m_leanLeft)
                {
                    m_leanRight = !m_leanRight;
                }
               
            }
        }
        private void Update()
        {
            if (m_leanLeft)
            {
                RaycastHit hit;

                if (!Physics.Raycast(transform.position, -transform.right, out hit, CheckCollisionDistance)){
                    var temp_leanPositionShift = LeanPositionShift;
                    m_leanCurrentAngle = Mathf.MoveTowardsAngle(m_leanCurrentAngle, MaxAngle, LeanRotationSpeed * Time.smoothDeltaTime);
                    transform.localPosition = Vector3.SmoothDamp(transform.localPosition, new Vector3(-temp_leanPositionShift, 0, 0), ref m_leanVelocity, LeanPositionSpeed * Time.smoothDeltaTime);
                }else
                {
                    var temp_leanPositionShift = Vector3.Distance(transform.position, hit.point)/1.5f;
                    m_leanCurrentAngle = Mathf.MoveTowardsAngle(m_leanCurrentAngle, MaxAngle/3, LeanRotationSpeed * Time.smoothDeltaTime);
                    transform.localPosition = Vector3.SmoothDamp(transform.localPosition, new Vector3(-temp_leanPositionShift, 0, 0), ref m_leanVelocity, LeanPositionSpeed * Time.smoothDeltaTime);
                }
            }
            else if (m_leanRight)
            {
                RaycastHit hit;

                if (!Physics.Raycast(transform.position, transform.right, out hit, CheckCollisionDistance))
                {
                    var temp_leanPositionShift = LeanPositionShift;
                    m_leanCurrentAngle = Mathf.MoveTowardsAngle(m_leanCurrentAngle, -MaxAngle, LeanRotationSpeed * Time.smoothDeltaTime);
                    transform.localPosition = Vector3.SmoothDamp(transform.localPosition, new Vector3(temp_leanPositionShift, 0, 0), ref m_leanVelocity, LeanPositionSpeed * Time.smoothDeltaTime);
                }
                else
                {
                    var temp_leanPositionShift = Vector3.Distance(transform.position, hit.point) / 1.5f;
                    m_leanCurrentAngle = Mathf.MoveTowardsAngle(m_leanCurrentAngle, -MaxAngle / 3, LeanRotationSpeed * Time.smoothDeltaTime);
                    transform.localPosition = Vector3.SmoothDamp(transform.localPosition, new Vector3(temp_leanPositionShift, 0, 0), ref m_leanVelocity, LeanPositionSpeed * Time.smoothDeltaTime);
                }
            }
            else
            {
                m_leanCurrentAngle = Mathf.MoveTowardsAngle(m_leanCurrentAngle, 0f, LeanRotationSpeed * Time.deltaTime);
                transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref m_leanVelocity, LeanPositionSpeed * Time.smoothDeltaTime);
            }

            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, m_leanCurrentAngle));
            
        }
    }
}
