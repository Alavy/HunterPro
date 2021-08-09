using Algine;
using Algine.Animal.Npc;
using Algine.FPS;
using Algine.AI;
using Algine.AI.State;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.AI;

public class AnimalNPC : MonoBehaviour
{
    [SerializeField]
    private float RunSpeed = 4f;
    [SerializeField]
    private float WalkSpeed = 1f;
    [SerializeField]
    private float IdleWaitTime = 5f;

    private NavMeshAgent m_agent;
    private Animator m_animator;
    private StateMachine m_stateMachine;
    private AnimalVision m_animalVision;

    private bool isHit = false;

    [SerializeField]
    private float m_Health = 100;
    [SerializeField]
    private Transform m_player;
    [SerializeField]
    private Transform Head;
    [SerializeField]
    private LayerMask PlayerMask;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_stateMachine = new StateMachine();
        m_agent = GetComponent<NavMeshAgent>();
        m_animalVision = GetComponentInChildren<AnimalVision>();

        m_agent.updatePosition = true;
        m_agent.updateRotation = true;

        var idle = new Idling(transform , IdleWaitTime);

        var wander = new Wandering(transform,WalkSpeed);

        var running = new RunningAway(transform,RunSpeed);

        var attacking = new Attacking(transform, RunSpeed);


        m_stateMachine.AddTransition(idle, wander,
            ()=> idle.IsAbleToGoNextState);

        m_stateMachine.AddTransition(wander, idle,
            ()=> wander.IsAbleToGoNextState );

        m_stateMachine.AddTransition(running, wander, 
            () => running.IsAbleToGoNextState);

        m_stateMachine.AddAnyTransition(running, () => {
            if (isHit)
            {
                isHit = false;
                return true;
            }
            else
            {
                return false;
            }
        });

        m_stateMachine.AddAnyTransition(attacking, 
            () => m_animalVision.IsPlayerVisible );

        m_stateMachine.SetState(idle);
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
        applyAttack();
        m_stateMachine.Tick();
    }
    private void applyAttack()
    {
        RaycastHit hit;
        if(Physics.SphereCast(Head.position,0.5f,-Head.up,out hit, 0.5f, PlayerMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                hit.transform.GetComponent<FPSController>().Damage(1);
            }
        }
    }

    public void Damage(int damage)
    {
        m_Health = m_Health - damage;
        isHit = true;

        if (m_Health < 0)
        {
            death();
        }
    }
    private void death()
    {
        m_animator.enabled = false;
        m_agent.enabled = false;

        foreach (var item in GetComponentsInChildren<Rigidbody>())
        {
            item.isKinematic = false;
        }
        enabled = false;

    }
}
