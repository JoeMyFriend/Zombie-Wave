using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private CharacterController controller;
    public float speed;

    [Header("Jump and Gravity")]
    [SerializeField] private Vector3 velocity;
    private float gravity;
    public float gravityScale;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    private bool isGrounded;
    public float jumpForce;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        gravity = Physics.gravity.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * x + transform.forward * z);
        

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
        }

        velocity.y += gravity * gravityScale * Time.deltaTime;
        
        controller.Move(velocity * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, whatIsGround);
    }
}
