using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Algine
{
    public class WeaponPickup : MonoBehaviour
    {
        private WeaponManager weaponManager;
        
        public int AmmoCount;
        public int WeaponID;
        public string WeaponName;

        private void Start()
        {
            weaponManager = GameObject.FindGameObjectWithTag("weapon_manager")
                .GetComponent<WeaponManager>();
        }

        public void Pickup()
        {
            ///> for solving dynamic reticle 
            weaponManager.Reticle.gameObject.SetActive(true);
            weaponManager.EquipWeapon(WeaponID, gameObject);
            
        }
    }
}
