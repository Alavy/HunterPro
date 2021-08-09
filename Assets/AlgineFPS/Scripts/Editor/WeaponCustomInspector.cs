using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

namespace Algine {

    [CustomEditor(typeof(Weapon))]
    public class WeaponCustomInspector : Editor
    {
        Weapon weapon;

        public override void OnInspectorGUI()
        {
            weapon = target as Weapon;
            
            DrawGeneral();
    
            if(weapon.WeaponSetting.WeaponVariant == WeaponType.Melee)
            {
                DrawMelee();
            }
            else if(weapon.WeaponSetting.WeaponVariant == WeaponType.Grenade)
            {
                DrawGrenade();
            }
            else if (weapon.WeaponSetting.WeaponVariant == WeaponType.Crossbow)
            {
                DrawCrossbow();
            }else if (weapon.WeaponSetting.WeaponVariant == WeaponType.Bow)
            {
                DrawBow();
            }
            else if (weapon.WeaponSetting.WeaponVariant == WeaponType.GrenadeLauncher)
            {
                DrawGrenadeLauncher();
            }
            else
            {
                DrawFirearms();
            }

            EditorUtility.SetDirty(weapon);
        }
        public void DrawBow()
        {
            GUILayout.Label("Firearms settings", EditorStyles.boldLabel);
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Ammo Item ID");
            weapon.AmmoItemID = EditorGUILayout.IntField(weapon.AmmoItemID);
            GUILayout.Label("Reload animation duration");
            weapon.ReloadAnimationDuration = EditorGUILayout.FloatField(weapon.ReloadAnimationDuration);
            GUILayout.Label("Auto reloading");
            weapon.AutoReload = EditorGUILayout.Toggle(weapon.AutoReload);
            GUILayout.Label("Current ammo value");
            weapon.CurrentAmmo = EditorGUILayout.IntField(weapon.CurrentAmmo);
            GUILayout.Label("Ammo Clip Size");
            weapon.AmmoClipSize = EditorGUILayout.IntField(weapon.AmmoClipSize);
            GUILayout.Label("Arrow Reference");
            weapon.Arrow = (Transform)EditorGUILayout.ObjectField(weapon.Arrow, typeof(Transform), true);
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Helpers", EditorStyles.centeredGreyMiniLabel);
            GUILayout.Label("Bullet spawn transform");
            weapon.BulletTransform = (Transform)EditorGUILayout.ObjectField(weapon.BulletTransform, typeof(Transform), true);
            GUILayout.Label("Aim Sensitivity");
            weapon.AimSensitivity = EditorGUILayout.Vector2Field("Values X And Y", weapon.AimSensitivity);
            EditorGUILayout.HelpBox("Z axis is forward axis of each transform. Check forward for each helper to set right direction!", MessageType.Warning);
            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }
        public void DrawGrenadeLauncher()
        {
            GUILayout.Label("Firearms settings", EditorStyles.boldLabel);
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Ammo Item ID");
            weapon.AmmoItemID = EditorGUILayout.IntField(weapon.AmmoItemID);
            GUILayout.Label("Reload animation duration");
            weapon.ReloadAnimationDuration = EditorGUILayout.FloatField(weapon.ReloadAnimationDuration);
            GUILayout.Label("Auto reloading");
            weapon.AutoReload = EditorGUILayout.Toggle(weapon.AutoReload);
            GUILayout.Label("Current ammo value");
            weapon.CurrentAmmo = EditorGUILayout.IntField(weapon.CurrentAmmo);
            GUILayout.Label("Ammo Clip Size");
            weapon.AmmoClipSize = EditorGUILayout.IntField(weapon.AmmoClipSize);
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Helpers", EditorStyles.centeredGreyMiniLabel);

            GUILayout.Label("Muzzle flash transform");
            weapon.MuzzleTransform = (Transform)EditorGUILayout.ObjectField(weapon.MuzzleTransform, typeof(Transform), true);
            GUILayout.Label("Bullet spawn transform");
            weapon.BulletTransform = (Transform)EditorGUILayout.ObjectField(weapon.BulletTransform, typeof(Transform), true);
            EditorGUILayout.HelpBox("Z axis is forward axis of each transform. Check forward for each helper to set right direction!", MessageType.Warning);
            GUILayout.Label("Aim Position");
            weapon.AimPos = EditorGUILayout.Vector3Field("Aim Pos", weapon.AimPos);
            GUILayout.Label("Aim Sensitivity");
            weapon.AimSensitivity = EditorGUILayout.Vector2Field("Values X And Y", weapon.AimSensitivity);
            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }
        public void DrawCrossbow()
        {
            GUILayout.Label("Firearms settings", EditorStyles.boldLabel);
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Ammo Item ID");
            weapon.AmmoItemID = EditorGUILayout.IntField(weapon.AmmoItemID);
            GUILayout.Label("Reload animation duration");
            weapon.ReloadAnimationDuration = EditorGUILayout.FloatField(weapon.ReloadAnimationDuration);
            GUILayout.Label("Auto reloading");
            weapon.AutoReload = EditorGUILayout.Toggle(weapon.AutoReload);
            GUILayout.Label("Current ammo value");
            weapon.CurrentAmmo = EditorGUILayout.IntField(weapon.CurrentAmmo);
            GUILayout.Label("Ammo Clip Size");
            weapon.AmmoClipSize = EditorGUILayout.IntField(weapon.AmmoClipSize);
            GUILayout.Label("Arrow Reference");
            weapon.Arrow = (Transform) EditorGUILayout.ObjectField(weapon.Arrow, typeof(Transform), true);
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Helpers", EditorStyles.centeredGreyMiniLabel);
            GUILayout.Label("Bullet spawn transform");
            weapon.BulletTransform = (Transform)EditorGUILayout.ObjectField(weapon.BulletTransform, typeof(Transform), true);
            GUILayout.Label("Aim Position");
            weapon.AimPos = EditorGUILayout.Vector3Field("Aim Pos", weapon.AimPos);
            GUILayout.Label("Aim Sensitivity");
            weapon.AimSensitivity = EditorGUILayout.Vector2Field("Values X And Y", weapon.AimSensitivity);
            EditorGUILayout.HelpBox("Z axis is forward axis of each transform. Check forward for each helper to set right direction!", MessageType.Warning);
            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }

