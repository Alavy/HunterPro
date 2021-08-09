using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Algine
{
    public class Fpsbody_rotation : MonoBehaviour
    {
        [SerializeField] private Transform holder;
        void Update()
        {
            transform.localRotation = Quaternion.Euler(0, holder.localEulerAngles.y, 0);
        }
    }
}

