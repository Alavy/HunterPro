
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Algine
{
    public class NPCVision : MonoBehaviour
    {
        [Tooltip("How often vision will check if player is in sight")]
        private float visibilityCheckInterval = 0.5f;
        [Tooltip("NPC field of fiev range")]
        public float FOV = 100;
        [Tooltip("How far NPC can look")]
        public float detectionRange = 30;
        private Transform player;

        public bool isPlayerVisible;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

            CallVisibleMethod();

        }

        public void CallVisibleMethod()
        {
            StartCoroutine(VisibilityCheck(visibilityCheckInterval));
        }

        //Simple coroutine that calling with interval step
        IEnumerator VisibilityCheck(float visibilityCheckInterval)
        {
            while(true)
            {
                Vector3 direction = player.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);

                RaycastHit hit;

                if (angle < FOV * 0.5f)
                {
                    if (Physics.Raycast(transform.position, direction, out hit, detectionRange))
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            Debug.DrawLine(transform.position, player.transform.position, Color.green);
                            isPlayerVisible = true;
                        }
                        else
                        {
                            isPlayerVisible = false;
                            Debug.DrawLine(transform.position, player.transform.position, Color.red);
                        }
                    }
                }
                else
                {
                    isPlayerVisible = false;
                    Debug.DrawLine(transform.position, player.transform.position, Color.red);
                }
                yield return new WaitForSeconds(visibilityCheckInterval);
            }
        }
    }
}
