using UnityEngine;
using UnityEngine.AI;

namespace Algine
{
    public class FootstepSound : MonoBehaviour
    { 
        private NavMeshAgent agent;
        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip[] Steps;
        private int arrayLength=0;
        private int index = 0;
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            arrayLength = Steps.Length;
        }

        public void PlayFootstep()
        {
            audioSource.volume = agent.desiredVelocity.magnitude;

            audioSource.pitch = Random.Range(0.6f, 1f);

            audioSource.PlayOneShot(Steps[index]);
            index++;
            if (index >= arrayLength)
            {
                index = 0;
            }
        }
    }
}
