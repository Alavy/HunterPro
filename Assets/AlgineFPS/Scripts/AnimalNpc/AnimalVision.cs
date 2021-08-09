using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Algine.Animal.Npc
{
    public class AnimalVision : MonoBehaviour
    {
        [SerializeField]
        private float VisibilityCheckInterval = 0.5f;
        [SerializeField]
        public float FOV = 100;
        [SerializeField]
        private float detectionRange = 30;
        [SerializeField]
        private LayerMask PlayerMask;

        private Transform m_player;

        public bool IsPlayerVisible { get; private set; }

        private void Awake()
        {
            m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        private void Start()
        {
            CallVisibleMethod();

        }

        public void CallVisibleMethod()
        {
            StartCoroutine(VisibilityCheck(VisibilityCheckInterval));
        }

        IEnumerator VisibilityCheck(float visibilityCheckInterval)
        {
            while (true)
            {
                Vector3 direction = m_player.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);

                RaycastHit hit;

                if (angle < FOV * 0.5f)
                {
                    if (Physics.SphereCast(transform.position, 3f, direction, out hit,
                        detectionRange, PlayerMask))
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            Debug.DrawLine(transform.position, m_player.transform.position, Color.green);
                            IsPlayerVisible = true;
                        }
                        else
                        {
                            IsPlayerVisible = false;
                            Debug.DrawLine(transform.position, m_player.transform.position, Color.red);
                        }
                    }
                }
                else
                {
                    IsPlayerVisible = false;
                    Debug.DrawLine(transform.position, m_player.transform.position, Color.red);
                }
                yield return new WaitForSeconds(visibilityCheckInterval);
            }
        }
    }

}