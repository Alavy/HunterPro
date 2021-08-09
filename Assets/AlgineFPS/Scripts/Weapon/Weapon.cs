using Algine.FPS.MobileInput;
using System.CodeDom;
using System.Collections;
using UnityEngine;

namespace Algine
{
    public class Weapon : MonoBehaviour
    {
        [Header("Weapon setting")]
        public WeaponSetting WeaponSetting;
        [Tooltip("Set the name of weapon as in the weapon settings objects and pickup")]
        public string WeaponName;
        [HideInInspector]
        public WeaponType WeaponVariant;
        [Tooltip("Ammo item index")]
        public int AmmoItemID;

        [Tooltip("Transform to instantiate particle system shot fx")]
        public Transform MuzzleTransform;
        [Tooltip("Transform to eject shell after shot")]
        public Transform ShellTransform;
        [Tooltip("Transform to instantiate bullet on shot")]
        public Transform BulletTransform;
        public Vector3 AimPos;
        public Vector2 AimSensitivity;

        [Tooltip("How long reload animation is? Time in seconds to synch reloading animation with script")]
        public float ReloadAnimationDuration = 3.0f;

        [Tooltip("Should weapon reload when ammo is 0")]
        public bool AutoReload = true;
        [Tooltip("Does Weapon have Empty Animation")]
        public bool HasEmptyAnim = false;

        [Header("Ammo")]
        [Tooltip("Ammo count in weapon magazine")]
        public int CurrentAmmo = 30;
        [Tooltip("Max weapon ammo capacity")]
        public int AmmoClipSize = 30;
        public Transform Arrow;


        #region Utility variables
        private AudioClip m_shotSFX, m_reloadingSFX, m_emptySFX, m_readySFX;
  
        private int m_damageMin, m_damageMax;
        private float m_fireRate;
        private float m_rigidbodyHitForce;
        private float m_spreadAmount;
        private Vector3 m_recoil;
        
        #endregion

        //variables Shell
        private GameObject m_shell;
        private int m_shellPoolSize;
        private float m_shellForce;
        private GameObject[] m_shells;
        private int shellIndex = 0;


        //Exposed Variables
        public bool IsReloading { get; private set; }
        public bool IsAim { get; private set; }

        
        //projectiles variables
        private float m_bulletInitialVelocity = 360;
        private GameObject m_projectile;
        private GameObject[] m_projectiles;
        private int m_projectilePoolSize;
        private int m_projectilesIndex;

        private int m_calculatedDamage;

        //Melee variables (used only if weapon is melee type)
        private float m_meleeAttackDistance;
        private int m_meleeDamagePoints;
        private float m_meleeRigidbodyHitForce;
        private AudioClip m_meleeHitFX;

        //Grenade variables (used only if weapon is Grenade)
        public GameObject GrenadePrefab;
        public Transform GrenadeThrowTransform;

        //Component dependencies 
        private WeaponManager m_weaponManager;
        private Animator m_animator;
        private AudioSource m_audioSource;
        private ParticleSystem temp_MuzzleFlashParticlesFX;
        private ParticleSystem m_muzzleFlashParticlesFX;


        //Hold Intermidiate states
        private Vector3 m_localPos;
        private Vector2 m_normalSensitivity;
        private float nextFireTime;

        private bool m_blankFireTrace = true;
        private bool m_isAbleToFire = false;

