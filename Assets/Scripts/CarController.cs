using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum Axel
{
    Front, Rear
}

[Serializable]
public struct Wheel
{
    public GameObject model;
    public WheelCollider wCollider;
    public Axel axel;
}

public class CarController : MonoBehaviour
{

    public Wheel[] wheels;

    private float torque;
    public float maxTorque;
    public float brakeTorque;
    public bool isBrake;
    public float maxSteerAngle = 30f;
    public float turnSensitivity = 1f;
    private float KMPH;
    public bool isExplode;

    private Rigidbody rbCar;
    public Vector2 input;

    private GameObject player;

    public CarEnterExit _CarEnterExit;

    // Start is called before the first frame update
    void Start()
    {
        rbCar = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");
        //_CarEnterExit = FindObjectOfType(typeof(CarEnterExit)) as CarEnterExit;

    }

    // Update is called once per frame
    void Update()
    {
        if (_CarEnterExit.isDriving && !isExplode) // caso esteja no modo dirigir
        {
            GetInput();
        }
        else
        {
            isBrake = true; // freio de mao puxado
        }
       

    }

    private void FixedUpdate()
    {

        SetTorque();
        SetBrake();
        Turn();
        AnimateWheels();
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (KMPH > 10f) // se velocidade acima de 10KM atropela o zumbi
            {
                collision.gameObject.SendMessage("IsDie", SendMessageOptions.DontRequireReceiver);
                gameObject.SendMessage("GetDamage", 5, SendMessageOptions.DontRequireReceiver);
            }
            
        }
    }


    #region MEUS M�TODOS
    private void GetInput()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isBrake = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isBrake = false;
        }
    }

    private void SetTorque()
    {

        KMPH = rbCar.linearVelocity.magnitude * 3.6f; // 3.6 � para converter para km/h
        //print(KMPH.ToString("N0") + "KM/H");

        torque = maxTorque / 4;

        foreach (Wheel w in wheels)
        {
            w.wCollider.motorTorque = input.y * torque;
        }
    }

    private void Turn()
    {
        foreach (Wheel w in wheels)
        {
            if (w.axel == Axel.Front)
            {
                float steerAngle = input.x * turnSensitivity * maxSteerAngle;
                w.wCollider.steerAngle = steerAngle;
            }
        }
    }

    private void SetBrake()
    {
        foreach (Wheel w in wheels)
        {
            if (isBrake)
            {
                w.wCollider.motorTorque = 0;
                w.wCollider.brakeTorque = brakeTorque;
            }
            else
            {
                w.wCollider.brakeTorque = 0;
            }
        }
    }
    private void AnimateWheels()
    {
        foreach (Wheel w in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            w.wCollider.GetWorldPose(out pos, out rot);

            w.model.transform.position = pos;
            w.model.transform.rotation = rot;

        }
    }

    #endregion
}
