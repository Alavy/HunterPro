using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Algine.AI;
using Algine.AI.State;

namespace Algine.Zombie.Npc
{

    public class NormalZombieNpc : MonoBehaviour
    {
        [SerializeField]
        private float RunSpeed = 4f;
        [SerializeField]
        private float WalkSpeed = 1f;
        [SerializeField]
        private float IdleWaitTime = 5f;
        [SerializeField]
        [Range(1, 6)]
        private float AttackDistance = 3f;
        [SerializeField]
        private float m_Health = 100;
        [SerializeField]
        private Transform SpawnPos;
        [SerializeField]
        private float RespawnTime = 8f;

        [SerializeField]
        private LayerMask PlayerMask;

        [SerializeField]
        private AudioClip Growl;
        [SerializeField]
        private AudioClip Breath;
        [SerializeField]
        private AudioClip Rage;


        private NavMeshAgent m_agent;
        private Animator m_animator;
        private AudioSource m_audioSource;

        private StateMachine m_stateMachine;
        private ZombieVision m_zombieVision;

        private bool m_isHit = false;
        private bool isNpcDead = false;

        private Iding idling;

        private void Awake()
        {
            m_agent = GetComponent<NavMeshAgent>();
            m_animator = GetComponent<Animator>();
            m_audioSource = GetComponent<AudioSource>();

            m_zombieVision = GetComponentInChildren<ZombieVision>();
            m_stateMachine = new StateMachine();

            m_agent.updatePosition = true;
            m_agent.updateRotation = true;

            idling = new Iding(transform,Breath,IdleWaitTime);
            var wandering = new Wandering(transform,Growl,WalkSpeed);
            var attacking = new Attacking(transform, m_zombieVision,Rage,
                RunSpeed,AttackDistance,PlayerMask);

            m_stateMachine.AddTransition(idling, wandering,
                () => idling.IsAbleToGoNextState);

            m_stateMachine.AddTransition(wandering, idling,
                () => wandering.IsAbleToGoNextState);

            m_stateMachine.AddAnyTransition(attacking, 
                () => m_zombieVision.IsPlayerVisible || m_isHit);

            m_stateMachine.SetState(idling);

        }

        private void Start()
        {
            foreach (var item in GetComponentsInChildren<Rigidbody>())
            {
                item.isKinematic = true;
            }
        }

        private void Update()
        {
            m_stateMachine.Tick();
        }
        
        public void Damage(int damage)
        {
            m_Health = m_Health - damage;
            m_isHit = true;

            if (m_Health < 0 && !isNpcDead)
            {
                death();
                isNpcDead = true;
            }
        }
        private void death()
        {
            m_audioSource.Stop();
            m_audioSource.enabled = false;
            m_animator.enabled = false;
            m_agent.enabled = false;

            foreach (var item in GetComponentsInChildren<Rigidbody>())
            {
                item.isKinematic = false;
            }
            enabled = false;
            Invoke("respawn", RespawnTime);
        }
        private void respawn()
        {
            enabled = true;
            isNpcDead = false;
            m_isHit = false;

            m_audioSource.enabled = true;
            m_animator.enabled = true;
            m_agent.enabled = true;

            foreach (var item in GetComponentsInChildren<Rigidbody>())
            {
                item.isKinematic = true;
            }
            m_stateMachine.SetState(idling);
            transform.position = SpawnPos.position;
            transform.rotation = Quaternion.identity;
        }
    }

}