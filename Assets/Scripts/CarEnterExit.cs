using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CarEnterExit : MonoBehaviour
{
    private GameManager _GameManager;
    public CarController carController;
    private CinemachineVirtualCamera carCamera;
    private GameObject player;
    public Transform playerManagerParent;

    public bool isDriving;

    private void Awake()
    {
        carCamera = GetComponentInChildren<CinemachineVirtualCamera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        //carController = FindObjectOfType(typeof(CarController)) as CarController;
        
        carCamera.Priority = 4;
        player = GameObject.FindGameObjectWithTag("Player");

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isDriving) // sair do carro
        {
            ExitCar();

        }
    }

    private void EnterCar() // é chamado la pelo Interaction
    {
        StartCoroutine("isDrivingOn");
    }

    public void ExitCar()
    {
        StartCoroutine("isDrivingOff");
    }

    IEnumerator isDrivingOn()
    {
        yield return new WaitForEndOfFrame();

        isDriving = true;
        player.SetActive(false);
        carCamera.Priority = 6;
        carController.isBrake = false; // como se voltasse o freio de mao
        player.transform.SetParent(carController.transform);


        _GameManager.canvasCar.SetActive(true);
        _GameManager.canvasPlayer.SetActive(false);

        // recarregar a quantidade de vida que o carro tem na HUD
        gameObject.SendMessage("UpdateHPBarCar", SendMessageOptions.DontRequireReceiver); 

    }
    
    IEnumerator isDrivingOff()
    {
        yield return new WaitForEndOfFrame();

        isDriving = false;
        player.SetActive(true);
        carCamera.Priority = 4;
        player.transform.SetParent(playerManagerParent);
        player.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

        _GameManager.canvasPlayer.SetActive(true);
        _GameManager.canvasCar.SetActive(false);

        //carController.torque = 0f;
        //carController.enabled = false;
    }

}
