using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AprendaUnity
{
    [CustomEditor(typeof(RaycastWeapon))]
    public class AudioListenEnemyEditor : Editor
    {
        private void OnSceneGUI()
        {
            RaycastWeapon wep = (RaycastWeapon)target;
            Handles.color = Color.white;
            Handles.DrawWireArc(wep.transform.position, Vector3.up, Vector3.forward, 360, wep.noiseRadius);

        }
    }
}


