using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Player>(out var damagable))
            {
                damagable.TakeDamage(10000);
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<IDamagable>(out var damagable))
            {
                damagable.TakeDamage(100000);
            }
        }
    }
}
