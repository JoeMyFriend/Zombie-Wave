using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairTarget : MonoBehaviour
{
    Camera camera;
    Ray ray;
    RaycastHit hitInfo;
    public Transform crossHairTargetEmpty;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ray.origin = camera.transform.position;
        ray.direction = camera.transform.forward;
        if(Physics.Raycast(ray, out hitInfo))
        {
            transform.position = hitInfo.point;
        }
        else
        {
            transform.position = crossHairTargetEmpty.position;
        }
        
    }
}
