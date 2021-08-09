using Algine.AI.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Algine.Zombie.Npc
{
    public class Wandering : IState
    {
        private Animator m_animator;
        private AudioSource m_audioSource;

        private NavMeshAgent m_agent;
        private Transform m_itSelf;
        private AudioClip m_growlClip;

        private float agentSpeed = 1.5f;
        public bool IsAbleToGoNextState { get; private set; }

        public Wandering(Transform itself,AudioClip clip,float walkSpeed)
        {
            m_itSelf = itself;
            m_animator = itself.GetComponent<Animator>();
            m_agent = itself.GetComponent<NavMeshAgent>();
            m_audioSource = itself.GetComponent<AudioSource>();
            m_growlClip = clip;

            IsAbleToGoNextState = false;
            agentSpeed = walkSpeed;
        }
        public void OnEnter()
        {

            m_audioSource.Stop();
            m_audioSource.clip = m_growlClip;
            m_audioSource.loop = true;
            m_audioSource.Play();

            m_agent.speed = agentSpeed;

            m_animator.SetBool("Walk", true);

            IsAbleToGoNextState = false;

            Vector3 randomDirection = Random.insideUnitSphere * 10;
            randomDirection += m_itSelf.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, 10, 1))
            {
                m_agent.SetDestination(hit.position);
            }
        }
        public void OnExit()
        {
            IsAbleToGoNextState = false;
            m_animator.SetBool("Walk", false);
        }
        public void Tick()
        {
            if (m_agent.remainingDistance < m_agent.stoppingDistance)
            {
                IsAbleToGoNextState = true;
            }
        }
    }
}