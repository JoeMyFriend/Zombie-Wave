using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Animations.Rigging;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAim : MonoBehaviour
{
    private GameManager GM;
    public float turnSpeed = 15f;
    private Camera mainCamera;
    public float mouseSensitivity;
    private float xRotation;
    public float xMinRot, xMaxRot;

    public Transform weaponPos, teste;

    public CinemachineVirtualCamera vcam;

    [Header("Rigs Control")]
    public Rig rigLayerRunning;
    private CharacterLocomotion Movimentation;
    public float timeRunning;

    private RaycastWeapon gun;

    public Animator weaponAnim; // teste
    //public Animator cubeAnim; // teste de novo

    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType(typeof(GameManager)) as GameManager;
        Movimentation = FindObjectOfType(typeof(CharacterLocomotion)) as CharacterLocomotion;
        gun = FindObjectOfType(typeof(RaycastWeapon)) as RaycastWeapon;
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GM.bulletTxt.text = gun.ammunition.ToString();
        GM.magazineTxt.text = gun.magazine.ToString();
    }

    void Update()
    {
        if (Movimentation.isRunning)
        {
            rigLayerRunning.weight += Time.deltaTime / timeRunning;
        }
        else
        {
            rigLayerRunning.weight -= Time.deltaTime / timeRunning;
        }

        if (Input.GetButtonDown("Fire1") && Movimentation.movementSpeed <= 0 && !gun.isReloading)
        {
            if (gun.ammunition > 0)
            {
                weaponAnim.SetTrigger("Shot");
                gun.StartFire();
                GM.bulletTxt.text = gun.ammunition.ToString();
            }
            if (gun.ammunition <= 0 && gun.magazine > 0)
            {
                StartCoroutine("Reload");
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && gun.ammunition < gun.ammunitionBase && !gun.isReloading && gun.magazine > 0)
        {
            print("Recarregando no R");
            StartCoroutine("Reload");
        }

        weaponAnim.SetBool("isReloading", gun.isReloading);

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);

        //xRotation -= yawCamera;
        //transform.rotation = Quaternion.Euler(0f, yawCamera, 0f);
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;


        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, xMinRot, xMaxRot);

        vcam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TakeDamage")
        {
            gameObject.SendMessage("GetDamage", GM.ZombieDamage, SendMessageOptions.DontRequireReceiver);

            
        }
    }
    IEnumerator Reload()
    {
        gun.isReloading = true;
        yield return new WaitForSeconds(gun.timeToReload);
        if (gun.magazine > 0)
        {
            gun.Reload();
            GM.magazineTxt.text = gun.magazine.ToString(); // mudar o texto dps que recarregar
            GM.bulletTxt.text = gun.ammunition.ToString();
        }
    }
}
