using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairTarget : MonoBehaviour
{
    Camera cam;
    Ray ray;
    RaycastHit hitInfo;
    public Transform crossHairTargetEmpty;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ray.origin = cam.transform.position;
        ray.direction = cam.transform.forward;
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
