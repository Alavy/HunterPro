using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Algine
{
    public class Fpsbody : MonoBehaviour
    {
        [SerializeField] private float default_Y;
        [SerializeField] private float crouch_Y;

        private FPSController controller;
        private Animator animator;

        [SerializeField] private Transform leftFootPivot;
        [SerializeField] private Transform rightFootPivot;

        private float leftFootWeight;
        private float rightFootWeight;
        private Transform leftFoot;
        private Transform rightFoot;

        private LayerMask ignoreLayer;

        [SerializeField] private float offsetY;

        private void Start()
        {
            animator = GetComponent<Animator>();
            controller = transform.GetComponentInParent<FPSController>();

            leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
            ignoreLayer = ~1 << LayerMask.NameToLayer("Player");
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            //float speed = InputManager.joystickInputVector.y;
           // float direction = InputManager.joystickInputVector.x;

            float speed =0;
            float direction=0;

            if (controller.IsRunning)
                    speed = 2;

            animator.SetFloat("Speed", speed);
            animator.SetFloat("Direction", direction);

            animator.SetBool("IsCrouching",controller.IsCrouching);
            
            float fixedVerticalPosition;
            if (controller.IsCrouching)
                fixedVerticalPosition = Mathf.MoveTowards(transform.localPosition.y, crouch_Y, 7 * Time.deltaTime);
            else
                fixedVerticalPosition = Mathf.MoveTowards(transform.localPosition.y, default_Y, 7 * Time.deltaTime);

            transform.localPosition = new Vector3(transform.localPosition.x, 
                fixedVerticalPosition,transform.localPosition.z);
                
            
        }

        /// <summary>
        /// Callback for setting up animation IK (inverse kinematics).
        /// </summary>
        /// <param name="layerIndex">Index of the layer on which the IK solver is called.</param>
        void OnAnimatorIK(int layerIndex)
        {
            leftFootWeight = animator.GetFloat("IK Left Foot");
            rightFootWeight = animator.GetFloat("IK Right Foot");

            //Activate IK, set the rotation directly to the goal.
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootWeight);

            RaycastHit leftHit;
            if (Physics.Raycast(leftFootPivot.position, -Vector3.up, out leftHit, ignoreLayer))
            {
                Quaternion ikRotation = Quaternion.FromToRotation(leftFoot.up, leftHit.normal) * leftFoot.rotation;
                ikRotation = new Quaternion(ikRotation.x, leftFoot.rotation.y, ikRotation.z, ikRotation.w);
                Vector3 ikPosition = new Vector3(leftFoot.position.x, leftHit.point.y, leftFoot.position.z);
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, ikPosition + (Vector3.up * offsetY));
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, ikRotation);
            }

            RaycastHit rightHit;
            if (Physics.Raycast(rightFootPivot.position, -Vector3.up, out rightHit, ignoreLayer))
            {
                Quaternion ikRotation = Quaternion.FromToRotation(rightFoot.up, rightHit.normal) * rightFoot.rotation;
                ikRotation = new Quaternion(ikRotation.x, rightFoot.rotation.y, ikRotation.z, ikRotation.w);
                Vector3 ikPosition = new Vector3(rightFoot.position.x, rightHit.point.y, rightFoot.position.z);
                animator.SetIKPosition(AvatarIKGoal.RightFoot, ikPosition + (Vector3.up * offsetY));
                animator.SetIKRotation(AvatarIKGoal.RightFoot, ikRotation);
            }
        }
    }
}

