using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Algine
{
    public class PickObjects : MonoBehaviour
    {
        [Tooltip("The distance within which you can pick up item")]
        public float distance = 1.5f;

        [SerializeField]
        private GameObject useCursor;

        private Button useButton;
        private TextMeshProUGUI useText;

        private Inventory m_inventory;
        

        private void Start()
        {
            if(useCursor == null)
            {
                Debug.LogError("use missing");
            }
            useText = useCursor.GetComponentInChildren<TextMeshProUGUI>();
            useButton = useCursor.GetComponentInChildren<Button>();

            m_inventory = GetComponentInChildren<Inventory>();
            useCursor.SetActive(false);
        }

        void Update()
        {
            Pickup();
        }

        public void Pickup()
        {
            RaycastHit hit;
            GameObject use;
            //Hit an object within pickup distance
            if (Physics.Raycast(transform.position, transform.forward, out hit, distance))
            {
                if (hit.collider.CompareTag("Item"))
                {
                    use = hit.collider.gameObject;
                    useCursor.SetActive(true);

                    if (use.GetComponent<Item>())
                    {
                        useText.text = use.GetComponent<Item>().Title;
                        
                         var item = use.GetComponent<Item>();
                            useButton.onClick.RemoveAllListeners();
                            useButton.onClick.AddListener(() => {
                                m_inventory.StoreItem(item);
                            });
                            use = null;
                            return;
                            
                    }
                    //useText.text = use.weaponNameToAddAmmo + " Ammo x " + use.ammoQuantity;
                    else if (use.GetComponent<WeaponPickup>()) {

                        useText.text = use.GetComponent<WeaponPickup>().WeaponName;

                      
                     var item = use.GetComponent<WeaponPickup>();
                            useButton.onClick.RemoveAllListeners();
                            useButton.onClick.AddListener(() => { 
                                item.Pickup(); 
                            });
                            use = null;
                            return;
                    }
                }
                else
                {
                    //Clear use object if there is no an object with "Item" tag
                    use = null;
                    useCursor.SetActive(false);
                    useText.text = "";
                }
            }
            else
            {
                useCursor.SetActive(false);
                useText.text = "";
            }
        }
    }
}
