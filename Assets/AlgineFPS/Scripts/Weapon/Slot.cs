using UnityEngine;

namespace Algine {

    public class Slot {

        public int SlotIndex = -1;
        public Weapon StoredWeapon;
        public GameObject StoredDropObject;

        public bool IsFree()
        {
            if (StoredWeapon == null)
                return true;
            else
                return false;
        }
    }
}
