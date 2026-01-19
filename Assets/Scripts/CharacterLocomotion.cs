using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{

    private Animator playerAnim;
    private Vector2 input;

    [HideInInspector]
    public bool isRunning;

    [HideInInspector]
    public float movementSpeed = 1f;

    public float incrementSpeed = 0.05f;

    [Header("Weapon")]
    private RaycastWeapon gun;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        gun = FindObjectOfType(typeof(RaycastWeapon)) as RaycastWeapon;
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;  
        }
        // nao corre quando solta o botao, quando para de pressionar pra frente ou pra tras ou quando está recarregando
        if (Input.GetKeyUp(KeyCode.LeftShift) || input.y < 0.1f || gun.isReloading) 
        {
            isRunning = false;
        }

        if (isRunning && movementSpeed < 1)
        {
            movementSpeed += incrementSpeed * Time.deltaTime;
        }
        else if (!isRunning && movementSpeed > 0)
        {
            movementSpeed -= incrementSpeed * Time.deltaTime;
        }


        playerAnim.SetFloat("InputX", input.x);
        playerAnim.SetFloat("InputY", input.y);
        playerAnim.SetFloat("MovementSpeed", movementSpeed);
        
    }
}
