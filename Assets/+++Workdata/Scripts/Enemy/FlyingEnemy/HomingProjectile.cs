using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public int damage = 30;
    public float speed = 5f;
    public float rotateSpeed = 200f;
    public float lifeTime = 5f;
    public float explosionRadius = 10f;
    public GameObject explosionParticle;
    private Transform target;

    private Rigidbody rb;
    private Player playerObj;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        playerObj = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (playerObj != null)
        {
            target = playerObj.transform;
        }

        rb.MoveRotation(Quaternion.LookRotation((target.position - rb.position).normalized));

        Invoke("DestroyObject", lifeTime);
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 direction = (target.position - rb.position).normalized;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, rotateSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(Quaternion.LookRotation(newDir));

        rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            DestroyObject();
        }
        else if (collision.collider.CompareTag("Ground"))
        {
            DestroyObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        Explode();

        GameObject particle = Instantiate(explosionParticle, transform.position, Quaternion.identity);
        particle.transform.localScale = Vector3.one * 2;

        Destroy(gameObject);
    }

    public void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, explosionRadius);

        foreach (Collider collider in hits)
        {
            if (collider.CompareTag("Player"))
            {
                playerObj.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
