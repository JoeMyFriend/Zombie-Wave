using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    private GameManager _GameManager;
    public Transform[] spawnWayPoints;
    public Transform[] spawnBulletsPoints;
    private List<int> spawnUtilizado = new List<int>();
    private List<int> spawnResourceUtilizado = new List<int>();
    private int idWayPoint;

    public int zombiesPerLevel;
    public int bulletsPerLevel;

    public GameObject zombie;
    public Transform zombieParent;
    public bool isSpawnZombie;
    private bool isAllZombiesDead;

    public GameObject bullet;
    public Transform bulletParent;

    [Header("Timer")]
    public float timeToRespawn;
    public float timeRemaing;
    public bool timerIsRunning = true;

    // Start is called before the first frame update
    void Start()
    {
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

        timeRemaing = timeToRespawn;
    }

    // Update is called once per frame
    void Update()
    {
        VerifySpawnZombie();

        Timer();

    }

    void VerifySpawnZombie()
    {
        //print(zombieParent.childCount);


        if (zombieParent.childCount <= 0)
        {

            if (isAllZombiesDead) // executar uma vez só
            {
                isAllZombiesDead = false;
                _GameManager.canvasRespawn.SetActive(true);
                timerIsRunning = true;
                timeRemaing = timeToRespawn;
            }

            if (Input.GetKeyDown(KeyCode.Space) || !timerIsRunning) // fica verificando
            {
                //_GameManager.dangerAtkRemaing = _GameManager.dangerAttackTime;
                StartCoroutine("SpawnCorroutine");
                
                isSpawnZombie = true;
                isAllZombiesDead = true; // quando n tiver zombie vivo vai entrar na condição para executar uma vez só
            }
        }


        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnZombies();
        }
        */

    }


    void SpawnZombies()
    {

        spawnUtilizado.Clear();

        if (zombiesPerLevel > spawnWayPoints.Length)
        {
            zombiesPerLevel = spawnWayPoints.Length;
        }

        for (int i = 0; i < zombiesPerLevel; i++)
        {


            idWayPoint = Random.Range(0, spawnWayPoints.Length);
            if (!spawnUtilizado.Contains(idWayPoint)) // ver se já foi utilizado aquele waypoint para nao spawnar no mesmo lugar
            {
                spawnUtilizado.Add(idWayPoint);
                Instantiate(zombie, spawnWayPoints[idWayPoint].position, Quaternion.identity, zombieParent);
            }
            else
            {
                i--;
            }

             //print("Executou quantas vezes");
            
        }
        // retornar quantos objetos tenho dentro do pai zombieParent que é onde está os zombies instanciados
        isSpawnZombie = true;
    }

    void SpawnResources()
    {
        spawnResourceUtilizado.Clear();
        ClearResources();

        if (zombiesPerLevel > spawnWayPoints.Length)
        {
            zombiesPerLevel = spawnWayPoints.Length;
        }

        for (int i = 0; i < bulletsPerLevel; i++)
        {


            idWayPoint = Random.Range(0, spawnBulletsPoints.Length);
            if (!spawnResourceUtilizado.Contains(idWayPoint)) // ver se já foi utilizado aquele waypoint para nao spawnar no mesmo lugar
            {
                spawnResourceUtilizado.Add(idWayPoint);
                Instantiate(bullet, spawnBulletsPoints[idWayPoint].position, Quaternion.identity, bulletParent);
            }
            else
            {
                i--;
            }

            //print("Executou quantas vezes");

        }

        void ClearResources()
        {
            foreach (Transform child in bulletParent.transform)
            {
                Destroy(child.gameObject);
            }
        }

    }

    IEnumerator SpawnCorroutine()
    {
        yield return new WaitForEndOfFrame();
        SpawnZombies();
        SpawnResources();
        _GameManager.canvasRespawn.SetActive(false);
        
    }

    void Timer()
    {
        if (timerIsRunning) {
            if (timeRemaing > 0)
            {
                timeRemaing -= Time.deltaTime;
                //print(Mathf.FloorToInt(timeRemaing % 15));
            }
            else
            {
                timerIsRunning = false;
                timeRemaing = 0;
            }
        }

        DisplayTime(timeRemaing);

    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        _GameManager.timerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
