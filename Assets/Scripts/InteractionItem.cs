using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionItem : MonoBehaviour
{

    private RaycastWeapon gun;
    private GameManager GM;
    private Animator anim;
    private bool isOpen;

    private void Start()
    {
        gun = FindObjectOfType(typeof(RaycastWeapon)) as RaycastWeapon;
        GM = FindObjectOfType(typeof(GameManager)) as GameManager;
        anim = GetComponent<Animator>();
    }

    void InteractionBullet()
    {
        gun.magazine += 1;
        GM.magazineTxt.text = gun.magazine.ToString();
        Destroy(gameObject, 0.3f);
    }
    void OpenCloseDoor()
    {
        //anim.SetTrigger("Open");

        anim.SetBool("isOpen", isOpen);
        isOpen = !isOpen;

    }
}
