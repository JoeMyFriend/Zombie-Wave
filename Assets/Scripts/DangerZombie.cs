using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZombie : MonoBehaviour
{
    private SpawnEnemies spawnEnemies;
    private GameManager _GM;
    //public bool isDangerMode;
    public GameObject zombie;
    private EnemyIA AIManager;

    // Start is called before the first frame update
    void Start()
    {
        _GM = FindObjectOfType(typeof(GameManager)) as GameManager;
        spawnEnemies = FindObjectOfType(typeof(SpawnEnemies)) as SpawnEnemies;
        AIManager = FindObjectOfType(typeof(EnemyIA)) as EnemyIA;
        //Zombies = spawnEnemies.zombieParent.GetComponentsInChildren<GameObject>();

    }

    // Update is called once per frame
    void Update()
    {
        EnterDangerMode();

        /*
        for (int i = 0; i < spawnEnemies.zombieParent.childCount; i++)
        {
            //print(spawnEnemies.zombieParent.GetChild(i).transform.position);
        }
        */
    }

    public void EnterDangerMode()
    {
        StartCoroutine("EnterDangerModeTimer");

        //gameObject.SendMessage("GetShot", SendMessageOptions.RequireReceiver);
    }

    
    IEnumerator EnterDangerModeTimer()
    {
        yield return new WaitForSeconds(_GM.dangerAttackTime);

        if (spawnEnemies.isSpawnZombie) // entrar aqui só quando tiver spawnado os zombies
        {
            // nao precisa do for nessa parte
            //for (int i = 0; i < spawnEnemies.zombieParent.childCount; i++)
            //{
                //spawnEnemies.zombieParent.GetChild(0).SendMessage("GetShot", SendMessageOptions.DontRequireReceiver);
                gameObject.SendMessage("GetShot", SendMessageOptions.DontRequireReceiver);
                print("Quantas vezes veio no fela");
                yield return new WaitForEndOfFrame(); // esperar um pouco pra executar pra ir pra todos os zombies
            //}
            spawnEnemies.isSpawnZombie = false;
        }
    }
    
}
