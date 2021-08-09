using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Algine
{
    public class GrenadeLauncher : MonoBehaviour
    {
        public float explosionForce;
        public float damageRadius;
        public float damage;

        public GameObject explosionEffects;
        private GameObject effects_temp;


        private void Start()
        {
            effects_temp = Instantiate(explosionEffects);
            effects_temp.SetActive(false);
        }
        
        private void OnTriggerEnter()
        {
            StartCoroutine(Explosion(3f));  
        }
        IEnumerator Explosion(float time)
        {
            effects_temp.transform.position = transform.position;
            effects_temp.transform.rotation = transform.rotation;
            effects_temp.SetActive(true);

            Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);

            foreach (Collider collider in colliders)
            {

                collider.SendMessage("Damage",
                    SendMessageOptions.DontRequireReceiver);
            }

            yield return new WaitForSeconds(time);

            effects_temp.SetActive(false);
            gameObject.SetActive(false);

        }
    }

}
