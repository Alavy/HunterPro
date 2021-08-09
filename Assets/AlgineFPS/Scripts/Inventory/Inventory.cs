using Algine.FPS.MobileInput;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Algine
{
    public class Inventory : MonoBehaviour
    {
        private readonly List<Item> Items = new List<Item>();

        //Method to add item to inventory
        public void StoreItem(Item item)
        {
            if(item.Type == ItemType.ammo)
            {
                Item ite = Items.Find(x => x.Id == item.Id);
                if ( ite == null)
                {
                    Items.Add(item);
                }
                else
                {
                    ite.Ammo += item.Ammo;
                }
                //updating weapon slot ui
                InputEvents.Current.FireTrigger(false);
            }
            item.gameObject.SetActive(false);
        }
        public Item FindItem(int id)
        {
            var _item = Items.Find(x => id == x.Id);
            return _item;
        }
        public void DestroyItem(int id)
        {
            
            var _item = Items.Find(x => id == x.Id);
            if(_item != null)
            {
                Items.Remove(_item);
                Destroy(_item.gameObject);
            }
        }
        public void ThrowItem(int id)
        {
            var _item = Items.Find(x => id == x.Id);
            if (_item != null)
            {
                if(_item.gameObject != null)
                    {
                    _item.gameObject.transform.position = transform.position + transform.forward * 0.5f;
                    _item.gameObject.SetActive(true);
                }
                Items.Remove(_item);
            }
        }
    }
}
