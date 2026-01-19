using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum enemyState
{ 
    IDLE, PATROL, ATTACK, ALERT, EXPLORE, SEARCH, DIE
}
public enum characterType
{
    PLAYER, ENEMY, CAR
}

public class GameManager : MonoBehaviour
{
    [Header("HUD")]
    public Text bulletTxt;
    public Text magazineTxt;
    public TextMeshProUGUI timerTxt;
    public GameObject canvasPlayer;
    public GameObject canvasCar;
    public GameObject canvasRespawn;
    //public Image getHitImage;

    [Header("Enemy")]
    public float idleWaitTime;
    public int percPatrol;
    public float distanceToAttack;
    public float attackDelay;
    public float rotationSpeed;
    public float searchWaitTime;
    public int ZombieDamage;
    public float dangerAttackTime;

    [Header("Car")]
    public Image hpBarCar;

    [Header("Sounds")]
    public AudioClip pistolFire;
    public AudioClip zombieAttack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region

    public bool RandomSystem(int perc)
    {
        int rand = Random.Range(0, 100);
        bool retorno = rand <= perc;
        return retorno;
    }


    #endregion
}
