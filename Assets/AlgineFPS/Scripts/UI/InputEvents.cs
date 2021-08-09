using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Algine.FPS.MobileInput
{
    public class InputEvents
    {
        public Action<bool> OnJump;
        public Action<bool> OnCrouch;
        public Action<bool> OnAim;
        public Action<bool> OnFire;

        public Action<bool> OnReload;
        public Action<bool> OnInventory;
        public Action<bool> OnLeanLeft;
        public Action<bool> OnLeanRight;

        public Action<bool> OnPrimarySlotPressed;
        public Action<bool> OnSecondarySlotPressed;

        public Action OnWeaponSlotUpdateTrigger;
        public Action<bool> OnFireTrigger;

        public Action<Vector2> OnTouchLook;
        public Action<Vector2> OnJoyStickDrag;

        private static InputEvents m_current;
        public static InputEvents Current
        {
            private set
            {

            }
            get
            {
                if (m_current == null)
                {
                    m_current = new InputEvents();
                    return m_current;
                }
                else
                {
                    return m_current;
                }
            }
        }

        public void TouchLook(Vector2 dir)
        {
            OnTouchLook?.Invoke(dir);
        }
        public void JoyStickDrag(Vector2 dir)
        {
            OnJoyStickDrag?.Invoke(dir);
        }

        public void FireTrigger(bool state)
        {
            OnFireTrigger?.Invoke(state);
        }
        public void WeaponSlotUpdateTrigger()
        {
            OnWeaponSlotUpdateTrigger?.Invoke();
        }
        public void InventoryPressed(bool state)
        {
            OnInventory?.Invoke(state);
        }
        public void LeanLeft(bool state)
        {
            OnLeanLeft?.Invoke(state);
        }
        public void PrimarySlotPressed(bool state)
        {
            OnPrimarySlotPressed?.Invoke(state);
        }
        public void SecondarySlotPressed(bool state)
        {
            OnSecondarySlotPressed?.Invoke(state);
        }
        public void LeanRight(bool state)
        {
            OnLeanRight?.Invoke(state);

        }
        public void ReloadPressed(bool state)
        {
            OnReload?.Invoke(state);
        }
        public void AimPressed(bool state)
        {
            OnAim?.Invoke(state);
        }
        public void FirePressed(bool state)
        {
            OnFire?.Invoke(state);
        }
        public void JumpPressed(bool state)
        {
            OnJump?.Invoke(state);
        }
        public void CrouchPressed(bool state)
        {
            OnCrouch?.Invoke(state);
        }
    }
}