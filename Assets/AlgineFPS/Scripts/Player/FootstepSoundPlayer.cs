using UnityEngine;
using UnityEngine.AI;

namespace Algine.FPS
{
    public class FootstepSoundPlayer : MonoBehaviour
    {
       

        [SerializeField]
        private AudioClip[] Steps;

        private int arrayLength=0;
        private int index = 0;
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponentInChildren<AudioSource>();
            arrayLength = Steps.Length;
        }

        public void PlayFootstepPlayer()
        {
            audioSource.pitch = Random.Range(0.8f, 1f);
            audioSource.PlayOneShot(Steps[index]);
            index++;
            if (index >= arrayLength)
            {
                index = 0;
            }
        }
    }
}
