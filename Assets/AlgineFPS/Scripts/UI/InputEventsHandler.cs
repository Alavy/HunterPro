using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Algine.FPS.MobileInput
{
    public class InputEventsHandler : MonoBehaviour
    {

        [SerializeField]
        private GameObject CrouchUI;
        [SerializeField]
        private GameObject AimUI;
        [SerializeField]
        private GameObject FireUI1;
        [SerializeField]
        private GameObject FireUI2;
        [SerializeField]
        private GameObject JumpUI;

        [SerializeField]
        private GameObject ReloadUI;

        [SerializeField]
        private GameObject LeanLeftUI;
        [SerializeField]
        private GameObject LeanRightUI;
        [SerializeField]
        private GameObject PrimarySlotUI;
        [SerializeField]
        private Image PrimaryWeponImage;
        [SerializeField]
        private TextMeshProUGUI PrimaryWeaponName;
        [SerializeField]
        private TextMeshProUGUI PrimaryWeponAmmo;
        [SerializeField]
        private GameObject PrimaryDrop;
        [SerializeField]
        private GameObject SecondarySlotUI;
        [SerializeField]
        private Image SecondaryWeponImage;
        [SerializeField]
        private TextMeshProUGUI SecondaryWeaponName;
        [SerializeField]
        private TextMeshProUGUI SecondaryWeponAmmo;
        [SerializeField]
        private GameObject SecondaryDrop;
        [SerializeField]
        private GameObject InventoryUI;
        [SerializeField]
        private WeaponManager Manager;

        private bool isReadyToTakeInput = true;

        public static InputEventsHandler Current
        {
            private set;
            get;
        }

        private void Awake()
        {
            //single instance
            Current = this;

            LeanLeftUI.SetActive(false);
            LeanRightUI.SetActive(false);

            //Refresh UI
            PrimaryWeponImage.sprite = null;
            PrimaryWeponAmmo.text = "";
            PrimaryWeaponName.text = "";
            PrimaryWeponImage.color = Color.clear;
            PrimaryDrop.SetActive(false);

            SecondaryWeponImage.sprite = null;
            SecondaryWeponAmmo.text = "";
            SecondaryWeaponName.text = "";
            SecondaryWeponImage.color = Color.clear;
            SecondaryDrop.SetActive(false);

            InputEvents.Current.OnWeaponSlotUpdateTrigger += OnWeaponSlotUpdateTrigger;
            InputEvents.Current.OnFireTrigger += OnFireTrigger;
            GameEvents.Current.OnPlayerDeath += onPlayerDeath;
            GameEvents.Current.OnPlayerResPawn += onPlayerRespawn;
        }
        private void onPlayerDeath()
        {
            isReadyToTakeInput = false;
        }
        private void onPlayerRespawn()
        {
            isReadyToTakeInput = true;
        }
        private void OnDestroy()
        {
            InputEvents.Current.OnWeaponSlotUpdateTrigger -= OnWeaponSlotUpdateTrigger;
            InputEvents.Current.OnFireTrigger -= OnFireTrigger;

            GameEvents.Current.OnPlayerDeath -= onPlayerDeath;
            GameEvents.Current.OnPlayerResPawn -= onPlayerRespawn;

        }
        private void OnFireTrigger(bool state)
        {
            if (Manager.ActiveSlot.SlotIndex == Manager.PrimarySlot.SlotIndex)
            {
                PrimaryWeponAmmo.text = string.Format("{0} | {1}\n",
                    Manager.PrimarySlot?.StoredWeapon?.CurrentAmmo,
                    Manager.PrimarySlot?.StoredWeapon?.CalculateTotalAmmo());
            }
            else
            {
                SecondaryWeponAmmo.text = string.Format("{0} | {1}\n", 
                    Manager.SecondarySlot?.StoredWeapon?.CurrentAmmo,
                    Manager.SecondarySlot?.StoredWeapon?.CalculateTotalAmmo());
            }
        }
        private void OnWeaponSlotUpdateTrigger()
        {
            if (Manager.PrimarySlot.StoredWeapon != null)
            {
                PrimaryWeponImage.sprite = Manager.PrimarySlot?.StoredWeapon?
                    .WeaponSetting?.WeaponIcon;

                PrimaryWeponAmmo.text = string.Format("{0} | {1}\n", 
                    Manager.PrimarySlot?.StoredWeapon?.CurrentAmmo,
                    Manager.PrimarySlot?.StoredWeapon?.CalculateTotalAmmo());

                PrimaryWeaponName.text = Manager.PrimarySlot?.StoredWeapon?.WeaponName;
                PrimaryWeponImage.color = Color.white;
                PrimaryDrop.SetActive(true);
            }
            else
            {
                PrimaryWeponImage.sprite = null;
                PrimaryWeponAmmo.text = "";
                PrimaryWeaponName.text = "";
                PrimaryWeponImage.color = Color.clear;
                PrimaryDrop.SetActive(false);

            }

            if (Manager.SecondarySlot.StoredWeapon != null)
            {
                SecondaryWeponImage.sprite = Manager.SecondarySlot?.StoredWeapon?
                    .WeaponSetting?.WeaponIcon;

                SecondaryWeponAmmo.text = string.Format("{0} | {1}\n",
                    Manager.SecondarySlot?.StoredWeapon?.CurrentAmmo,
                    Manager.SecondarySlot?.StoredWeapon?.CalculateTotalAmmo());
                SecondaryWeaponName.text = Manager.SecondarySlot?.StoredWeapon?.WeaponName;

                SecondaryWeponImage.color = Color.white;
                SecondaryDrop.SetActive(true);
            }
            else
            {
                SecondaryWeponImage.sprite = null;
                SecondaryWeponAmmo.text = "";
                SecondaryWeaponName.text = "";
                SecondaryWeponImage.color = Color.clear;
                SecondaryDrop.SetActive(false);

            }
        }


        public void Inventory(bool state)
        {
            if (!isReadyToTakeInput)
            {
                return;
            }
            InputEvents.Current.InventoryPressed(state);

            if (state)
            {
                LeanTween.scale(InventoryUI, new Vector3(1.3f, 1.3f, 1), 0.1f);
            }
            else
            {
                LeanTween.scale(InventoryUI, new Vector3(1, 1, 1), 0.08f);
            }
        }
        public void Reload(bool state)
        {
            if (!isReadyToTakeInput)
            {
                return;
            }
            InputEvents.Current.ReloadPressed(state);

            if (state)
            {
                LeanTween.scale(ReloadUI, new Vector3(1.3f, 1.3f, 1), 0.1f);
            }
            else
            {
                LeanTween.scale(ReloadUI, new Vector3(1, 1, 1), 0.08f);
            }
        }
        public void PrimarySlotPressed(bool state)
        {
            if (!isReadyToTakeInput)
            {
                return;
            }
            InputEvents.Current.PrimarySlotPressed(state);

            if (state)
            {
                LeanTween.scale(PrimarySlotUI, new Vector3(1.1f, 1.1f, 1), 0.1f);
            }
            else
            {
                LeanTween.scale(PrimarySlotUI, new Vector3(1, 1, 1), 0.08f);
            }
        }
        public void SecondarySlotPressed(bool state)
        {
            if (!isReadyToTakeInput)
            {
                return;
            }
            InputEvents.Current.SecondarySlotPressed(state);

            if (state)
            {
                LeanTween.scale(SecondarySlotUI, new Vector3(1.1f, 1.1f, 1), 0.1f);
            }
            else
            {
                LeanTween.scale(SecondarySlotUI, new Vector3(1, 1, 1), 0.08f);
            }
        }
        public void LeanLeft(bool state)
        {
            if (!isReadyToTakeInput)
            {
                return;
            }
            InputEvents.Current.LeanLeft(state);

            if (state)
            {
                LeanTween.scale(LeanLeftUI, new Vector3(1.3f, 1.3f, 1), 0.1f);
            }
            else
            {
                LeanTween.scale(LeanLeftUI, new Vector3(1, 1, 1), 0.08f);
            }
        }
        public void LeanRight(bool state)
        {
            if (!isReadyToTakeInput)
            {
                return;
            }
            InputEvents.Current.LeanRight(state);

            if (state)
            {
                LeanTween.scale(LeanRightUI, new Vector3(1.3f, 1.3f, 1), 0.1f);
            }
            else
            {
                LeanTween.scale(LeanRightUI, new Vector3(1, 1, 1), 0.08f);
            }
        }
        public void Jump(bool state)
        {
            if (!isReadyToTakeInput)
            {
                return;
            }
            InputEvents.Current.JumpPressed(state);

            if (state)
            {
                LeanTween.scale(JumpUI, new Vector3(1.3f, 1.3f, 1), 0.1f);
            }
            else
            {
                LeanTween.scale(JumpUI, new Vector3(1, 1, 1), 0.08f);
            }
            
        }
        public void Aim(bool state)
        {
            if (!isReadyToTakeInput)
            {
                return;
            }
            InputEvents.Current.AimPressed(state);

            
            if (state)
            {
                LeanTween.scale(AimUI, new Vector3(1.3f, 1.3f, 1), 0.1f);
            }
            else
            {
                LeanTween.scale(AimUI, new Vector3(1, 1, 1), 0.08f);
            }

            if (Manager.ActiveSlot.StoredWeapon.IsAim)
            {
                LeanLeftUI.SetActive(true);
                LeanRightUI.SetActive(true);
            }
            else
            {
                LeanLeftUI.SetActive(false);
                LeanRightUI.SetActive(false);
            }
        }
        public void Fire1(bool state)
        {
            if (!isReadyToTakeInput)
            {
                return;
            }
            InputEvents.Current.FirePressed(state);

            if (state)
            {
                LeanTween.scale(FireUI1, new Vector3(1.3f, 1.3f, 1), 0.1f);
            }
            else
            {
                LeanTween.scale(FireUI1, new Vector3(1, 1, 1), 0.08f);
            }

        }
        public void Fire2(bool state)
        {
            if (!isReadyToTakeInput)
            {
                return;
            }
            InputEvents.Current.FirePressed(state);

            if (state)
            {
                LeanTween.scale(FireUI2, new Vector3(1.3f, 1.3f, 1), 0.1f);
            }
            else
            {
                LeanTween.scale(FireUI2, new Vector3(1, 1, 1), 0.08f);
            }

        }
        public void Crouch(bool state)
        {
            if (!isReadyToTakeInput)
            {
                return;
            }

            InputEvents.Current.CrouchPressed(state);

            if (state)
            {
                LeanTween.scale(CrouchUI, new Vector3(1.3f, 1.3f, 1), 0.1f);
            }
            else
            {
                LeanTween.scale(CrouchUI, new Vector3(1, 1, 1), 0.08f);
            }
            
        }
        public void PrimaryDropPressed(bool state)
        {
            if (!isReadyToTakeInput)
            {
                return;
            }
            Manager.DropWeaponFromSlot(Manager.PrimarySlot.SlotIndex);
            if (state)
            {
                LeanTween.scale(PrimaryDrop, new Vector3(1.1f, 1.1f, 1), 0.1f);
            }
            else
            {
                LeanTween.scale(PrimaryDrop, new Vector3(1, 1, 1), 0.08f);
            }
        }
        public void SecondaryDropPressed(bool state)
        {
            if (!isReadyToTakeInput)
            {
                return;
            }
            Manager.DropWeaponFromSlot(Manager.SecondarySlot.SlotIndex);
            if (state)
            {
                LeanTween.scale(SecondaryDrop, new Vector3(1.1f, 1.1f, 1), 0.1f);
            }
            else
            {
                LeanTween.scale(SecondaryDrop, new Vector3(1, 1, 1), 0.08f);
            }
        }
        public void Touch(Vector2 dir)
        {
            if (!isReadyToTakeInput)
            {
                return;
            }

            InputEvents.Current.TouchLook(dir);
        }
        public void JoyStick(Vector2 dir)
        {
            if (!isReadyToTakeInput)
            {
                return;
            }

            InputEvents.Current.JoyStickDrag(dir);
        }

    }
}