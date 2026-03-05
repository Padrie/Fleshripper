using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] int spawnDelay;
    [SerializeField] float spawnInterval;
    [SerializeField] int spawnAmountPerInterval;
    [SerializeField] int maxSpawnAmount;
    [SerializeField] GameObject spawnObject;

    List<GameObject> spawnList = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private void Update()
    {
        for (int i = 0; i < spawnList.Count; i++)
        {
            if (spawnList[i] == null)
            {
                spawnList.RemoveAt(i);
            }
        }
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(spawnDelay);

        while (true)
        {
            if(spawnList.Count <= maxSpawnAmount - 1)
            {            
                if (spawnAmountPerInterval > maxSpawnAmount)
                    spawnAmountPerInterval = maxSpawnAmount;
                for (int i = 0; i < spawnAmountPerInterval; i++)
                {
                    GameObject obj = Instantiate(spawnObject, gameObject.transform);
                    EnemyManager.AddToEnemyList(obj);
                    spawnList.Add(obj);
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
