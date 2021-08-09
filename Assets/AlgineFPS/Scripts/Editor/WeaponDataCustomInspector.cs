using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Algine
{
    [CustomEditor(typeof(WeaponSetting))]
    public class WeaponDataCustomInspector : Editor
    {
        WeaponSetting settingSO;

        public override void OnInspectorGUI()
        {
            settingSO = target as WeaponSetting;

            DrawGeneral();

            if(settingSO.WeaponVariant == WeaponType.Melee)
            {
                DrawMelee();
            }
            else if (settingSO.WeaponVariant == WeaponType.GrenadeLauncher)
            {
                DrawGrenadeLauncher();
            }
            else if(settingSO.WeaponVariant == WeaponType.Grenade)
            {
                DrawGrenade();
            }
            else if(settingSO.WeaponVariant == WeaponType.Crossbow || settingSO.WeaponVariant == WeaponType.Bow)
            {
                DrawCrossbow();
            }
            else if(settingSO.WeaponVariant == WeaponType.Revolver)
            {
                DrawRevolver();
            }
            else
            {
                DrawFirearms();
            }
                

            EditorUtility.SetDirty(settingSO);
        }
        public void DrawGrenadeLauncher()
        {
            GUILayout.Label("Grenade Launcher settings", EditorStyles.boldLabel);
            GUILayout.BeginVertical("HelpBox");
            /// Main
            GUILayout.Label("Main settings", EditorStyles.centeredGreyMiniLabel);

            GUILayout.BeginVertical();
            GUILayout.Label("Fire rate in shot per second");
            settingSO.FireRate = EditorGUILayout.FloatField(settingSO.FireRate);
            GUILayout.Label("Grenade pool size");
            settingSO.ProjectilePoolSize = EditorGUILayout.IntSlider(settingSO.ProjectilePoolSize, 1, 100);
            GUILayout.EndVertical();
            /// Effects
            GUILayout.Label("Effects", EditorStyles.centeredGreyMiniLabel);
            GUILayout.BeginVertical();
            GUILayout.Label("MuzzleFlash effect");
            settingSO.MuzzleFlashParticlesFX = (ParticleSystem)EditorGUILayout.ObjectField(settingSO.MuzzleFlashParticlesFX, typeof(ParticleSystem), false);
            GUILayout.Label("Reloading sound effect");
            settingSO.ReloadingSFX = (AudioClip)EditorGUILayout.ObjectField(settingSO.ReloadingSFX, typeof(AudioClip), false);
            GUILayout.Label("Weapon empty sound effect");
            settingSO.EmptySFX = (AudioClip)EditorGUILayout.ObjectField(settingSO.EmptySFX, typeof(AudioClip), false);
            GUILayout.EndVertical();

            /// Objects
            GUILayout.Label("Required objects", EditorStyles.centeredGreyMiniLabel);
            GUILayout.BeginVertical();
            GUILayout.Label("Grenade settings", EditorStyles.boldLabel);
            GUILayout.Label("Grenade prefab");
            settingSO.GrenadePrefab = (GameObject)EditorGUILayout.ObjectField(settingSO.GrenadePrefab, typeof(GameObject), false);
            GUILayout.Label("Grenade throw force");
            settingSO.ThrowForce = EditorGUILayout.FloatField(settingSO.ThrowForce);

            GUILayout.EndVertical();

            GUILayout.EndVertical(); 
        }
        public void DrawCrossbow()
        {
            GUILayout.Label("Crossbow settings", EditorStyles.boldLabel);
            GUILayout.BeginVertical("HelpBox");
            /// Main
            GUILayout.Label("Main settings", EditorStyles.centeredGreyMiniLabel);
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Weapon damage. Minimum | Maximum");

            GUILayout.BeginHorizontal();
            settingSO.DamageMinimum = EditorGUILayout.IntField(settingSO.DamageMinimum);
            settingSO.DamageMaximum = EditorGUILayout.IntField(settingSO.DamageMaximum);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Rigidbody hit force");
            settingSO.RigidBodyHitForce = EditorGUILayout.FloatField(settingSO.RigidBodyHitForce);
            GUILayout.Label("Fire rate in shot per second");
            settingSO.FireRate = EditorGUILayout.FloatField(settingSO.FireRate);
            GUILayout.Label("Spread default factor");
            settingSO.SpreadAmount = EditorGUILayout.FloatField(settingSO.SpreadAmount);
            GUILayout.Label("Bow initial velocity");
            settingSO.BulletInitialVelocity = EditorGUILayout.FloatField(settingSO.BulletInitialVelocity);
            GUILayout.Label("Bow pool size");
            settingSO.ProjectilePoolSize = EditorGUILayout.IntSlider(settingSO.ProjectilePoolSize, 1, 100);
            GUILayout.EndVertical();
            
            GUILayout.Label("Effects", EditorStyles.centeredGreyMiniLabel);
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Reloading sound effect");
            settingSO.ReloadingSFX = (AudioClip)EditorGUILayout.ObjectField(settingSO.ReloadingSFX, typeof(AudioClip), false);
            GUILayout.Label("Weapon empty sound effect");
            settingSO.EmptySFX = (AudioClip)EditorGUILayout.ObjectField(settingSO.EmptySFX, typeof(AudioClip), false);
            GUILayout.Label("Weapon Ready sound effect");
            settingSO.ReadySFX = (AudioClip)EditorGUILayout.ObjectField(settingSO.ReadySFX, typeof(AudioClip), false);
            GUILayout.EndVertical();

            GUILayout.Label("Required objects", EditorStyles.centeredGreyMiniLabel);
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Bow prefab");
            settingSO.Projectile = (GameObject)EditorGUILayout.ObjectField(settingSO.Projectile, typeof(GameObject), false);

            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }

        public void DrawGeneral()
        {
            GUILayout.Label("General settings", EditorStyles.boldLabel);
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Weapon type");
            settingSO.WeaponVariant = (WeaponType)EditorGUILayout.EnumPopup(settingSO.WeaponVariant);
            GUILayout.Label("Weapon icon");
            settingSO.WeaponIcon = (Sprite)EditorGUILayout.ObjectField(settingSO.WeaponIcon, typeof(Sprite), false);
            GUILayout.Label("Shot or melee attack (swoosh) sound effect");
            settingSO.ShotSFX = (AudioClip)EditorGUILayout.ObjectField(settingSO.ShotSFX, typeof(AudioClip), false);
            GUILayout.Label("Recoil Vector3");
            settingSO.Recoil = EditorGUILayout.Vector3Field("Recoil", settingSO.Recoil);
            GUILayout.EndVertical();
        }

        public void DrawFirearms()
        {
            GUILayout.Label("Firearms settings", EditorStyles.boldLabel);
            GUILayout.BeginVertical("HelpBox");
            /// Main
            GUILayout.Label("Main settings", EditorStyles.centeredGreyMiniLabel);
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Weapon damage. Minimum | Maximum");
            GUILayout.BeginHorizontal();
            settingSO.DamageMinimum = EditorGUILayout.IntField(settingSO.DamageMinimum);
            settingSO.DamageMaximum = EditorGUILayout.IntField(settingSO.DamageMaximum);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            
            GUILayout.BeginVertical();
            GUILayout.Label("Rigidbody hit force");
            settingSO.RigidBodyHitForce = EditorGUILayout.FloatField(settingSO.RigidBodyHitForce);
            GUILayout.Label("Fire rate in shot per second");
            settingSO.FireRate = EditorGUILayout.FloatField(settingSO.FireRate);
            GUILayout.Label("Spread default factor");
            settingSO.SpreadAmount = EditorGUILayout.FloatField(settingSO.SpreadAmount);
            GUILayout.Label("Bullet initial velocity");
            settingSO.BulletInitialVelocity = EditorGUILayout.FloatField(settingSO.BulletInitialVelocity);
            GUILayout.Label("Shell ejected force");
            settingSO.ShellEjectingForce = EditorGUILayout.FloatField(settingSO.ShellEjectingForce);
   
            GUILayout.Label("Shell pool size");
            settingSO.ShellsPoolSize = EditorGUILayout.IntSlider(settingSO.ShellsPoolSize, 1, 100);
            GUILayout.EndVertical();

            GUILayout.Label("Effects", EditorStyles.centeredGreyMiniLabel);
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("MuzzleFlash effect");
            settingSO.MuzzleFlashParticlesFX = (ParticleSystem)EditorGUILayout.ObjectField(settingSO.MuzzleFlashParticlesFX, typeof(ParticleSystem), false);
            GUILayout.Label("Reloading sound effect");
            settingSO.ReloadingSFX = (AudioClip)EditorGUILayout.ObjectField(settingSO.ReloadingSFX, typeof(AudioClip), false);
            GUILayout.Label("Weapon empty sound effect");
            settingSO.EmptySFX = (AudioClip)EditorGUILayout.ObjectField(settingSO.EmptySFX, typeof(AudioClip), false);
            GUILayout.Label("Weapon Ready sound effect");
            settingSO.ReadySFX = (AudioClip)EditorGUILayout.ObjectField(settingSO.ReadySFX, typeof(AudioClip), false);
            GUILayout.EndVertical();
            /// Objects
            GUILayout.Label("Required objects", EditorStyles.centeredGreyMiniLabel);
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Shell object prefab");
            settingSO.Shell = (GameObject)EditorGUILayout.ObjectField(settingSO.Shell, typeof(GameObject), false);
       
            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }

        public void DrawRevolver()
        {
            GUILayout.Label("Firearms settings", EditorStyles.boldLabel);
            GUILayout.BeginVertical("HelpBox");
            /// Main
            GUILayout.Label("Main settings", EditorStyles.centeredGreyMiniLabel);
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Weapon damage. Minimum | Maximum");
            GUILayout.BeginHorizontal();
            settingSO.DamageMinimum = EditorGUILayout.IntField(settingSO.DamageMinimum);
            settingSO.DamageMaximum = EditorGUILayout.IntField(settingSO.DamageMaximum);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();


            GUILayout.BeginVertical();
            GUILayout.Label("Rigidbody hit force");
            settingSO.RigidBodyHitForce = EditorGUILayout.FloatField(settingSO.RigidBodyHitForce);
            GUILayout.Label("Fire rate in shot per second");
            settingSO.FireRate = EditorGUILayout.FloatField(settingSO.FireRate);
            GUILayout.Label("Spread default factor");
            settingSO.SpreadAmount = EditorGUILayout.FloatField(settingSO.SpreadAmount);
            GUILayout.Label("Bullet initial velocity");
            settingSO.BulletInitialVelocity = EditorGUILayout.FloatField(settingSO.BulletInitialVelocity);
            GUILayout.EndVertical();

            GUILayout.Label("Effects", EditorStyles.centeredGreyMiniLabel);
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("MuzzleFlash effect");
            settingSO.MuzzleFlashParticlesFX = (ParticleSystem)EditorGUILayout.ObjectField(settingSO.MuzzleFlashParticlesFX, typeof(ParticleSystem), false);
            GUILayout.Label("Reloading sound effect");
            settingSO.ReloadingSFX = (AudioClip)EditorGUILayout.ObjectField(settingSO.ReloadingSFX, typeof(AudioClip), false);
            GUILayout.Label("Weapon empty sound effect");
            settingSO.EmptySFX = (AudioClip)EditorGUILayout.ObjectField(settingSO.EmptySFX, typeof(AudioClip), false);
            GUILayout.EndVertical();
            GUILayout.EndVertical();
        }

        public void DrawGrenade()
        {
            GUILayout.Label("Grenade settings", EditorStyles.boldLabel);
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("first time sound effect");
            settingSO.ReadySFX = (AudioClip)EditorGUILayout.ObjectField(
                settingSO.ReadySFX, typeof(AudioClip), false);
            GUILayout.Label("Grenade prefab");
            settingSO.GrenadePrefab = (GameObject)EditorGUILayout.ObjectField(settingSO.GrenadePrefab, typeof(GameObject), false);
            GUILayout.Label("Grenade throw force");
            settingSO.ThrowForce = EditorGUILayout.FloatField(settingSO.ThrowForce);
            GUILayout.EndVertical();
        }

        public void DrawMelee()
        {
            GUILayout.Label("Melee settings", EditorStyles.boldLabel);
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Hit sound effect");
            settingSO.MeleeHitFX = (AudioClip)EditorGUILayout.ObjectField(settingSO.MeleeHitFX, typeof(AudioClip), false);
            GUILayout.Label("Ready sound effect");
            settingSO.ReadySFX = (AudioClip)EditorGUILayout.ObjectField(settingSO.ReadySFX, typeof(AudioClip), false);
            GUILayout.Label("Attack distance");
            settingSO.MeleeAttackDistance = EditorGUILayout.FloatField(settingSO.MeleeAttackDistance);
           GUILayout.Label("Damage points");
            settingSO.MeleeDamagePoints = EditorGUILayout.IntField(settingSO.MeleeDamagePoints);
            GUILayout.Label("Rigidbody hit force");
            settingSO.RigidBodyHitForce = EditorGUILayout.FloatField(settingSO.RigidBodyHitForce);
            GUILayout.EndVertical();
        }
    }
    
}