        private void Awake()
        {
            GetWeaponSettings();

            m_animator = GetComponent<Animator>();
            m_audioSource = GetComponentInParent<AudioSource>();
            m_weaponManager = GetComponentInParent<WeaponManager>();

            //Inititial state
            IsAim = false;
            IsReloading = false;


            if (m_animator == null)
            {
                Debug.LogError("Please attach animator to your weapon object");
            }

            m_animator.keepAnimatorControllerStateOnDisable = true;
        }
        private void GetWeaponSettings()
        {
            WeaponVariant = WeaponSetting.WeaponVariant;

            if (WeaponVariant != WeaponType.Melee && WeaponVariant != WeaponType.Crossbow)
                m_muzzleFlashParticlesFX = WeaponSetting.MuzzleFlashParticlesFX;

            m_shotSFX = WeaponSetting.ShotSFX;
            m_reloadingSFX = WeaponSetting.ReloadingSFX;
            m_emptySFX = WeaponSetting.EmptySFX;
            m_readySFX = WeaponSetting.ReadySFX;

            m_shell = WeaponSetting.Shell;
            m_shellPoolSize = WeaponSetting.ShellsPoolSize;
            m_shellForce = WeaponSetting.ShellEjectingForce;

            m_damageMin = WeaponSetting.DamageMinimum;
            m_damageMax = WeaponSetting.DamageMaximum;
            m_fireRate = WeaponSetting.FireRate;
            m_rigidbodyHitForce = WeaponSetting.RigidBodyHitForce;
            m_recoil = WeaponSetting.Recoil;
            m_spreadAmount = WeaponSetting.SpreadAmount;

            m_bulletInitialVelocity = WeaponSetting.BulletInitialVelocity;
            m_projectile = WeaponSetting.Projectile;
            m_projectilePoolSize = WeaponSetting.ProjectilePoolSize;

            if (WeaponVariant == WeaponType.Melee)
            {
                m_meleeAttackDistance = WeaponSetting.MeleeAttackDistance;
                m_meleeDamagePoints = WeaponSetting.MeleeDamagePoints;
                m_meleeRigidbodyHitForce = WeaponSetting.MeleeRigidbodyHitForce;
                m_meleeHitFX = WeaponSetting.MeleeHitFX;
            }

        }
        private void Start()
        {
            if (WeaponVariant == WeaponType.GrenadeLauncher)
                grenadeLauncherProjectilesPool();
            else if (WeaponVariant == WeaponType.Crossbow || WeaponVariant == WeaponType.Bow)
                arrowPool();

            if (WeaponVariant != WeaponType.Melee && WeaponVariant != WeaponType.Grenade)
            {
                if (m_shell)
                    shellsPool();


                if (m_muzzleFlashParticlesFX)
                    temp_MuzzleFlashParticlesFX = Instantiate(m_muzzleFlashParticlesFX, 
                        MuzzleTransform.position, MuzzleTransform.rotation, MuzzleTransform);
               
            }
            m_localPos = transform.localPosition;
            m_normalSensitivity = m_weaponManager.PlayerController.Sensitivity;
        }
        private void OnDisable()
        {
            transform.localPosition = m_localPos;
            if (WeaponVariant == WeaponType.Bow)
            {
                m_animator.SetBool("BowAim", false);
            }
            IsReloading = false;
            IsAim = false;
            m_isAbleToFire = false;
        }

        public void FireWeapon()
        {
            if(WeaponVariant == WeaponType.Bow)
            {
                if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("BowAim"))
                {
                    fireBOW();
                }
            }
            else
            {
                if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    m_isAbleToFire = true;
                }

