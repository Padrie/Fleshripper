using System.Collections;
using UnityEngine;

public interface IDamagable
{
    void TakeDamage(float damage);

    void Die();

    IEnumerator ApplyKnockBack(Vector3 knockbackStrength);
}
