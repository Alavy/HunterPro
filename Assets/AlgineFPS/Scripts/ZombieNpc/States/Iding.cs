using Algine.AI.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Algine.Zombie.Npc
{
    public class Iding : IState
    {
        private Animator m_animator;
        private AudioSource m_audioSource;
        private MonoBehaviour m_zombie;
        private NavMeshAgent m_agent;

        public bool IsAbleToGoNextState { get; private set; }

        private float m_waitTimeMax = 8f;
        private float m_waitTimeMin = 3f;
        private AudioClip m_breathClip;



        public Iding(Transform itself,AudioClip clip,float maxIdleTime)
        {
            m_animator = itself.GetComponent<Animator>();
            m_zombie = itself.GetComponent<MonoBehaviour>();
            m_agent = itself.GetComponent<NavMeshAgent>();
            m_audioSource = itself.GetComponent<AudioSource>();

            m_breathClip = clip;

            IsAbleToGoNextState = false;
            m_waitTimeMax = maxIdleTime;
        }
        public void OnEnter()
        {
            m_audioSource.Stop();
            m_audioSource.clip = m_breathClip;
            m_audioSource.loop = true;
            m_audioSource.Play();

            m_animator.SetBool("Walk", false);
            m_animator.SetBool("Run", false);

            m_agent.speed = 0;
            m_agent.velocity = Vector3.zero;
            IsAbleToGoNextState = false;
            m_zombie.StartCoroutine(ChangeState(Random.Range(m_waitTimeMin, m_waitTimeMax)));
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