                if (m_isAbleToFire)
                {
                    fireSMG();
                }
                
            }
        }
        public void AimWeapon()
        {
            if (WeaponVariant != WeaponType.Melee 
                && WeaponVariant != WeaponType.Grenade)
            {
                handleAim();
            }

        }
        public void ReloadWeapon()
        {
            if (!IsReloading && CalculateTotalAmmo() > 0 &&
               CurrentAmmo != AmmoClipSize)
            {
                //setAim = false;
                IsReloading = true;

                //Aim handle
                if (IsAim && WeaponVariant != WeaponType.Bow)
                {
                    aimAnimation(false);
                }

                m_animator.Play(AnimatorHash.hash_Reload);
                m_audioSource.PlayOneShot(m_reloadingSFX);

                if (WeaponVariant == WeaponType.Crossbow || WeaponVariant == WeaponType.Bow)
                {
                    Arrow.gameObject.SetActive(true);
                }

                StartCoroutine(reloadEnd(ReloadAnimationDuration));
            }
        }
        public void ReadyWeapon()
        {
            gameObject.SetActive(true);
            m_animator.SetBool("Hide", false);
            if (m_readySFX)
            {
                m_audioSource.PlayOneShot(m_readySFX);
            }
            if (HasEmptyAnim)
            {
                if (CurrentAmmo == 0)
                {
                    m_animator.SetBool(AnimatorHash.hash_Empty, true);
                }
                else
                {
                    m_animator.SetBool(AnimatorHash.hash_Empty, false);
                }
            }
            m_animator.Play(AnimatorHash.hash_Ready);
        }
        public void HideWeapon()
        {
            m_animator.SetBool("Hide",true);
            StartCoroutine(deactiveAfter(m_animator.
                GetCurrentAnimatorStateInfo(0).length));
        }
        public void UnHideWeapon()
        {
            gameObject.SetActive(true);
            m_animator.SetBool("Hide", false);
        }
        public int CalculateTotalAmmo()
        {
            int totalAmmo = new int();

            Item item = m_weaponManager.Inventory.FindItem(AmmoItemID);
            if (item == null)
            {
                totalAmmo = 0;
            }
            else
            {
                totalAmmo = item.Ammo;
            }
            return totalAmmo;
        }

        private void fireBOW()
        {
            if (Time.time > nextFireTime &&
                    !IsReloading && !m_weaponManager.PlayerController.IsClimbing)
            {
                if (CurrentAmmo > 0)
                {
                    m_calculatedDamage = Random.Range(m_damageMin, m_damageMax);
                    ArrowManager();
                    m_weaponManager.RecoilComponent.AddRecoil(m_recoil);
                    PlayBowFX();
                    CurrentAmmo -= 1;
                    nextFireTime = Time.time + m_fireRate;
                }
            }
        }

        private void fireSMG()
        {
            
            if (WeaponVariant == WeaponType.Crossbow)
            {
                if (Time.time > nextFireTime &&
                    !IsReloading  && !m_weaponManager.PlayerController.IsClimbing)
                {
                    if (CurrentAmmo > 0)
                    {
                        m_calculatedDamage = Random.Range(m_damageMin, m_damageMax);
                        PlayCrossbowFX();
                        ArrowManager();
                        m_weaponManager.RecoilComponent.AddRecoil(m_recoil);
                        CurrentAmmo -= 1;
                        nextFireTime = Time.time + m_fireRate;

                    }
                    else
                    {
                        if (!IsReloading && AutoReload)
                        {
                            ReloadWeapon();
                        }
                        else
                        {
                            if (m_blankFireTrace)
                            {
                                m_audioSource.PlayOneShot(m_emptySFX);
                                m_blankFireTrace = false;
                            }

                        }
                        nextFireTime = Time.time + m_fireRate;
                    }
                }
            }
            else if(WeaponVariant == WeaponType.GrenadeLauncher)
            {
                if (Time.time > nextFireTime && !IsReloading
                    && !m_weaponManager.PlayerController.IsClimbing) //Allow fire statement
                {
                    if (CurrentAmmo > 0)
                    {

                        m_calculatedDamage = Random.Range(m_damageMin, m_damageMax);

                        PlayGrenadeLauncherFX();
                        GrenadeManager();
                        m_weaponManager.RecoilComponent.AddRecoil(m_recoil);
                        

                        CurrentAmmo -= 1;
                        nextFireTime = Time.time + m_fireRate;
                    }
                    else
                    {
                        if (!IsReloading && AutoReload)
                        {
                            ReloadWeapon();
                        }
                        else
                        {
                            if (m_blankFireTrace)
                            {
                                m_audioSource.PlayOneShot(m_emptySFX);
                                m_blankFireTrace = false;
                                //StartCoroutine(blankShort());
                            }

                        }


                        nextFireTime = Time.time + m_fireRate;
                    }
                }
            }
            else if(WeaponVariant == WeaponType.Shotgun)
            {
                if (Time.time > nextFireTime && !IsReloading && !m_weaponManager.
                    PlayerController.IsClimbing) //Allow fire statement
                {
                    if (CurrentAmmo > 0)
                    {
                        m_calculatedDamage = Random.Range(m_damageMin, m_damageMax);

                        PlayFX();

                        ShortBulletManager();
                        m_weaponManager.RecoilComponent.AddRecoil(m_recoil);

                        CurrentAmmo -= 1;
                        nextFireTime = Time.time + m_fireRate;
                    }
                    else
                    {
                        if (!IsReloading && AutoReload)
                        {
                            ReloadWeapon();
                        }
                            
                        else
                        {
                            if (m_blankFireTrace)
                            {
                                m_audioSource.PlayOneShot(m_emptySFX);
                                m_blankFireTrace = false;
                                //StartCoroutine(blankShort());
                            }

                        }


                        nextFireTime = Time.time + m_fireRate;
                    }
                }
            }
            else
            {
                if (Time.time > nextFireTime && !IsReloading 
                     && !m_weaponManager.PlayerController.IsClimbing)
                {
                    if (CurrentAmmo > 0)
                    {
                        m_calculatedDamage = Random.Range(m_damageMin, m_damageMax);

                        PlayFX();

                        ProjectilesManager();

                        m_weaponManager.RecoilComponent.AddRecoil(m_recoil);

                        CurrentAmmo -= 1;

                        nextFireTime = Time.time + m_fireRate;
                        
                    }
                    else
                    {
                        if (!IsReloading && AutoReload)
                        {
                            ReloadWeapon();
                        }
                            
                        else
                        {
                            if (m_blankFireTrace)
                            {
                                m_audioSource.PlayOneShot(m_emptySFX);
                                m_blankFireTrace = false;
                                //StartCoroutine(blankShort());
                            }

                        }
                        nextFireTime = Time.time + m_fireRate;
                    }
                }
            }
        }
        private void handleAim()
        {
            IsAim = !IsAim;

            if (!IsReloading)
            {
                if (IsAim)
                {
                    if (WeaponVariant == WeaponType.Bow)
                    {
                        m_animator.SetBool("BowAim", true);
                        m_audioSource.PlayOneShot(m_shotSFX);
                    }
                    else
                    {
                        aimAnimation(true);
                    }
                    m_weaponManager.Reticle.gameObject.SetActive(false);
                }
                else
                {
                    if(WeaponVariant == WeaponType.Bow)
                    {
                        m_animator.SetBool("BowAim", false);
                    }
                    else
                    {
                        aimAnimation(false);

                    }
                    m_weaponManager.Reticle.gameObject.SetActive(true);

                }
            }
            else
            {
                IsAim = false;
            }
        }
        private void aimAnimation(bool state)
        {
            if (state)
            {
                m_weaponManager.PlayerController.Sensitivity = AimSensitivity;
                transform.LeanMoveLocal(AimPos, .2f);
            }
            else
            {
                m_weaponManager.PlayerController.Sensitivity = m_normalSensitivity;
                transform.LeanMoveLocal(m_localPos, .2f);
            }

        }
        private void applyHit(RaycastHit hit)
        {
            //Set tag and transform of hit to HitParticlesFXManager
            m_weaponManager.HitFXComponent.HitParticlesFXManager(hit);
            m_weaponManager.HitFXComponent.DecalManager(hit);

            if (hit.collider.CompareTag("NPC"))
            {
                hit.transform.root.SendMessage("Damage", m_calculatedDamage,
                    SendMessageOptions.DontRequireReceiver);
            }
            else if (hit.collider.CompareTag("Head"))
            {
                hit.transform.root.SendMessage("Damage", m_damageMax,
                    SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                if (hit.rigidbody)
                {
                    if (hit.rigidbody.isKinematic == false)
                    {
                        //Add force to the rigidbody for bullet impact effect
                        hit.rigidbody.AddForceAtPosition(m_rigidbodyHitForce * m_damageMin *
                        m_weaponManager.MainCamera.transform.forward, hit.point);
                    }
                }

            }
        }
        IEnumerator reloadEnd(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            IsReloading = false;

            var ammoItem = m_weaponManager.Inventory.FindItem(AmmoItemID);

            if(ammoItem != null)
            {
                var neededAmmo = AmmoClipSize - CurrentAmmo;

                ///< new updated code 
                if(ammoItem.Ammo >= neededAmmo)
                {
                    ammoItem.Ammo -= neededAmmo;
                    CurrentAmmo += neededAmmo;
                }
                else
                {
                    CurrentAmmo = ammoItem.Ammo;
                    ammoItem.Ammo = 0;
                }

                if (ammoItem.Ammo <= 0)
                {
                    m_weaponManager.Inventory.DestroyItem(AmmoItemID);
                }
            }

            if (IsAim && WeaponVariant != WeaponType.Bow)
            {
                aimAnimation(true);
            }
            InputEvents.Current.FireTrigger(false);

        }
        IEnumerator deactiveAfter(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            gameObject.SetActive(false);
        }

        #region Decal, projectiles, shot FX, hitFX managers
        private void GrenadeManager()
        {
            m_projectiles[m_projectilesIndex].transform.position = MuzzleTransform.position;
            m_projectiles[m_projectilesIndex].transform.rotation = MuzzleTransform.rotation;
            m_projectiles[m_projectilesIndex].transform.Rotate(90, 0, 0);
            m_projectiles[m_projectilesIndex].SetActive(true);
            m_projectiles[m_projectilesIndex].GetComponent<Rigidbody>().AddForce(transform.forward * 
                WeaponSetting.ThrowForce);
            m_projectilesIndex++;

            if (m_projectilesIndex == m_projectiles.Length)
            {
                m_projectilesIndex = 0;
            }
        }

        private void ArrowManager()
        {
            var spreadWithMovementFactor = m_spreadAmount;

            ///Make lower spread factor if player is aiming
            if (IsAim)
                spreadWithMovementFactor /= 3;

            Vector3 spreadVector = new Vector3(Random.Range(-spreadWithMovementFactor,
                spreadWithMovementFactor),
                Random.Range(-spreadWithMovementFactor,
                spreadWithMovementFactor),
                Random.Range(-spreadWithMovementFactor,
                spreadWithMovementFactor));

            if (m_projectilesIndex == m_projectiles.Length)
            {
                m_projectilesIndex = 0;
            }

            if (IsAim)
            {
                RaycastHit hit;
                if (Physics.Raycast(BulletTransform.position
                    + spreadVector, BulletTransform.forward, out hit,
                         (int)m_bulletInitialVelocity))
                {
                    m_projectiles[m_projectilesIndex].transform.position = hit.point;

                    m_projectiles[m_projectilesIndex].transform.rotation = Quaternion.LookRotation(
                       transform.forward);

                    m_projectiles[m_projectilesIndex].transform.parent = hit.transform;

                    m_projectiles[m_projectilesIndex].SetActive(true);

                    m_projectilesIndex++;

                    applyHit(hit);
                }
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(m_weaponManager.MainCamera.transform.position
                    + spreadVector, m_weaponManager.MainCamera.transform.forward, out hit,
                         (int)m_bulletInitialVelocity))
                {

                    m_projectiles[m_projectilesIndex].transform.position = hit.point;

                    m_projectiles[m_projectilesIndex].transform.rotation = Quaternion.LookRotation(
                       transform.forward);

                    m_projectiles[m_projectilesIndex].transform.parent = hit.transform;

                    m_projectiles[m_projectilesIndex].SetActive(true);
                    m_projectilesIndex++;

                    applyHit(hit);
                }
            }
           
        }

        private void ShortBulletManager()
        {
            Transform temp;
            if (IsAim)
            {
                temp = BulletTransform;
            }
            else
            {
                temp = m_weaponManager.MainCamera.transform;
            }
            int rand = Random.Range(7, 14);
            for(int i=0;i< 7;i++)
            {
                Vector3 direction = temp.transform.forward; // your initial aim.
                Vector3 spread = new Vector3();
                spread += transform.up * Random.Range(-1f, 1f); // add random up or down (because random can get negative too)
                spread += transform.right * Random.Range(-1f, 1f); // add random left or right

                // Using random up and right values will lead to a square spray pattern. If we normalize this vector, we'll get the spread direction, but as a circle.
                // Since the radius is always 1 then (after normalization), we need another random call. 
                direction += spread.normalized * Random.Range(0f, 0.2f);

                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(temp.transform.position, direction, out hit,
                    (int)m_bulletInitialVelocity))
                {
                    applyHit(hit);
                }
            }
        }

        public void ProjectilesManager()
        {
            
            var spreadWithMovementFactor = m_spreadAmount ;

            ///Make lower spread factor if player is aiming
            if (IsAim)
                spreadWithMovementFactor /= 3;

            Vector3 spreadVector = new Vector3(Random.Range(-spreadWithMovementFactor,
                spreadWithMovementFactor),
                Random.Range(-spreadWithMovementFactor, 
                spreadWithMovementFactor),
                Random.Range(-spreadWithMovementFactor,
                spreadWithMovementFactor));

            if (IsAim)
            {
                RaycastHit hit;
                if (Physics.Raycast(BulletTransform.position 
                    + spreadVector, BulletTransform.forward, out hit,
                         (int)m_bulletInitialVelocity))
                {
                    applyHit(hit);
                }
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(m_weaponManager.MainCamera.transform.position
                    + spreadVector, m_weaponManager.MainCamera.transform.forward, out hit,
                         (int)m_bulletInitialVelocity))
                {
                    applyHit(hit);
                }
            }

        }

        private void PlayCrossbowFX()
        {
            if(CurrentAmmo == 1)
            {
                m_animator.SetBool(AnimatorHash.hash_Empty, true);
            }
            else
            {
                m_animator.SetBool(AnimatorHash.hash_Empty, false);

            }

            m_animator.Play(AnimatorHash.hash_Shot);

            StartCoroutine(handleStick(m_animator.GetCurrentAnimatorStateInfo(0).length));

            m_audioSource.PlayOneShot(m_shotSFX);
        }
        private void PlayBowFX()
        {
            m_animator.SetTrigger("Fire");
            StartCoroutine(handleArrow(m_animator.
                GetCurrentAnimatorStateInfo(0).length));
        }

        IEnumerator handleStick(float waitTime)
        {
            yield return new WaitForSeconds(waitTime * 0.15f);
            Arrow.gameObject.SetActive(false);
        }

        IEnumerator handleArrow(float waitTime)
        {
            yield return new WaitForSeconds(waitTime * 0.15f);
            Arrow.gameObject.SetActive(false);

            yield return new WaitForSeconds(waitTime);

            if(CalculateTotalAmmo()<= 0)
            {
                handleAim();
                yield return null;
            }
            if (!IsReloading && AutoReload)
            {
                ReloadWeapon();
            }
            else
            {
                if (m_blankFireTrace)
                {
                    m_audioSource.PlayOneShot(m_emptySFX);
                    m_blankFireTrace = false;
                }

            }
            nextFireTime = Time.time + m_fireRate;
        }

        private void PlayGrenadeLauncherFX()
        {
            m_animator.Play(AnimatorHash.hash_Shot);

            temp_MuzzleFlashParticlesFX.time = 0;
            temp_MuzzleFlashParticlesFX.Play();

            m_audioSource.PlayOneShot(m_shotSFX);
        }

        private void PlayFX()
        {
            if (HasEmptyAnim)
            {
                if (CurrentAmmo == 1)
                {
                    m_animator.SetBool(AnimatorHash.hash_Empty, true);
                }
                else
                {
                    m_animator.SetBool(AnimatorHash.hash_Empty, false);
                }
            }
            

            m_animator.Play(AnimatorHash.hash_Shot);
            //m_audioSource.Stop();
            m_audioSource.PlayOneShot(m_shotSFX);

            temp_MuzzleFlashParticlesFX.time = 0;
            temp_MuzzleFlashParticlesFX.Play();

            

            if (m_shells != null)
            {
                m_shells[shellIndex].GetComponent<Rigidbody>().velocity = Vector3.zero;
                m_shells[shellIndex].SetActive(true);
                
                m_shells[shellIndex].transform.position = ShellTransform.position;
                m_shells[shellIndex].transform.rotation = ShellTransform.rotation;

                m_shells[shellIndex].GetComponent<Rigidbody>().AddForce(
                      ShellTransform.right *m_shellForce,
                    ForceMode.Impulse);

                shellIndex++;

                if (shellIndex == m_shells.Length)
                {
                    shellIndex = 0;
                }
            }
        }

        #endregion

        #region Pool methods

        private void shellsPool()
        {
            m_shells = new GameObject[m_shellPoolSize];
            var shellsParentObject = new GameObject(WeaponName + "_shellsPool");
            shellsParentObject.transform.hierarchyCapacity = m_shellPoolSize;

            for (int i = 0; i < m_shellPoolSize; i++)
            {
                m_shells[i] = Instantiate(m_shell, shellsParentObject.transform);
                m_shells[i].SetActive(false);
            }
        }
        private void grenadeLauncherProjectilesPool()
        {
            m_projectiles = new GameObject[m_projectilePoolSize];
            var projectile = Instantiate(WeaponSetting.GrenadePrefab);

            var projectilesParentObject = new GameObject(WeaponName + "_projectilesPool" + 
                " " + WeaponName);

            projectilesParentObject.transform.hierarchyCapacity = m_projectilePoolSize;

            for (int i = 0; i < m_projectilePoolSize; i++)
            {
                m_projectiles[i] = Instantiate(projectile, projectilesParentObject.transform);
                m_projectiles[i].SetActive(false);
            }
        }
        private void arrowPool()
        {
            m_projectiles = new GameObject[m_projectilePoolSize];

            var arrow = Instantiate(m_projectile);

            var projectilesParentObject = new GameObject(WeaponName + "_projectilesPool" + 
                " " + WeaponName);

            projectilesParentObject.transform.hierarchyCapacity = m_projectilePoolSize;

            for (int i = 0; i < m_projectilePoolSize; i++)
            {
                m_projectiles[i] = Instantiate(arrow, projectilesParentObject.transform);
                m_projectiles[i].SetActive(false);
            }
        }

        #endregion
    }
}
