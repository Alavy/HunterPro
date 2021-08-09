using Algine.FPS;
using Algine.AI.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Algine.Animal.Npc
{
    public class Attacking : IState
    {
        private Animator m_animator;
        private NavMeshAgent m_agent;
        private Transform m_target;
        private Transform m_itSelf;
        private float agentSpeed = 4f;

        public Attacking(Transform itself,float speed)
        {
            m_animator = itself.GetComponent<Animator>();
            m_agent = itself.GetComponent<NavMeshAgent>();
            m_target = GameObject.FindGameObjectWithTag("Player").transform;
            m_itSelf = itself;
            agentSpeed = speed;
        }

        public void OnEnter()
        {
            m_agent.speed = agentSpeed;
        }

        public void OnExit()
        {
            m_animator.SetBool("Run", false);
            m_animator.SetBool("Attack", false);
        }

        public void Tick()
        {
            if ((m_target.position - m_itSelf.position).magnitude > 3f)
            {
                m_animator.SetBool("Run", true);
                m_animator.SetBool("Attack", false);

                m_agent.speed = agentSpeed;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(m_target.position, out hit, 3f, 1))
                {
                    m_agent.SetDestination(hit.position);
                }
            }
            else
            {
                m_animator.SetBool("Run", false);

                if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    m_agent.speed = 0;
                    m_agent.velocity = Vector3.zero;
                    m_animator.SetBool("Attack",true);
                }
            }

        }
    }

}