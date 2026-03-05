using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "BulletEffects/Sticky Explosion")]
public class StickyExplosionEffect : ScriptableObject, IBulletEffect
{
    public GameObject bombPrefab;
    public float damage = 20f;
    public float radius = 5f;
    public GameObject particleSystem;
    public Vector2 knockbackStrength = Vector2.one;

    public bool boostPlayer = false;

    float test = 0f;

    public void ApplyEffect(Bullet bullet, GameObject gameObject)
    {
        EnemyManager.Instance.StartCoroutine(Explosion(gameObject));
        GameObject bomb = Instantiate(bombPrefab, bullet.transform.position, Quaternion.identity, gameObject.transform);
    }

    public IEnumerator Explosion(GameObject gameObject)
    {
        Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, radius);

        yield return new WaitForSeconds(1f);

        foreach (Collider collider in hits)
        {
            if (collider.TryGetComponent<IDamagable>(out var enemy))
            {
                enemy.TakeDamage(damage);
                EnemyManager.Instance.StartCoroutine(enemy.ApplyKnockBack(knockbackStrength));

                var particle = VFXManager.instance.Play("BulletExplosion");
                ObjectPoolManager.SpawnObject(particle, gameObject.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.ParticleSystems);
                //particleSystem.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
            }
        }

        yield return null;
    }
}