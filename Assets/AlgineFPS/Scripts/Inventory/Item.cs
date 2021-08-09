using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Algine {

    public enum ItemType { none, ammo, consumable }

    public class Item : MonoBehaviour
    {
        public int Id;
        public string Title;
        public ItemType Type;
        public int Ammo;
    }
}


