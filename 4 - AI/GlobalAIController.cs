using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GlobalAIController : MonoBehaviour
{

    public AudioSource specialAudioClip;
    public AudioSource attackAudioClip;
    public AudioSource hurtAudioClip;
    public AudioSource deadAudioClip;


    private SpawnAnimals spawnAnimals;
    private NavMeshAgent agent;
    private Player playerScript;
    private Transform playerPosition;

    private bool isChasing = false;
    private bool isAttacking = false;
    private bool isRoaming = false;
    private bool isRunning = false;
    private bool isDead = false;

    public string aiName;
    public bool willRun;
    public bool willAttack;
    public bool willGetHurt;
    public bool willGetBlocked;
    public bool hasStamina;
    public bool hasSpecial;

    public float maxStamina;
    public float stamina;
    public int health;
    public float viewRange;
    public float rotationSpeed;
    public float speed = 2f;

    private float maxHealth;
    private int mainMaxHealth;

    [Header("Attacking")]
    public float attackAnimLength;
    public int damage;
    public float distanceToChase;
    public float distanceToAttack;

    [Header("Run Away")]
    public float multiplyBy;
    public float distanceToRun;


    private float roamTimeMax = 20;
    private float randomCircleRadius = 20;
    private float roamTimeMin = 10;

    private float roamTimerStart;

    private Animator anim;
    private bool once = true;
    private bool check = true;
    private bool once1 = true;
    public bool blocked;

    private float distance;


    private string idleState = "Idle";

    private string specialState = "Special";
    private string runState = "Run";
    private string walkState = "Walk";
    private string deadState = "Dead";
    private string hurtState = "Hurt";
    private string endRestState = "EndRest";
    private string attackState = "Attack";
    private string blockedState = "Blocked";

    private string currentState;
    private CurrentState state;
    public AnimalType animalType;
    private float healthStillUnchanged = 0;

    public static GlobalAIController instance;

    private void Awake()
    {
        instance = this;
    }

    private enum CurrentState 
    { 
        Running, 
        Walking, 
        Attacking, 
        Dead, 
        Idle,
        Hurt, 
        Blocked,
        Tired,
        Awake,
        Special
    }

    public enum AnimalType
    {
        Aggresive,
        Neutral,
        Passive
    }

    void Start()
    {
        spawnAnimals = FindObjectOfType<SpawnAnimals>();
        stamina = maxStamina;
        agent = GetComponent<NavMeshAgent>();
        playerScript = FindObjectOfType<Player>();
        roamTimerStart = Random.Range(roamTimeMin, roamTimeMax);
        playerPosition = FindObjectOfType<Player>().transform;
        anim = GetComponent<Animator>();
        maxHealth = health;
        mainMaxHealth = (int)maxHealth;
        state = CurrentState.Idle;
    }

    void Update()
    {
        if (gameObject.activeInHierarchy && LoadingScreen.done)
        {
            distance = Vector3.Distance(transform.position, playerScript.transform.position);
            if(distance < 150)
            {
                AnimationStates();
                StateCheck();
            }

            if (maxHealth != health)
            {
                healthStillUnchanged += Time.deltaTime;
                if (healthStillUnchanged >= 10)
                {
                    maxHealth = health;
                    healthStillUnchanged = 0;
                }
            }

            #region Aggresive
            if (animalType == AnimalType.Aggresive && health > 0)
            {
                //Chase Params
                if (distance < distanceToChase && distance >= distanceToAttack || health > 0 && maxHealth != health)
                {
                    isChasing = true;
                }
                else
                {
                    isChasing = false;
                }
                //Attack Params
                if (distance < distanceToAttack && !isChasing)
                {
                    isAttacking = true;
                }
                else
                {
                    isAttacking = false;
                }
            }
            #endregion

            #region Neutral
            if (animalType == AnimalType.Neutral && health > 0)
            {
                if (maxHealth != health && health > 0)
                {
                    isChasing = true;
                }
                else
                {
                    isChasing = false;
                }
                if (maxHealth != health && distance < distanceToAttack)
                {
                    isAttacking = true;
                }
                else
                {
                    isAttacking = false;
                }
            }
            #endregion

            #region Passive
            if (animalType == AnimalType.Passive && health > 0)
            {
                //Run Params
                if (distance <= distanceToRun || maxHealth != health)
                {
                    isRunning = true;
                }
                else
                {
                    isRunning = false;
                }
            }
            #endregion

            #region Basic
            //Roam Params
            if (!isAttacking && !isChasing && !isRunning && health > 0)
            {
                isRoaming = true;
            }
            else
            {
                isRoaming = false;
            }
            //Dead Params
            if (health <= 0)
            {
                isDead = true;
                health = 0;
            }
            #endregion
        }
    }

    private void StateCheck()
    {
        if(!isDead)
        {
            #region Attacking Logic
            if (isAttacking)
            {
                RotateTowards(playerPosition);
                agent.SetDestination(agent.transform.position);
                if (once)
                {
                    StartCoroutine(Attack());
                    once = false;
                }
            }
            #endregion

            #region Chasing Logic
            if (isChasing)
            {
                agent.speed = speed;
                RotateTowards(playerPosition);
                NavMeshPath path = new NavMeshPath();
                NavMesh.CalculatePath(transform.position, playerPosition.position, NavMesh.AllAreas, path);

                agent.SetPath(path);

                if (agent.velocity != Vector3.zero)
                {
                    state = CurrentState.Running;
                }
                else
                {
                    state = CurrentState.Idle;
                }
            }
            #endregion

            #region Running Logic
            if (isRunning)
            {
                Vector3 directionToPlayer = transform.position - playerPosition.position;
                Vector3 destination = transform.position + directionToPlayer;
                agent.speed = speed * 1.6f;
                NavMeshPath path = new NavMeshPath();
                NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);

                agent.SetPath(path);
                if (agent.velocity != Vector3.zero)
                {
                    state = CurrentState.Running;
                }
                else
                {
                    state = CurrentState.Idle;
                }
            }
            #endregion

            #region Roaming Logic
            if (isRoaming)
            {
                if (agent.velocity != Vector3.zero) state = CurrentState.Walking;
                else if (state != CurrentState.Special && agent.velocity == Vector3.zero && state != CurrentState.Tired) state = CurrentState.Idle;

                agent.speed = speed / 2;
                roamTimerStart -= Time.deltaTime;

                if (roamTimerStart <= 0)
                {
                    if (hasSpecial)
                    {
                        int random = Random.Range(1, 100);
                        if (random >= 1 && random < 80)
                        {
                            NavMeshHit closestHit;
                            if (NavMesh.SamplePosition(RandomNavmeshLocation(randomCircleRadius), out closestHit, randomCircleRadius, 1))
                            {
                                agent.SetDestination(closestHit.position);
                            }
                        }
                        else if (random >= 90 && random < 100)
                        {
                            if (once1)
                            {
                                if (specialAudioClip != null)
                                {
                                    specialAudioClip.Play();
                                }
                                state = CurrentState.Special;
                                once1 = false;
                            }
                        }
                    }
                    else
                    {
                        NavMeshHit closestHit;
                        if (NavMesh.SamplePosition(RandomNavmeshLocation(randomCircleRadius), out closestHit, randomCircleRadius, 1))
                        {
                            agent.SetDestination(closestHit.position);
                        }
                    }
                    roamTimerStart = Random.Range(roamTimeMin, roamTimeMax);
                }

                //if (hasStamina)
                //{
                //    stamina -= Time.deltaTime / 2;
                //
                //    if (stamina <= 0) tired = true;
                //
                //    if (tired)
                //    {
                //        agent.SetDestination(agent.transform.position);
                //        state = CurrentState.Tired;
                //        stamina += Time.deltaTime * 2;
                //    }
                //    if(stamina > maxStamina / 1.5f)
                //    {
                //        state = CurrentState.Awake;
                //    }
                //
                //    if (stamina >= maxStamina)
                //    {
                //        
                //        tired = false;
                //        stamina = maxStamina;
                //    }
                //
                //    if (roamTimerStart <= 0 && !tired)
                //    {
                //        if (hasSpecial)
                //        {
                //            int random = Random.Range(1, 100);
                //            if (random >= 1 && random < 80)
                //            {
                //                agent.SetDestination(RandomNavmeshLocation(randomCircleRadius));
                //            }
                //            else if (random >= 90 && random < 100)
                //            {
                //                if (once1)
                //                {
                //                    state = CurrentState.Special;
                //                    once1 = false;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            agent.SetDestination(RandomNavmeshLocation(randomCircleRadius));
                //        }
                //        roamTimerStart = Random.Range(roamTimeMin, roamTimeMax);
                //    }
                //}
                //else
                //{
                //    if (roamTimerStart <= 0)
                //    {
                //        if (hasSpecial)
                //        {
                //            int random = Random.Range(1, 100);
                //            if (random >= 1 && random < 80)
                //            {
                //                agent.SetDestination(RandomNavmeshLocation(randomCircleRadius));
                //            }
                //            else if (random >= 90 && random < 100)
                //            {
                //                if (once1)
                //                {
                //                    state = CurrentState.Special;
                //                    once1 = false;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            agent.SetDestination(RandomNavmeshLocation(randomCircleRadius));
                //        }
                //        roamTimerStart = Random.Range(roamTimeMin, roamTimeMax);
                //    }
                //}



            }
            #endregion
        }
        else 
        {
            agent.ResetPath();
            agent.SetDestination(transform.position);
            transform.GetComponent<CapsuleCollider>().enabled = false;
            state = CurrentState.Dead;
            StartCoroutine(DestroyEnemy());
        }
    }

    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void AnimationStates()
    {
        switch (state)
        {
            case CurrentState.Walking: ChangeAnimationState(walkState); break;
            case CurrentState.Running: ChangeAnimationState(runState); break;
            case CurrentState.Special: ChangeAnimationState(specialState); break;
            case CurrentState.Attacking: ChangeAnimationState(attackState); break;
            case CurrentState.Idle: ChangeAnimationState(idleState); break;
            case CurrentState.Dead: ChangeAnimationState(deadState); break;
            case CurrentState.Hurt: ChangeAnimationState(hurtState); break;
            case CurrentState.Blocked: ChangeAnimationState(blockedState); break;
            case CurrentState.Awake: ChangeAnimationState(endRestState); break;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            if (willGetHurt)
            {
                state = CurrentState.Hurt;
            }
            if (hurtAudioClip != null)
            {
                hurtAudioClip.Play();
            }
            health -= damage;
        }  
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    private IEnumerator Attack()
    {
        float waitTime = attackAnimLength;
        float counter = 0;
        while (counter <= waitTime)
        {
            if (!blocked)
            {
                state = CurrentState.Attacking;
            }
            if (counter > attackAnimLength / 2 && check)
            {
                if(attackAudioClip != null)
                {
                    attackAudioClip.Play();
                }
                if (blocked)
                {
                    state = CurrentState.Blocked;
                }
                else
                {
                    playerScript.TakeDamage(damage);
                    
                }
                check = false;
            }
            
            counter += Time.deltaTime;
            if (counter >= waitTime)
            {
                state = CurrentState.Idle;
                once = true;
                check = true;
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator DestroyEnemy()
    {
        float waitTime = 10f;
        float counter = 0;
        while (counter <= waitTime)
        {
            counter += Time.deltaTime;
            if (counter >= waitTime)
            {
                switch (aiName)
                {
                    case "Bear": spawnAnimals.bearAmounttt -= 1; break;
                    case "Wolf1": spawnAnimals.wolf1Amountt -= 1; break;
                    case "Wolf2": spawnAnimals.wolf2Amountt -= 1; break;
                    case "Boar": spawnAnimals.boarAmounttt -= 1; break;
                    case "DeerM": spawnAnimals.deerMAmountt -= 1; break;
                    case "DeerF": spawnAnimals.deerFAmountt -= 1; break;
                    case "Cow1": spawnAnimals.cow1Amounttt -= 1; break;
                    case "Cow2": spawnAnimals.cow2Amounttt -= 1; break;
                    case "Cow3": spawnAnimals.cow3Amounttt -= 1; break;
                    case "Rabbit": spawnAnimals.rabbitAmount -= 1; break;
                    case "Sheep": spawnAnimals.sheepAmountt -= 1; break;
                    case "Fox": spawnAnimals.foxAmountttt -= 1; break;
                }
                health = mainMaxHealth;
                transform.GetComponent<CapsuleCollider>().enabled = true;
                gameObject.SetActive(false);
                isDead = false;
                yield break;
            }
            yield return null;
        }
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState && currentState != attackState) return;

        anim.Play(newState);

        currentState = newState;
    }
}
