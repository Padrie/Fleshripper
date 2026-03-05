using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BulletEffects/Lightning")]
public class LightningEffect : ScriptableObject, IBulletEffect
{
    public float damage = 20f;
    public float range = 10f;
    public int chainAmount = 2;
    public bool endlessLightning = false;

    public void ApplyEffect(Bullet bullet, GameObject gameObject)
    {
        EnemyManager.Instance.StartCoroutine(ChainEffect(gameObject));
    }

    public LineRenderer lineRendererPrefab;

    public IEnumerator ChainEffect(GameObject gameObject)
    {
        List<GameObject> excludeList = new List<GameObject>();
        GameObject currentTarget = gameObject;
        int currentChains = 0;

        while (currentTarget != null && currentChains < chainAmount)
        {
            excludeList.Add(currentTarget);
            DealDamage(currentTarget);

            GameObject nextTarget = GetNearestEnemy(currentTarget.transform.position, excludeList);
            if (nextTarget == null)
                break;

            if (lineRendererPrefab != null)
            {
                LineRenderer lr = Instantiate(lineRendererPrefab);
                lr.SetPosition(0, currentTarget.transform.position + Vector3.up);
                lr.SetPosition(1, nextTarget.transform.position + Vector3.up);
                Destroy(lr.gameObject, 0.15f);
            }

            currentTarget = nextTarget;
            excludeList.RemoveAll(enemy => enemy == null);

            if (excludeList.Count >= EnemyManager.GetEnemyList().Count - 1 && endlessLightning)
            {
                excludeList.Clear();
            }

            currentChains++;
            yield return null;
        }
    }

    GameObject GetNearestEnemy(Vector3 fromPosition, List<GameObject> exclude)
    {
        List<GameObject> allEnemies = EnemyManager.GetEnemyList();

        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in allEnemies)
        {
            if (enemy == null || exclude.Contains(enemy)) continue;

            float dist = Vector3.Distance(fromPosition, enemy.transform.position);
            if (dist < closestDistance && dist <= range)
            {
                closest = enemy;
                closestDistance = dist;
            }
        }

        return closest;
    }

    void DealDamage(GameObject enemyObject)
    {
        if (enemyObject.TryGetComponent<IDamagable>(out var enemy))
        {
            enemy.TakeDamage(damage);
        }
    }
}
