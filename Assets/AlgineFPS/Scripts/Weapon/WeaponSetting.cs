using UnityEngine;

namespace Algine
{
    public enum WeaponType { SMG, Revolver, 
        Shotgun, Melee, Grenade, Crossbow,GrenadeLauncher, Bow }

    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon Data", order = 0)]
    public class WeaponSetting : ScriptableObject
    {
        public WeaponType WeaponVariant;
        public Sprite WeaponIcon;

        [Header("Settings of objects")]

        [Tooltip("Particle system which being started after calling fire method")]
        public ParticleSystem MuzzleFlashParticlesFX;
        [Tooltip("SFX for each type of weapon state")]
        public AudioClip ShotSFX, ReloadingSFX, EmptySFX,ReadySFX;

        [Tooltip("Shell object which drops from the defined point in Weapon() class after calling fire method")]
        public GameObject Shell;
        [Tooltip("Maximal amount of shell which being created on Start()")]
        public int ShellsPoolSize;
        [Tooltip("Force with shells ejected from weapon")]
        public float ShellEjectingForce;

        [Header("Weapon stats")]

        [Tooltip("Maximal and minimal damage ammounts to apply on target")]
        public int DamageMinimum;
        public int DamageMaximum;
        
        [Tooltip("Force that be applyed to rigidbody on raycast hit")]
        public float RigidBodyHitForce;
        [Tooltip("Time in seconds to call next fire. 1 means 1 shot per second, 0.5 means 2 shots per second etc")]
        public float FireRate;
        [Tooltip("Bullet spread value. Influence on shooting trajectory in static reticle mod")]
        public float SpreadAmount = 0.01f;
        [Tooltip("Recoil value to X axis of camera")]
        public Vector3 Recoil;

        [Header("Melee settings (for melee only!)")]
        public float MeleeAttackDistance;
        public int MeleeDamagePoints;
        public float MeleeRigidbodyHitForce;
        public AudioClip MeleeHitFX;

        [Header("Ballistic settings")]
        [Tooltip("Initial bullet velocity in meters per second. Recomended to take real weapons parameters")]
        public float BulletInitialVelocity = 360;
        [Tooltip("Projectile prefab used as projectile. Select one from prefabs")]
        public GameObject Projectile;
        [Tooltip("Max amount of projectiles which being created on Start()")]
        [Range(1, 100)]
        public int ProjectilePoolSize = 1;

        [Header("Grenade settings")]
        public GameObject GrenadePrefab;
        public float ThrowForce = 1500;
    }
}
