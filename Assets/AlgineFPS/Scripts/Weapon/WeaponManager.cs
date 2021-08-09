using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using Algine.FPS.MobileInput;

namespace Algine
{
    public class WeaponManager : MonoBehaviour
    {
        ///> A public list which get all aviliable weapons on Start() and operate with them
        [Tooltip("Crosshair")]
        public RectTransform Reticle;
        public Camera MainCamera;
        public Lean LeanController;
        public FPSController PlayerController;
        [Space]
        public GameObject StartWeapon1;
        public Item StartAmmo1;
        public int StartWeaponID1;
        [HideInInspector]
        public Slot ActiveSlot { get; private set; }
        [HideInInspector]
        public Slot PrimarySlot { get; private set; }
        [HideInInspector]
        public Slot SecondarySlot { get; private set; }
        [HideInInspector]
        public Inventory Inventory { get; private set; }
        [HideInInspector]
        public Recoil RecoilComponent { get; private set; }
        [HideInInspector]
        public HitFXManager HitFXComponent { get; private set; }

        private AudioSource m_audioSource;

        [SerializeField]
        private int m_slotsSize = 2;

        private List<Slot> m_Slots = new List<Slot>();
        private List<Weapon> m_Weapons = new List<Weapon>();

        private Animator m_holderAnimator;


        private bool m_isFire;

        private void Start()
        {
            //------>handling dependencies man

            if(Reticle == null || MainCamera==null ||
                LeanController==null || PlayerController == null)
            {
                Debug.LogError("Weapon manager Component missing");
            }
            m_holderAnimator = GetComponentInParent<Animator>();
            Inventory = GetComponent<Inventory>();
            RecoilComponent = MainCamera.GetComponent<Recoil>();
            HitFXComponent = GetComponent<HitFXManager>();
            m_audioSource = GetComponent<AudioSource>();

            //<-----end

            for (int i = 0; i < m_slotsSize; i++)
            {
                Slot slot_temp = new Slot();
                slot_temp.SlotIndex = i;
                m_Slots.Add(slot_temp);
            }
            PrimarySlot = m_Slots[0];
            SecondarySlot = m_Slots[1];

            Reticle.gameObject.SetActive(true);
            m_Weapons.Clear();
            foreach (Weapon weapon in GetComponentsInChildren<Weapon>(true))
            {
                m_Weapons.Add(weapon);
                weapon.gameObject.SetActive(false);

            }
            equipFirstWeapon();
            InputEvents.Current.OnAim += OnAim;
            InputEvents.Current.OnFire += OnFire;
            InputEvents.Current.OnReload += OnReload;
            InputEvents.Current.OnPrimarySlotPressed += OnPrimaySlotPressed;
            InputEvents.Current.OnSecondarySlotPressed += OnSecondarySlotPressed;

            GameEvents.Current.OnPlayerDeath += onPlayerDeath;
            GameEvents.Current.OnPlayerResPawn += onPlayerRespawn;
        }
        private void OnDestroy()
        {
            InputEvents.Current.OnAim -= OnAim;
            InputEvents.Current.OnFire -= OnFire;
            InputEvents.Current.OnReload -= OnReload;
            InputEvents.Current.OnPrimarySlotPressed -= OnPrimaySlotPressed;
            InputEvents.Current.OnSecondarySlotPressed -= OnSecondarySlotPressed;

            GameEvents.Current.OnPlayerDeath -= onPlayerDeath;
            GameEvents.Current.OnPlayerResPawn -= onPlayerRespawn;
        }

        private void OnPrimaySlotPressed(bool state)
        {
            if (state)
            {
                if (ActiveSlot.StoredWeapon.IsAim ||
                     ActiveSlot.StoredWeapon.IsReloading || ActiveSlot.SlotIndex 
                     == PrimarySlot.SlotIndex)
                {
                    return;
                }
                Reticle.gameObject.SetActive(true);
                if (PrimarySlot.StoredWeapon != null)
                {
                    ActiveSlot?.StoredWeapon?.HideWeapon();
                    SlotChange(PrimarySlot.SlotIndex);
                    //updating weapon slot ui
                    InputEvents.Current.WeaponSlotUpdateTrigger();
                }
                
            }
        }
        private void OnSecondarySlotPressed(bool state)
        {
            if (state)
            {
                if (ActiveSlot.StoredWeapon.IsAim ||
                        ActiveSlot.StoredWeapon.IsReloading || ActiveSlot.SlotIndex 
                        == SecondarySlot.SlotIndex)
                {
                    return;
                }
                Reticle.gameObject.SetActive(true);
                if (SecondarySlot.StoredWeapon != null)
                {
                    ActiveSlot?.StoredWeapon?.HideWeapon();
                    SlotChange(SecondarySlot.SlotIndex);
                    //updating weapon slot ui
                    InputEvents.Current.WeaponSlotUpdateTrigger();
                }
                
            }
        }

        private void OnReload(bool state)
        {
            if (state)
            {
               ActiveSlot?.StoredWeapon?.ReloadWeapon();
            }
        }
        private void OnFire(bool state)
        {
            m_isFire = state;
            InputEvents.Current.FireTrigger(state);

        }
        private void OnAim(bool state)
        {
            
            if (state && !ActiveSlot.StoredWeapon.IsReloading)
            {
                ActiveSlot?.StoredWeapon?.AimWeapon();
                if (!ActiveSlot.StoredWeapon.IsAim)
                {
                    LeanController.LeanToDefaultState();
                }
               
            }
        }
        private void onPlayerDeath()
        {
            m_audioSource.Stop();
            m_audioSource.enabled = false;
            DropAllWeapons();
        }
        private void onPlayerRespawn()
        {
            m_audioSource.enabled = true;
            EquipWeapon(StartWeaponID1, StartWeapon1);
        }
        private void Update()
        {
            if (m_isFire)
            {
                ActiveSlot?.StoredWeapon?.FireWeapon();
                InputEvents.Current.FireTrigger(true);
            }

        }

