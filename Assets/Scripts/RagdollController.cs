using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetRigidBodyState(true);
        SetColliderState(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRigidBodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

        GetComponent<Rigidbody>().isKinematic = !state;
    }

    public void SetColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = state;

            
            if (collider.tag == "TakeDamage") // essa linha é para nao ligar o colisor do ataque quando o zumbi morrer
            {
                collider.enabled = false;
            }
        }

        GetComponent<Collider>().enabled = !state;

    }
}
