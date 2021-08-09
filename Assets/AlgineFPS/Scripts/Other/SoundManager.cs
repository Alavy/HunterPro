using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip pickupSound;
    public AudioClip inventoryOpenSound;
    public AudioClip clickSound;

    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void Pickup()
    {
        m_audioSource.PlayOneShot(pickupSound);
    }

    public void InventoryOpen()
    {
        m_audioSource.PlayOneShot(inventoryOpenSound);
    }

    public void Click()
    {
        m_audioSource.PlayOneShot(clickSound);
    }

}
