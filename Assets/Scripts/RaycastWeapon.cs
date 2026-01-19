using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{

    GameManager _GameManager;

    Ray ray;
    RaycastHit hitInfo;

    public Transform raycastOrigin;
    public Transform raycastDestination;

    public ParticleSystem muzzleFlash;
    public ParticleSystem[] hitFX;

    [Header("Munição")]

    public int ammunitionBase;
    //[HideInInspector]
    public int ammunition;
    public int magazine;
    public float timeToReload;
    public bool isReloading;

    public float noiseRadius;
    public LayerMask targetMask;
    private int idLayerMask;

    public Collider[] targetInRadius;

    // SOM
    private AudioSource aSource;
    


    private void Start()
    {
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        aSource = GetComponent<AudioSource>();
        ammunition = ammunitionBase;
    }

    public void StartFire()
    {
        ammunition--;
        ray.origin = raycastOrigin.position;
        ray.direction = raycastDestination.position - raycastOrigin.position;

        muzzleFlash.Emit(1);

        NoiseWeapon();


        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {

            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                idLayerMask = 1;
            }
            else if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Wood"))
            {
                idLayerMask = 2;
            }
            else 
            {
                idLayerMask = 0;
            }

            hitFX[idLayerMask].transform.position = hitInfo.point;
            hitFX[idLayerMask].transform.forward = hitInfo.normal;
            hitFX[idLayerMask].Emit(1);

            hitInfo.collider.gameObject.SendMessageUpwards("GetDamage", 1, SendMessageOptions.DontRequireReceiver);
        } 
       
    }

    public void Reload()
    {
        magazine--;
        ammunition = ammunitionBase;
        isReloading = false;
    }

    void NoiseWeapon()
    {
        aSource.PlayOneShot(_GameManager.pistolFire);
        targetInRadius = Physics.OverlapSphere(transform.position, noiseRadius, targetMask);

        for (int i = 0; i < targetInRadius.Length; i++)
        {
            targetInRadius[i].gameObject.SendMessage("ListenShot", SendMessageOptions.DontRequireReceiver);
        }
    }

}
