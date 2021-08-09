using UnityEngine;

namespace Algine
{
    class AnimatorHash
    {
        //for weapon
        public static readonly int hash_Shot = Animator.StringToHash("Shot");
        public static readonly int hash_Idle_empty = Animator.StringToHash("Idle_empty");
        public static readonly int hash_Reload = Animator.StringToHash("Reload");
        public static readonly int hash_Ready = Animator.StringToHash("Ready");
        public static readonly int hash_UnhideWeapon = Animator.StringToHash("Unhide");
        public static readonly int hash_HideWeapon = Animator.StringToHash("Hide");


        public static readonly int hash_Attack = Animator.StringToHash("Attack");
        public static readonly int hash_Empty = Animator.StringToHash("Empty");
        public static readonly int hash_Fire_State = Animator.StringToHash("Fire");


        // for fps Controler
        public static readonly int hash_Walk = Animator.StringToHash("Walk");
        public static readonly int hash_Run = Animator.StringToHash("Run");
        public static readonly int hash_Crouch = Animator.StringToHash("Crouch"); 
        public static readonly int hash_Landing = Animator.StringToHash("Landing");

        //for NPC
        
        public static readonly int hash_Forward = Animator.StringToHash("Forward");
        public static readonly int hash_LookAtTarget = Animator.StringToHash("LookAtTarget");
        public static readonly int hash_Turn = Animator.StringToHash("Turn");
        public static readonly int hash_Horizontal = Animator.StringToHash("Horizontal");
        public static readonly int hash_OnGround = Animator.StringToHash("OnGround");
        public static readonly int hash_Jump = Animator.StringToHash("Jump");
        public static readonly int hash_JumpLeg = Animator.StringToHash("JumpLeg");

    }
}
