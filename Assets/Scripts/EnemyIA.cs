using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AprendaUnity;

public class EnemyIA : MonoBehaviour
{
    public enemyState currentState;
    private NavMeshAgent agent;
    private GameManager _GameManager;
    private AudioSource enemyASource;
    public RagdollController ragdoll;
    private Animator enemyAnim;

    private Transform target;

    private bool isAttack = true;

    private bool isSearch;

    public Transform[] wayPoints;
    private int idWayPoint;




    // Start is called before the first frame update
    void Start()
    {
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
        enemyASource = GetComponent<AudioSource>();
        //ragdoll = FindObjectOfType(typeof(RagdollController)) as RagdollController;
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<Animator>();

        OnStateEnter(enemyState.IDLE);

        target = GameObject.FindGameObjectWithTag("Player").transform;

        //agent.updatePosition = false; //parte do animator move
        //agent.updateRotation = true;

    }

    // Update is called once per frame
    void Update()
    {
        
        UpdateEnemyState();

        /*
         //calcular para o AnimatorMove
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        //print("world delta " + worldDeltaPosition.magnitude);

        // Pull agent towards character
        if (worldDeltaPosition.magnitude > agent.radius) 
        {
            agent.nextPosition = transform.position + 0.9f * worldDeltaPosition;
        }
        */

    }

    /* //andar de acordo com a animação
     void OnAnimatorMove()
     {
         Vector3 position = enemyAnim.rootPosition;
         position.y = agent.nextPosition.y;
         transform.position = position;

     }
    */

    #region MEUS MÉTODOS

    private void SetDestinationAgent(Vector3 destination)
    {
        agent.destination = destination;
    }

    void OnStateEnter(enemyState newEnemyState)
    {
        StopAllCoroutines();
        currentState = newEnemyState;

        switch (currentState)
        {
            case enemyState.IDLE:
                StartCoroutine("Idle");
                //print("Entrou em Idle");
                break;

            case enemyState.PATROL:
                //print("Entrou em PATROL");

                agent.stoppingDistance = 0.3f;
                SetDestinationAgent(wayPoints[idWayPoint].position);

                break;

            case enemyState.ATTACK:
                //print("Entrou no ataque, visto na visão primaria");
                agent.stoppingDistance = _GameManager.distanceToAttack;
                StartCoroutine("AttackTimer");

                break;

            case enemyState.ALERT:
                //print("Entrou no alerta, visto na visão secundaria");
                SetDestinationAgent(transform.position);


                break;

            case enemyState.EXPLORE:

                break;

            case enemyState.SEARCH:

                StartCoroutine("Search");

                break;

            case enemyState.DIE:
                enemyAnim.enabled = false;
                ragdoll.SetRigidBodyState(false);
                ragdoll.SetColliderState(true);
                //gameObject.SendMessage("SetRigidBodyState", false, SendMessageOptions.DontRequireReceiver);

                //gameObject.SendMessage("SetColliderState", true, SendMessageOptions.DontRequireReceiver);
                StopAllCoroutines();
                agent.enabled = false;

                enemyASource.Stop();

                Destroy(transform.parent.gameObject, 4f);

                break;
        }
    }

    void UpdateEnemyState()
    {
        if (agent.desiredVelocity.magnitude != 0)
        {
            enemyAnim.SetInteger("idAnimation", 1);
        }
        else
        {
            enemyAnim.SetInteger("idAnimation", 0);
        }

        switch (currentState)
        {
            case enemyState.IDLE:

                break;

            case enemyState.PATROL:

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    idWayPoint = (idWayPoint + 1) % wayPoints.Length;
                    agent.SetDestination(wayPoints[idWayPoint].position);
                }

                break;

            case enemyState.ATTACK:



                float distanceToPlayer = Vector3.Distance(target.position, transform.position);

                SetDestinationAgent(target.position);

                //if(agent.remainingDistance <= agent.stoppingDistance)

                if (distanceToPlayer <= agent.stoppingDistance)
                {
                    Attack();
                }

                break;

            case enemyState.ALERT:

                LookAtTarget();

                break;

            case enemyState.EXPLORE:

                break;

            case enemyState.SEARCH:

                //LookAtTarget();

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    OnStateEnter(enemyState.PATROL);
                }


                break;
        }
    }

    private void IsVisible(viewState vState)
    {
        if (currentState == enemyState.ATTACK || currentState == enemyState.DIE) { return; }
        switch (vState)
        {
            case viewState.Primary:
                OnStateEnter(enemyState.ATTACK);
                break;

            case viewState.Secondary:
                OnStateEnter(enemyState.ALERT);
                break;
        }
    }

    void Attack()
    {
        if (!isAttack)
        {
            isAttack = true;
            enemyAnim.SetTrigger("attack");
            enemyASource.Stop();
        }
    }

    void AttackSound()
    {
        enemyASource.PlayOneShot(_GameManager.zombieAttack);
    }

    void AttackIsDone()
    {
        StartCoroutine("AttackTimer");
    }

    private void LookAtTarget()
    {
        Quaternion rotTarget = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, _GameManager.rotationSpeed * Time.deltaTime);
    }

    void GetShot()
    {
        OnStateEnter(enemyState.ATTACK);
    }

    void IsDie()
    {
        OnStateEnter(enemyState.DIE);
    }

    void ListenShot()
    {
        if (currentState != enemyState.SEARCH && currentState != enemyState.ATTACK)
        {
            OnStateEnter(enemyState.SEARCH);
        }
        else if (currentState == enemyState.SEARCH)
        {
            agent.SetDestination(target.position);
        }

    }

    IEnumerator Idle()
    {
        SetDestinationAgent(transform.position);
        yield return new WaitForSeconds(_GameManager.idleWaitTime);

        if (_GameManager.RandomSystem(_GameManager.percPatrol))
        {
            OnStateEnter(enemyState.PATROL);
        }
        else
        {
            OnStateEnter(enemyState.IDLE);
        }

    }

    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(_GameManager.attackDelay);
        isAttack = false;
    }

    IEnumerator Search()
    {
        isSearch = true;

        yield return new WaitForSeconds(_GameManager.searchWaitTime);
        SetDestinationAgent(target.position);
    }

    #endregion
}