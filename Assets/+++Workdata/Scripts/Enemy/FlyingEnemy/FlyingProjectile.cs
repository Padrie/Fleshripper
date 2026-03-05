using UnityEngine;

public class FlyingProjectile : MonoBehaviour
{
    public int bulletDamage = 20;
    public int lifeTime = 5;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Player>(out var damagable))
                damagable.TakeDamage(bulletDamage);

            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}