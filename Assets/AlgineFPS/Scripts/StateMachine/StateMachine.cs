using Algine.Animal.Npc;
using Algine.AI.State;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Object = System.Object;

namespace Algine.AI
{
    public class StateMachine
    {
        private IState m_currentState;

        private Dictionary<Type, List<Transition>> m_transitions = new Dictionary<Type, 
            List<Transition>>();
        private List<Transition> m_currentTransitions = new List<Transition>();
        private List<Transition> m_anyTransitions = new List<Transition>();
        private static List<Transition> EmptyTransitions = new List<Transition>(0);

        public void Tick()
        {
            var transition = GetTransition();
            if (transition != null)
                SetState(transition.To);

            m_currentState?.Tick();
        }

        public void SetState(IState state)
        {
            if (state == m_currentState)
                return;

            m_currentState?.OnExit();
            m_currentState = state;

            m_transitions.TryGetValue(m_currentState.GetType(), out m_currentTransitions);
            if (m_currentTransitions == null)
                m_currentTransitions = EmptyTransitions;

            m_currentState.OnEnter();
        }

        public void AddTransition(IState from, IState to, Func<bool> predicate)
        {
            if (m_transitions.TryGetValue(from.GetType(), out var transitions) == false)
            {
                transitions = new List<Transition>();
                m_transitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to, predicate));
        }

        public void AddAnyTransition(IState state, Func<bool> predicate)
        {
            m_anyTransitions.Add(new Transition(state, predicate));
        }

        private class Transition
        {
            public Func<bool> Condition { get; }
            public IState To { get; }

            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }
        }

        private Transition GetTransition()
        {
            foreach (var transition in m_anyTransitions)
                if (transition.Condition())
                    return transition;

            foreach (var transition in m_currentTransitions)
                if (transition.Condition())
                    return transition;

            return null;
        }
    }
}