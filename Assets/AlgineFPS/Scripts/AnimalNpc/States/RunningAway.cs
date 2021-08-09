using Algine;
using Algine.FPS;
using Algine.AI.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Algine.Animal.Npc
{
    public class RunningAway : IState
    {
        private Animator m_animator;
        private NavMeshAgent m_agent;
        private Transform m_itSelf;

        public bool IsAbleToGoNextState { get; private set; }
        private float m_agentSpeed = 10f;

        public RunningAway(Transform itself,float speed)
        {
            m_itSelf = itself;
            m_animator = itself.GetComponent<Animator>();
            m_agent = itself.GetComponent<NavMeshAgent>();
            m_agentSpeed = speed;
            IsAbleToGoNextState = false;
        }
        public void OnEnter()
        {
            m_animator.SetBool("Run", true);
            m_animator.SetBool("Attack", false);

            m_agent.speed = m_agentSpeed;

            IsAbleToGoNextState = false;

            Vector3 dir = Random.insideUnitSphere * 20f;
            dir += m_itSelf.position;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(dir, out hit, 20f, 1))
            {
                m_agent.SetDestination(hit.position);
            }

        }

        public void OnExit()
        {
            IsAbleToGoNextState = false;

            m_animator.SetBool("Run", false);
            m_animator.SetBool("Attack", false);
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
