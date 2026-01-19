using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    private GameManager _GameManager;
    public Animator WeaponGripAnim;

    private Transform mainCamera;

    private Ray ray;
    private RaycastHit hitInfo;

    public float rayRange;
    public LayerMask interactionLayer;

    [SerializeField]
    private GameObject interactionGameObj;

    // Start is called before the first frame update
    void Start()
    {
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        mainCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {

        ray.origin = mainCamera.position;
        ray.direction = mainCamera.forward;


        if(Physics.Raycast(ray, out hitInfo, rayRange, interactionLayer))
        {
            interactionGameObj = hitInfo.collider.gameObject;
        }
        else 
        {
            interactionGameObj = null;
        }

        if (interactionGameObj != null)
        {
            if (Input.GetKeyDown(KeyCode.E) && interactionGameObj.layer == LayerMask.NameToLayer("Item"))
            {
                interactionGameObj.SendMessage("InteractionBullet", SendMessageOptions.DontRequireReceiver);
                WeaponGripAnim.SetTrigger("Item");
            }

            if (Input.GetKeyDown(KeyCode.E) && interactionGameObj.layer == LayerMask.NameToLayer("Vehicle"))
            {
                interactionGameObj.SendMessage("EnterCar", SendMessageOptions.DontRequireReceiver);
            }

            if (Input.GetKeyDown(KeyCode.E) && interactionGameObj.layer == LayerMask.NameToLayer("Door"))
            {
                interactionGameObj.SendMessageUpwards("OpenCloseDoor", SendMessageOptions.DontRequireReceiver);
            }
        }

    }
}
