
using UnityEngine;

namespace Algine
{
    public class HitFXManager : MonoBehaviour
    {
        [Header("Hit Particles FX")]
        public ParticleSystem concreteHitFX;
        public ParticleSystem woodHitFX;
        public ParticleSystem dirtHitFX;
        public ParticleSystem metalHitFX;
        public ParticleSystem bloodHitFX;

        [HideInInspector]
        public ParticleSystem objConcreteHitFX;
        [HideInInspector]
        public ParticleSystem objWoodHitFX;
        [HideInInspector]
        public ParticleSystem objDirtHitFX;
        [HideInInspector]
        public ParticleSystem objMetalHitFX;
        [HideInInspector]
        public ParticleSystem objBloodHitFX;

        [Header("Decals")]
        [Range(1, 30)]
        [Tooltip("Pool size for each type of decals. For example," +
            " if pool size is 10, concrete, wood, dirt, metal decals" +
            " will have 10 their instances after start")]

        public int decalsPoolSizeForEachType;

        public GameObject concreteDecal;
        public GameObject woodDecal;
        public GameObject dirtDecal;
        public GameObject metalDecal;

        private int decalIndex_wood = 0;
        private int decalIndex_concrete = 0;
        private int decalIndex_dirt = 0;
        private int decalIndex_metal = 0;

        public bool isNPC = false;

        private GameObject[] concreteDecal_pool;
        private GameObject[] woodDecal_pool;
        private GameObject[] dirtDecal_pool;
        private GameObject[] metalDecal_pool;

        void Awake()
        {
            if (!isNPC)
            {
                DecalsPool();
            }
            objConcreteHitFX = Instantiate(concreteHitFX);
            objWoodHitFX = Instantiate(woodHitFX);
            objDirtHitFX = Instantiate(dirtHitFX);
            objMetalHitFX = Instantiate(metalHitFX);
            objBloodHitFX = Instantiate(bloodHitFX);
        }

        private void DecalsPool()
        {
            concreteDecal_pool = new GameObject[decalsPoolSizeForEachType];
            var decalsParentObject_concrete = new GameObject("decalsPool_concrete");
            /////<for increasing performance
            ///
            decalsParentObject_concrete.transform.hierarchyCapacity = decalsPoolSizeForEachType;

            for (int i = 0; i < decalsPoolSizeForEachType; i++)
            {
                concreteDecal_pool[i] = Instantiate(concreteDecal, decalsParentObject_concrete.transform);
                concreteDecal_pool[i].SetActive(false);
            }

            woodDecal_pool = new GameObject[decalsPoolSizeForEachType];
            /////<for increasing performance
            ///
            var decalsParentObject_wood = new GameObject("decalsPool_wood");
            decalsParentObject_wood.transform.hierarchyCapacity = decalsPoolSizeForEachType;

            for (int i = 0; i < decalsPoolSizeForEachType; i++)
            {
                woodDecal_pool[i] = Instantiate(woodDecal, decalsParentObject_wood.transform);
                woodDecal_pool[i].SetActive(false);
            }

            dirtDecal_pool = new GameObject[decalsPoolSizeForEachType];
            /////<for increasing performance
            ///
            var decalsParentObject_dirt = new GameObject("decalsPool_dirt");
            decalsParentObject_dirt.transform.hierarchyCapacity = decalsPoolSizeForEachType;

            for (int i = 0; i < decalsPoolSizeForEachType; i++)
            {
                dirtDecal_pool[i] = Instantiate(dirtDecal, decalsParentObject_dirt.transform);
                dirtDecal_pool[i].SetActive(false);
            }

            metalDecal_pool = new GameObject[decalsPoolSizeForEachType];
            /////<for increasing performance
            ///
            var decalsParentObject_metal = new GameObject("decalsPool_metal");
            decalsParentObject_metal.transform.hierarchyCapacity = decalsPoolSizeForEachType;

            for (int i = 0; i < decalsPoolSizeForEachType; i++)
            {
                metalDecal_pool[i] = Instantiate(metalDecal, decalsParentObject_metal.transform);
                metalDecal_pool[i].SetActive(false);
            }

        }

