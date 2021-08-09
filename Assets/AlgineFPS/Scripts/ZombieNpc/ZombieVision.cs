using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Algine.Zombie.Npc
{
    public class ZombieVision : MonoBehaviour
    {
        [SerializeField]
        private float PlayerCheckingInterval = 0.5f;
        [SerializeField]
        private float FOV = 100;
        [SerializeField]
        private float DetectionRange = 100;
        [SerializeField]
        private LayerMask PlayerDetectionMask;
        [SerializeField]
        private LayerMask DeathDetectionMask;
        [SerializeField]
        private float DeathCheckRange = 20f;
        [SerializeField]
        private bool HasEatingMode = false;

        public bool IsPlayerVisible { get; private set; }
        public bool IsDeathVisible { get; private set; }
        public Transform DeathTransform { get; private set; }
        public Transform Player { get; private set; }
       
        private bool IsAlreadyVisit = false;

        private void Awake()
        {
            IsDeathVisible = false;
            IsPlayerVisible = false;

            Player = GameObject.FindGameObjectWithTag("Player").transform;

            if (HasEatingMode)
            {
                DeathTransform = GameObject.FindGameObjectWithTag("Death").transform;

            }

        }

        private void Start()
        {
            StartCoroutine(playerChecking(PlayerCheckingInterval));
            if (HasEatingMode)
            {
                StartCoroutine(deathChecking(PlayerCheckingInterval));

            }
        }

        IEnumerator deathChecking(float time)
        {
            while (true)
            {
                Vector3 direction = DeathTransform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);

                RaycastHit hit;
                if (angle < FOV * 0.5f)
                {
                    if (Physics.Raycast(transform.position,
                        direction, out hit, DetectionRange, DeathDetectionMask))
                    {
                        if (hit.collider.CompareTag("Death") && !IsAlreadyVisit)
                        {
                            //Debug.DrawLine(transform.position, DeathTransform.position, Color.green);
                            IsDeathVisible = true;
                            IsAlreadyVisit = true;
                        }
                        else
                        {
                            IsDeathVisible = false;
                            //Debug.DrawLine(transform.position, DeathTransform.position, Color.red);
                        }
                    }
                }
                else
                {
                    IsDeathVisible = false;
                    //Debug.DrawLine(transform.position, DeathTransform.position, Color.red);
                }


                yield return new WaitForSeconds(time);
            }

        }

        IEnumerator playerChecking(float time)
        {
            while (true)
            {
                Vector3 direction = Player.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);

                RaycastHit hit;
                if (angle < FOV * 0.5f)
                {
                    if (Physics.Raycast(transform.position,
                        direction, out hit, DetectionRange, PlayerDetectionMask))
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            //Debug.DrawLine(transform.position, Player.position, Color.green);
                            IsPlayerVisible = true;
                        }
                        else
                        {
                            IsPlayerVisible = false;
                            //Debug.DrawLine(transform.position, Player.position, Color.red);
                        }
                    }
                }
                else
                {
                    IsPlayerVisible = false;
                    //Debug.DrawLine(transform.position, Player.position, Color.red);
                }


                yield return new WaitForSeconds(time);
            }
        }
    }

}