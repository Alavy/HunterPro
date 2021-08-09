using Algine.FPS.MobileInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Algine
{
    public class ReticleAnimationHandler : MonoBehaviour
    {
        private Animator animator;
        private void Start()
        {
            InputEvents.Current.OnFireTrigger += OnFireTrigger;
            animator = GetComponent<Animator>();
            
        }

        private void OnFireTrigger(bool state)
        {
            animator.SetBool(AnimatorHash.hash_Fire_State, state);
        }
        private void OnDestroy()
        {
            InputEvents.Current.OnFireTrigger -= OnFireTrigger;
        }
    }
    
}