        public void HitParticlesFXManager(RaycastHit hit)
        {
            if (hit.collider.CompareTag("Wood"))
            {
                objWoodHitFX.Stop();
                objWoodHitFX.transform.position = new Vector3(hit.point.x,
                    hit.point.y, hit.point.z);
                objWoodHitFX.transform.LookAt(transform.position);
                objWoodHitFX.Play(true);
            }
            else if (hit.collider.CompareTag("Concrete"))
            {
                objConcreteHitFX.Stop();
                objConcreteHitFX.transform.position = new Vector3(hit.point.x,
                    hit.point.y, hit.point.z);
                objConcreteHitFX.transform.LookAt(transform.position);
                objConcreteHitFX.Play(true);
            }
            else if (hit.collider.CompareTag("Dirt"))
            {
                objDirtHitFX.Stop();
                objDirtHitFX.transform.position = new Vector3(hit.point.x,
                    hit.point.y, hit.point.z);

                objDirtHitFX.transform.LookAt(
                    transform.position);

                objDirtHitFX.Play(true);
            }
            else if (hit.collider.CompareTag("Metal"))
            {
                objMetalHitFX.Stop();
                objMetalHitFX.transform.position = new Vector3(hit.point.x,
                    hit.point.y, hit.point.z);
                objMetalHitFX.transform.LookAt(
                    transform.position);
                objMetalHitFX.Play(true);
            }
            else if (hit.collider.CompareTag("NPC") || hit.collider.CompareTag("Head"))
            {
                objBloodHitFX.Stop();
                objBloodHitFX.transform.position = new Vector3(hit.point.x,
                    hit.point.y, hit.point.z);
                objBloodHitFX.transform.LookAt(
                    transform.position);
                objBloodHitFX.Play(true);
            }
            else
            {
                objConcreteHitFX.Stop();
                objConcreteHitFX.transform.position = new Vector3(hit.point.x,
                    hit.point.y, hit.point.z);
                objConcreteHitFX.transform.LookAt(
                    transform.position);
                objConcreteHitFX.Play(true);
            }

        }

        public void DecalManager(RaycastHit hit)
        {
            if (hit.collider.CompareTag("Concrete"))
            {
                concreteDecal_pool[decalIndex_concrete].SetActive(true);
                var decalPostion = hit.point + hit.normal * 0.025f;
                concreteDecal_pool[decalIndex_concrete].transform.position = decalPostion;
                concreteDecal_pool[decalIndex_concrete].transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);

                decalIndex_concrete++;

                if (decalIndex_concrete >= decalsPoolSizeForEachType)
                {
                    decalIndex_concrete = 0;
                }
            }
            else if (hit.collider.CompareTag("Wood"))
            {
                woodDecal_pool[decalIndex_wood].SetActive(true);
                var decalPostion = hit.point + hit.normal * 0.025f;
                woodDecal_pool[decalIndex_wood].transform.position = decalPostion;
                woodDecal_pool[decalIndex_wood].transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);

                decalIndex_wood++;

                if (decalIndex_wood >= decalsPoolSizeForEachType)
                {
                    decalIndex_wood = 0;
                }
            }
            else if (hit.collider.CompareTag("Dirt"))
            {
                dirtDecal_pool[decalIndex_dirt].SetActive(true); var decalPostion = hit.point + hit.normal * 0.025f;
                dirtDecal_pool[decalIndex_dirt].transform.position = decalPostion;
                dirtDecal_pool[decalIndex_dirt].transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);

                decalIndex_dirt++;

                if (decalIndex_dirt >= decalsPoolSizeForEachType)
                {
                    decalIndex_dirt = 0;
                }
            }
            else if (hit.collider.CompareTag("Metal"))
            {
                metalDecal_pool[decalIndex_metal].SetActive(true);
                var decalPostion = hit.point + hit.normal * 0.025f;
                metalDecal_pool[decalIndex_metal].transform.position = decalPostion;
                metalDecal_pool[decalIndex_metal].transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);

                decalIndex_metal++;

                if (decalIndex_metal >= decalsPoolSizeForEachType)
                {
                    decalIndex_metal = 0;
                }
            }
            /*
            else
            {
                hitFXManager.concreteDecal_pool[decalIndex_concrete].SetActive(true);
                var decalPostion = hit.point + hit.normal * 0.025f;
                hitFXManager.concreteDecal_pool[decalIndex_concrete].transform.position = decalPostion;
                hitFXManager.concreteDecal_pool[decalIndex_concrete].transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);
                if (applyParent)
                    decals[decalIndex_concrete].transform.parent = hit.transform;

                decalIndex_concrete++;

                if (decalIndex_concrete >= hitFXManager.decalsPoolSizeForEachType)
                {
                    decalIndex_concrete = 0;
                }
            }
            */
        }

    }
}