        public void equipFirstWeapon()
        {
            EquipWeapon(StartWeaponID1, StartWeapon1);
            Inventory.StoreItem(StartAmmo1);
        }
        public void EquipWeapon(int weaponID, GameObject dropObject)
        {
            if (!IsWeaponAlreadyPicked(weaponID))
            {
                if (FindFreeSlot() != null)
                {

                    foreach (var item in m_Slots)
                    {
                        item?.StoredWeapon?.gameObject.SetActive(false);
                    }

                    ActiveSlot = FindFreeSlot();

                    foreach (Weapon weapon in m_Weapons)
                    {
                        if (weapon.AmmoItemID == weaponID)
                        {
                            ActiveSlot.StoredWeapon = weapon;

                            ActiveSlot.StoredWeapon.CurrentAmmo =
                                dropObject.GetComponent<WeaponPickup>().AmmoCount;
                            ActiveSlot.StoredDropObject = dropObject;

                            ActiveSlot.StoredDropObject.SetActive(false);

                            ActiveSlot.StoredWeapon.ReadyWeapon();

                            dropObject = null;
                            break;
                        }

                    }

                }
            }
            //updating weapon slot ui
            InputEvents.Current.WeaponSlotUpdateTrigger();
        }

        private Slot FindFreeSlot()
        {
            foreach (Slot slot in m_Slots)
            {
                if (slot.IsFree())
                    return slot;
            }

            return null;
        }
        private void SlotChange(int switchSlotIndex)
        {
            if (m_Slots.Count > switchSlotIndex)
            {
                if (m_Slots[switchSlotIndex].StoredWeapon != null)
                {
                    ActiveSlot?.StoredWeapon?.HideWeapon();

                    ActiveSlot = null;
                    ActiveSlot = m_Slots[switchSlotIndex];
                    ActiveSlot?.StoredWeapon?.ReadyWeapon();
                }
            }
        }
        private bool IsWeaponAlreadyPicked(int weaponID)
        {
            foreach (Slot slot in m_Slots)
            {
                if (slot.StoredWeapon != null)
                {
                    if (slot.StoredWeapon.AmmoItemID == weaponID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private Slot FindEquipedSlot()
        {
            foreach (Slot slot in m_Slots)
            {
                if (!slot.IsFree())
                    return slot;
            }

            return null;
        }


        public void DropAllWeapons()
        {
            m_holderAnimator.SetLayerWeight(0, 0);

            foreach (Slot slot in m_Slots)
            {
                if (!slot.IsFree())
                {
                    if (slot.StoredWeapon.WeaponVariant != WeaponType.Melee)
                    {
                        if (slot.SlotIndex == ActiveSlot.SlotIndex)
                        {
                            DropWeapon(false);
                        }
                        else
                        {
                            if (slot.StoredDropObject)
                            {
                                slot.StoredDropObject.SetActive(true);
                                slot.StoredDropObject.transform.position =
                                    transform.transform.position
                                    + transform.forward * 1.5f;
                                slot.StoredDropObject = null;
                                slot.StoredWeapon = null;
                            }
                        }
                    }
                }
            }
            //updating weapon slot ui
            InputEvents.Current.WeaponSlotUpdateTrigger();
        }
        public void DropWeaponFromSlot(int slot)
        {
            if (ActiveSlot.StoredWeapon.IsAim)
            {
                return;
            }

            if (ActiveSlot.StoredWeapon == m_Slots[slot].StoredWeapon)
            {
                DropWeapon(true);
            }
            else
            {
                m_Slots[slot].StoredDropObject.GetComponent<WeaponPickup>()
                    .AmmoCount = m_Slots[slot].StoredWeapon.CurrentAmmo;
                m_Slots[slot].StoredDropObject.transform.position =
                    transform.position + transform.forward * 1.5f;
                m_Slots[slot].StoredDropObject.SetActive(true);
                m_Slots[slot].StoredDropObject = null;
                m_Slots[slot].StoredWeapon = null;
            }
            //updating weapon slot ui
            InputEvents.Current.WeaponSlotUpdateTrigger();
        }
        private void DropWeapon(bool show)
        {
            if (ActiveSlot != null && ActiveSlot.StoredWeapon !=null)
            {
                ActiveSlot.StoredDropObject.GetComponent<WeaponPickup>().
                         AmmoCount = ActiveSlot.StoredWeapon.CurrentAmmo;
                //droping 
                ActiveSlot.StoredWeapon.gameObject.SetActive(false);
                ActiveSlot.StoredDropObject.transform.position = transform.position
                    + transform.forward * 1.5f;
                ActiveSlot.StoredDropObject.SetActive(true);
                ActiveSlot.StoredDropObject = null;
                ActiveSlot.StoredWeapon = null;
                if (show)
                {
                    ActiveSlot = FindEquipedSlot();
                    ActiveSlot?.StoredWeapon?.ReadyWeapon();
                }
                
            }
        }
    }
}
