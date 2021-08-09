using Algine.AI.State;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Algine.Zombie.Npc
{
    class Eating : IState
    {
        private Animator m_animator;
        private AudioSource m_audioSource;
        private NavMeshAgent m_agent;

        private Transform m_itSelf;
        private ZombieVision m_zombieVision;
        private float m_agentSpeed = 4f;

        private bool m_state = false;
        private AudioClip m_eatingClip;

        public bool IsAbleToGoNextState { get; private set; }
        public bool IsOnThisState { get; private set; }

        public Eating(Transform itself,ZombieVision vision,AudioClip clip,float speed)
        {
            m_itSelf = itself;
            m_animator = itself.GetComponent<Animator>();
            m_agent = itself.GetComponent<NavMeshAgent>();
            m_audioSource = itself.GetComponent<AudioSource>();
            m_eatingClip = clip;

            m_zombieVision = vision;
            m_agentSpeed = speed;
            IsAbleToGoNextState = false;
            IsOnThisState = false;
        }
        public void OnEnter()
        {
            IsAbleToGoNextState = false;
            IsOnThisState = true;
            m_agent.speed = m_agentSpeed;
            m_state = false;

            m_audioSource.Stop();
            m_audioSource.clip = m_eatingClip;
            m_audioSource.loop = true;
            m_audioSource.Play();


        }

        public void OnExit()
        {
            IsAbleToGoNextState = false;
            IsOnThisState = false;
            m_state = false;
        }
        IEnumerator WaitTime(float time)
        {
            yield return new WaitForSeconds(time);

            m_animator.SetBool("Eat", false);

            yield return new WaitForSeconds(m_animator.
                GetCurrentAnimatorStateInfo(0).length);

            IsAbleToGoNextState = true;
        }
        public void Tick()
        {
            if ((m_zombieVision.DeathTransform.position - m_itSelf.position).magnitude >
                2f)
            {
                m_animator.SetBool("Walk", true);

                m_agent.speed = m_agentSpeed;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(m_zombieVision.DeathTransform.position,
                    out hit, 2f, 1))
                {
                    m_agent.SetDestination(hit.position);
                }
            }
            else
            {
                m_animator.SetBool("Walk", false);

                if (!m_state && m_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    m_agent.speed = 0;
                    m_agent.velocity = Vector3.zero;


                    m_animator.SetBool("Eat", true);

                    m_zombieVision.StartCoroutine(WaitTime(5f));
                    m_state = true;

                }
            }

        }
    }
}
