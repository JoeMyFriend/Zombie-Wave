using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CarCamera : MonoBehaviour
{

    private Transform rootNode;
    [SerializeField]
    private Transform car;
    private Rigidbody rbCar;

    public float rotationThreshold = 1f;
    public float cameraStickness = 10f;
    public float cameraRotationSpeed = 5f;


    private void Awake()
    {
        rootNode = this.transform;
        rbCar = rootNode.parent.GetComponent<Rigidbody>();
        car = rootNode.parent.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        rootNode.parent = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion lookAt;

        rootNode.position = Vector3.Lerp(rootNode.position, car.position, cameraStickness * Time.fixedDeltaTime);

        if (rbCar.linearVelocity.magnitude < rotationThreshold)
        {
            lookAt = Quaternion.LookRotation(car.forward);
        }
        else
        {
            lookAt = Quaternion.LookRotation(rbCar.linearVelocity.normalized);
        }

        lookAt = Quaternion.Slerp(rootNode.rotation, lookAt, cameraRotationSpeed * Time.fixedDeltaTime);

        rootNode.rotation = lookAt;

    }
}
