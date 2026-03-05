using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ExplosionProjectile : MonoBehaviour
{
    public ProjectileStats projectileStats;

    [HideInInspector] public int damage;
    int penetration;

    private void Awake()
    {
        damage = projectileStats.damage;
        penetration = projectileStats.penetration;
    }

    void Start()
    {
        Invoke("Disappear", 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            StartCoroutine(ApplyKnockBack(other));

            print("Enemy");
        }
    }
    public IEnumerator ApplyKnockBack(Collider other)
    {
        NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
        Rigidbody rb = other.GetComponent<Rigidbody>();
        agent.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddExplosionForce(500, other.transform.position, 500);
        yield return new WaitForSeconds(1f);
        agent.Warp(agent.transform.position);
        agent.enabled = true;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void Disappear()
    {
        Destroy(gameObject);
    }
}
