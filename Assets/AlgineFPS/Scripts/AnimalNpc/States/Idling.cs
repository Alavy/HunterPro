using Algine.AI.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Algine.Animal.Npc
{
    public class Idling : IState
    {
        private Animator m_animator;
        private NavMeshAgent m_agent;
        private MonoBehaviour m_animalNPC;

        public bool IsAbleToGoNextState{ get; private set; }
        private float WaitTime = 0;

        public Idling(Transform itself,float waitTime)
        {
            m_animator = itself.GetComponent<Animator>();
            m_agent = itself.GetComponent<NavMeshAgent>();
            m_animalNPC = itself.GetComponent<MonoBehaviour>();
            WaitTime = waitTime;

        }
        public void OnEnter()
        {
            IsAbleToGoNextState = false;

            m_agent.speed = 0f;
            m_agent.velocity = Vector3.zero;

            m_animator.SetBool("Walk", false);
            m_animator.SetBool("Run", false);

            m_animalNPC.StartCoroutine(ChangeState(WaitTime));
        }

        IEnumerator ChangeState(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            IsAbleToGoNextState = true;
        }
        public void OnExit()
        {
            IsAbleToGoNextState = false;

            m_animator.SetBool("Walk", false);
            m_animator.SetBool("Run", false);
        }

        public void Tick()
        {

        }
    }
}
