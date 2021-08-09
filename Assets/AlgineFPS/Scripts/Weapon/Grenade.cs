using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Algine {

    public class Grenade : MonoBehaviour
    {
        public float explosionTimer;
        public float explosionForce;
        public float damageRadius;
        public float damage;

        public GameObject explosionEffects;

        Collider[] colliders;
        GameObject effects_temp;

        void OnEnable()
        {
            effects_temp = Instantiate(explosionEffects);
            effects_temp.SetActive(false);

            StartCoroutine(Timer(explosionTimer));
        }
        
        IEnumerator Timer(float explosionTimer)
        {
                yield return new WaitForSeconds(explosionTimer);
                Explosion();
        }

        void Explosion()
        {

            colliders = Physics.OverlapSphere(transform.position, damageRadius);

            foreach (Collider collider in colliders)
            {
                collider.SendMessage("Damage", 
                    SendMessageOptions.DontRequireReceiver);
            }

            effects_temp.transform.position = transform.position;
            effects_temp.transform.rotation = transform.rotation;

            effects_temp.SetActive(true);

            Destroy(gameObject);
        }
    }
}
