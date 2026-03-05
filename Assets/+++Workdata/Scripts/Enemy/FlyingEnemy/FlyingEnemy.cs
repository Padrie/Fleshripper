using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemy : MonoBehaviour, IDamagable
{
    [Header("Stats")]
    public float health = 50f;
    [Header("Projectile")]
    public GameObject projectile;
    public float projectileSpeed = 10f;
    public float fireRate = 1f;
    public float shootRange = 10f;
    public int projectileAmount = 3;
    [Header("Homing Projectile")]
    public GameObject homingProjectile;
    public int shotsNeededForHoming = 3;
    public float chargeUpTime = 3f;
    public float recoveryTime = 2f;
    public int shotsFired = 0;
    public bool chargingShot = false;
    public LayerMask layerMask;
    [Header("Misc")]
    public GameObject sprite;
    public Rigidbody body;
    public NavMeshAgent agent;

    private Animator animator;
    private GameObject player;
    private Player playerScript;

    private Vector3 spriteInitialPosition;

    public static event Action<FlyingEnemy> OnFlyingEnemyDied;

    private RaycastHit hit;

    private bool isPlayerVisible = false;
    private bool isDead = false;

    void Start()
    {
        EnemyManager.AddToEnemyList(gameObject);

        shotsFired = 0;
        chargingShot = false;
        var playerObj = GameObject.FindWithTag("Player");

        body = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        if (playerObj != null)
        {
            player = playerObj;
            playerScript = player.GetComponent<Player>();
            StartCoroutine(Shoot());
        }

        spriteInitialPosition = sprite.transform.localPosition;

        animator.Play("Attack");
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, DirectionTo(player.transform.position), out hit, shootRange, layerMask, QueryTriggerInteraction.Ignore) && !chargingShot && !isDead)
        {
            if (hit.collider.CompareTag("Player"))
            {
                isPlayerVisible = true;
            }
            else
            {
                isPlayerVisible = false;
            }
        }
        else
        {
            isPlayerVisible = false;
        }

        if (isDead) return;

        if (isPlayerVisible && agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
        else if (!agent.isStopped && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
    }

    public void MoveTowardsPlayer()
    {
        agent.SetDestination(player.transform.position);
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            if (isPlayerVisible)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    for (int i = -1; i < projectileAmount - 1; i++)
                    {
                        GameObject p = Instantiate(projectile, transform.position, Quaternion.identity);
                        var rb = p.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            Vector3 dir = DirectionTo(player.transform.position + new Vector3(i * 15 / projectileAmount, UnityEngine.Random.Range(-.5f, .5f), 0));
                            rb.linearVelocity = dir * projectileSpeed;
                        }

                        if (rb.linearVelocity.sqrMagnitude > 0.001f)
                        {
                            p.transform.rotation = Quaternion.LookRotation(rb.linearVelocity.normalized);
                        }
                    }

                    shotsFired++;

                    if (shotsFired == shotsNeededForHoming)
                    {
                        StartCoroutine(ChargeUpShot());
                    }

                    yield return new WaitForSeconds(fireRate);
                }
                else
                {
                    print("Player not in sight 2");
                    Debug.DrawRay(transform.position, DirectionTo(player.transform.position) * shootRange, Color.yellow);
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }

            yield return null;
        }
    }

    private Vector3 DirectionTo(Vector3 target)
    {
        return (target - transform.position).normalized;
    }

    private IEnumerator ChargeUpShot()
    {
        float elapsed = 0f;

        chargingShot = true;

        while (elapsed < chargeUpTime)
        {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / chargeUpTime;

            sprite.transform.localPosition += new Vector3(UnityEngine.Random.Range(-0.05f, 0.05f), 0, UnityEngine.Random.Range(-0.05f, 0.05f)) * percentComplete;

            yield return null;
        }

        sprite.transform.localPosition = spriteInitialPosition;

        Instantiate(homingProjectile, transform.position + Vector3.up, Quaternion.LookRotation(player.transform.position - body.position).normalized);

        yield return new WaitForSeconds(recoveryTime);

        chargingShot = false;
        shotsFired = 0;

        yield return null;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        GameObject particle = VFXManager.instance.Play("EnemyTakeDamage");
        ObjectPoolManager.SpawnObject(particle, gameObject.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.ParticleSystems);

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy name: " + gameObject.name);
        EnemyManager.AddKills(1);
        EnemyManager.RemoveFromEnemyList(gameObject);
        Collider col = GetComponent<Collider>();
        OnFlyingEnemyDied?.Invoke(this);
        animator.Play("Die");
        agent.enabled = false;
        col.enabled = false;
        isDead = true;

        GameObject particle = VFXManager.instance.Play("EnemyTakeDamage");
        ObjectPoolManager.SpawnObject(particle, gameObject.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.ParticleSystems);

        StopAllCoroutines();
        StartCoroutine(Delete());
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

    IEnumerator Delete()
    {
        yield return new WaitForSeconds(0.75f);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, agent.stoppingDistance);
    }
}
