using Algine.AI.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Algine.Zombie.Npc
{
    public class Attacking : IState
    {
        private readonly Animator m_animator;
        private readonly NavMeshAgent m_agent;
        private readonly AudioSource m_audioSource;

        private readonly Transform m_itSelf;
        private readonly  AudioClip m_rageClip;
        private readonly LayerMask m_playerMask;
        private readonly ZombieVision m_zombieVision;

        private float m_agentSpeed = 4f;
        private float m_attackDistance = 2f;
        private float m_lookSpeed = 3f;


        public bool IsOnThisState { get; private set; }

        public Attacking(Transform itself, ZombieVision vision,AudioClip clip,
            float runSpeed,float attackdistance, LayerMask mask)
        {
            m_itSelf = itself;
            m_animator = itself.GetComponent<Animator>();
            m_agent = itself.GetComponent<NavMeshAgent>();
            m_audioSource = itself.GetComponent<AudioSource>();
            m_rageClip = clip;

            m_agentSpeed = runSpeed;
            m_playerMask = mask;
            m_zombieVision = vision;
            m_attackDistance = attackdistance;
            IsOnThisState = false;
        }
        public void OnEnter()
        {
            m_agent.speed = m_agentSpeed;
            IsOnThisState = true;

            m_audioSource.Stop();
            m_audioSource.clip = m_rageClip;
            m_audioSource.loop = true;
            m_audioSource.Play();
        }

        public void OnExit()
        {
            m_animator.SetBool("Run", false);
            m_animator.SetBool("Attack", false);

            IsOnThisState = false;
        }

        public void Tick()
        {
            if ((m_zombieVision.Player.position - m_itSelf.position).magnitude > m_attackDistance)
            {
                m_animator.SetBool("Run", true);
                m_animator.SetBool("Attack", false);

                m_agent.speed = m_agentSpeed;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(m_zombieVision.Player.position, 
                    out hit, m_attackDistance, 1))
                {
                    m_agent.SetDestination(hit.position);
                }
            }
            else
            {
                m_animator.SetBool("Run", false);
                
                m_itSelf.rotation = Quaternion.Euler(0,
                        Mathf.Lerp(m_itSelf.rotation.eulerAngles.y,
                        Quaternion.LookRotation(m_zombieVision.Player.position
                        - m_itSelf.position).eulerAngles.y,
                        Time.deltaTime * m_lookSpeed), 0);

                if (m_animator.GetBool("Attack"))
                {
                    applyAttack();
                }
                if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    m_agent.speed = 0;
                    m_agent.velocity = Vector3.zero;
                    m_animator.SetBool("Attack",true);

                }
            }
            
        }

        private void applyAttack()
        {
            RaycastHit hit;
            if (Physics.Raycast(m_itSelf.position + m_itSelf.up *.5f,
                m_itSelf.forward, out hit, m_attackDistance, m_playerMask))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    hit.transform.SendMessage("Damage", 1,
                        SendMessageOptions.DontRequireReceiver);
                }
            }
        }

    }

}