        public void DrawGeneral()
        {
            GUILayout.Label("General settings", EditorStyles.boldLabel);
            GUILayout.BeginVertical("HelpBox");
            weapon.WeaponSetting = (WeaponSetting)EditorGUILayout.ObjectField(weapon.WeaponSetting, typeof(WeaponSetting), false);
            GUILayout.Label("Weapon name");
            weapon.WeaponName = GUILayout.TextField(weapon.WeaponName);
            GUILayout.Label("Has Empty Animation");
            weapon.HasEmptyAnim = EditorGUILayout.Toggle(weapon.HasEmptyAnim);
            GUILayout.EndVertical();
        }
        public void DrawMelee()
        {
            EditorGUILayout.HelpBox("There is no settings for melee weapon type. See weapon settings", MessageType.Info);
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Ammo Item ID");
            weapon.AmmoItemID = EditorGUILayout.IntField(weapon.AmmoItemID);
            GUILayout.EndVertical();
        }
        public void DrawFirearms()
        {
            GUILayout.Label("Firearms settings", EditorStyles.boldLabel);
            GUILayout.BeginVertical("HelpBox");
            GUILayout.Label("Ammo Item ID");
            weapon.AmmoItemID = EditorGUILayout.IntField(weapon.AmmoItemID);
            GUILayout.Label("Reload animation duration");
            weapon.ReloadAnimationDuration = EditorGUILayout.FloatField(weapon.ReloadAnimationDuration);
            GUILayout.Label("Auto reloading");
            weapon.AutoReload = EditorGUILayout.Toggle(weapon.AutoReload);
            GUILayout.Label("Current ammo value");
            weapon.CurrentAmmo = EditorGUILayout.IntField(weapon.CurrentAmmo);
            GUILayout.Label("Ammo Clip Size");
            weapon.AmmoClipSize = EditorGUILayout.IntField(weapon.AmmoClipSize);
            GUILayout.BeginVertical("GroupBox");
            GUILayout.Label("Helpers", EditorStyles.centeredGreyMiniLabel);
            GUILayout.Label("Muzzle flash transform");
            weapon.MuzzleTransform = (Transform)EditorGUILayout.ObjectField(weapon.MuzzleTransform, typeof(Transform), true);
            GUILayout.Label("Shell eject transform");
            weapon.ShellTransform = (Transform)EditorGUILayout.ObjectField(weapon.ShellTransform, typeof(Transform), true);
            GUILayout.Label("Bullet spawn transform in case of aimimg");
            weapon.BulletTransform = (Transform)EditorGUILayout.ObjectField(weapon.BulletTransform, typeof(Transform), true);
            GUILayout.Label("Aim Position");
            weapon.AimPos = EditorGUILayout.Vector3Field("Aim Pos", weapon.AimPos);
            GUILayout.Label("Aim Sensitivity");
            weapon.AimSensitivity = EditorGUILayout.Vector2Field("Values X And Y", weapon.AimSensitivity);

            EditorGUILayout.HelpBox("Z axis is forward axis of each transform. Check forward for each helper to set right direction!", MessageType.Warning);
            GUILayout.EndVertical();
            GUILayout.EndVertical();
        }
        public void DrawGrenade()
        {
            GUILayout.Label("Ammo Item ID");
            weapon.AmmoItemID = EditorGUILayout.IntField(weapon.AmmoItemID);
            GUILayout.Label("Grenade position throw");
            weapon.GrenadeThrowTransform = (Transform)EditorGUILayout.ObjectField(weapon.GrenadeThrowTransform, typeof(Transform), true);
        }
    }
}
