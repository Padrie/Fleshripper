using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ___Workdata.Scripts.Enemy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

public abstract class Enemy : MonoBehaviour, IEnemy
{
    public int ID { get; set; }

    [Header("Objects")]
    public NavMeshAgent agent;
    public Rigidbody body;
    public Animator animator;

    [Header("Logic")]
    public bool noGravity = false;
    public float gravity = -9.8f;
    public Vector3 velocity;
    public Vector3 direction;

    [Header("Stats")]
    public EnemyStats Stats = new(100, 10, 1, 8);
    public int contactDamage;
    public int contactCooldown;


    [Header("Targeting")]
    public bool targetInRange;
    public bool targetInAttackRange;
    public float targetRange;
    public float attackRange;

    public Player target;

    public float walkPointDistance;

    public Vector3 walkDest;
    public bool foundWalkDest;

    public static event Action<Enemy> OnEnemyDied;

    public bool isDead = false;

    private List<Collider> colliders;

    public void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        agent = GetComponent<NavMeshAgent>();

        body = GetComponent<Rigidbody>();

        animator = GetComponentInChildren<Animator>();

        Initialize();

        colliders = GetComponents<Collider>().ToList();
    }

    public virtual void Initialize()
    {
        EnemyManager.AddToEnemyList(gameObject);
        animator.Play("Idle");
        isDead = false;
    }

    public void TakeDamage(float damage)
    {
        Stats.Health -= damage;
        target.ResetCombatTimer();

        GameObject particle = VFXManager.instance.Play("EnemyTakeDamage");
        ObjectPoolManager.SpawnObject(particle, gameObject.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.ParticleSystems);

        if (Stats.Health <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy name: " + gameObject.name);
        OnEnemyDied?.Invoke(this);
        EnemyManager.AddKills(1);
        EnemyManager.RemoveFromEnemyList(gameObject);
        animator.Play("Die");
        agent.enabled = false;
        GetComponentInChildren<EnemyRotation>().isDead = true;
        isDead = true;

        GameObject particle = VFXManager.instance.Play("EnemyDie");
        ObjectPoolManager.SpawnObject(particle, gameObject.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.ParticleSystems);

        foreach (Collider collider in colliders)
            collider.enabled = false;
    }

    public virtual void HandleGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    public virtual void CheckGrounded()
    {
    }

    public void Update()
    {
        CheckTarget();

        AIUpdate();

        velocity = body.linearVelocity;

    }

    public virtual void AIUpdate()
    {
    }

    public virtual void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointDistance, walkPointDistance);
        float randomX = Random.Range(-walkPointDistance, walkPointDistance);

        walkDest = new Vector3(transform.position.x + randomX, transform.position.y,
            transform.position.z + randomZ);

        if (Physics.Raycast(walkDest, -transform.up, 2f, LayerMask.GetMask("Ground")))
            foundWalkDest = true;
    }

    public void CheckTarget()
    {
        targetInRange = Physics.CheckSphere(transform.position, targetRange, LayerMask.GetMask("Player"));
        targetInAttackRange = Physics.CheckSphere(transform.position, attackRange, LayerMask.GetMask("Player"));
    }

    //Das ist noch messy das braucht cleanup 
    public void HandleRotation()
    {
        direction = target.transform.position - transform.position;
        direction.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 / Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Contact());
        }
    }

    public virtual void Idle()
    {
        //direction = transform.position;
        //direction.Normalize();

        //if (!foundWalkDest) SearchWalkPoint();

        //if (foundWalkDest)
        //    agent.SetDestination(walkDest);

        //Vector3 distanceToWalkPoint = transform.position - walkDest;

        ////Walkpoint reached
        //if (distanceToWalkPoint.magnitude < 1f)
        //    foundWalkDest = false;
    }

    public virtual void Approach()
    {
        HandleRotation();

        walkDest = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);

        if (agent.isOnNavMesh)
        {
            agent.SetDestination(walkDest);
        }
    }

    public virtual void Attack()
    {
        throw new NotImplementedException();
    }

    public IEnumerator Contact()
    {
        if (!isDead)
        {
            //AudioManager.instance.Play("EnemyAttack");
            animator.Play("Attack");
            target.TakeDamage(contactDamage);
        }
        yield return new WaitForSeconds(contactCooldown);
    }

    public virtual void Death()
    {
        throw new NotImplementedException();
    }

    public float DistanceTo(Vector3 position)
    {
        return Vector3.Distance(transform.position, position);
    }

    public float DistanceFrom(Vector3 position)
    {
        return Vector3.Distance(position, transform.position);
    }

    public Vector3 DirectionTo(Vector3 position)
    {
        return new Vector3(position.x - transform.position.x, position.y - transform.position.y,
            position.z - transform.position.z).normalized;
    }

    public Vector3 DirectionFrom(Vector3 position)
    {
        return new Vector3(transform.position.x - position.x, transform.position.y - position.y,
            transform.position.z - position.z).normalized;
    }

    public IEnumerator ApplyKnockBack(Vector3 strength)
    {
        yield return null;

        Collider col = GetComponent<Collider>();
        Vector3 yZero = new Vector3(1, 0, 1);
        Vector3 dir = Multiply((transform.position - GameObject.FindWithTag("Player").transform.position).normalized, yZero);

        Debug.Log(dir);
        agent.enabled = false;
        body.isKinematic = false;
        body.useGravity = true;
        col.isTrigger = false;

        body.AddForce(dir * strength.x + Vector3.up * strength.y, ForceMode.Impulse);

        yield return new WaitForFixedUpdate();
        float knockbackTime = Time.time;
        yield return new WaitUntil(() => body.linearVelocity.magnitude < 0.5f || Time.time > knockbackTime + 5);
        yield return new WaitForSeconds(0.25f);

        body.isKinematic = true;
        col.isTrigger = true;
        body.useGravity = false;
        agent.Warp(transform.position);
        if (!isDead)
            agent.enabled = true;


        yield return null;
    }

    Vector3 Multiply(Vector3 arg1, Vector3 arg2)
    {
        float x = arg1.x * arg2.x;
        float y = arg1.y * arg2.y;
        float z = arg1.z * arg2.z;

        return new Vector3(x, y, z);
    }

    public void ApplyEffect(StatusEffect effect, float duration)
    {
        effect.StartEffect(this);

        StartCoroutine(UpdateEffect(effect, duration));

        effect.ResetEffect(this);
    }

    public IEnumerator UpdateEffect(StatusEffect effect, float duration)
    {

        effect.Update(this);

        yield return new WaitForSeconds(duration);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRange);
    }
}