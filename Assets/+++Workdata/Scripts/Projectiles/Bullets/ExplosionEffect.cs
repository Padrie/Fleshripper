using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "BulletEffects/Explosion")]
public class ExplosionEffect : ScriptableObject, IBulletEffect
{
    public float damage = 20f;
    public float radius = 5f;
    public GameObject particleSystem;
    public Vector2 knockbackStrength = Vector2.one;

    public bool boostPlayer = false;

    public void ApplyEffect(Bullet bullet, GameObject gameObject)
    {
        Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, radius);

        RuntimeManager.PlayOneShot("event:/SFX/Explosion_Shotgun" , gameObject.transform.position);

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
    }
}