using Algine.FPS;
using Algine.AI.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Algine.Animal.Npc
{
    public class Wandering : IState
    {
        private Animator m_animator;
        private NavMeshAgent m_agent;
        private Transform m_itSelf;
        public bool IsAbleToGoNextState { get; private set; }
        private float m_agentSpeed = 1.5f;

        public Wandering(Transform itself,float speed)
        {
            m_itSelf = itself;
            m_animator = itself.GetComponent<Animator>();
            m_agent = itself.GetComponent<NavMeshAgent>();
            m_agentSpeed = speed;
            IsAbleToGoNextState = false;
        }

        public void OnEnter()
        {
            m_agent.speed = m_agentSpeed;
            m_animator.SetBool("Walk", true);

            Vector3 randomDirection = Random.insideUnitSphere * 20;
            randomDirection += m_itSelf.position;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, 20, 1))
            {
                m_agent.SetDestination(hit.position);
            }
            IsAbleToGoNextState = false;
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