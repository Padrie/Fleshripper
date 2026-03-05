using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleUseEnemySpawner : MonoBehaviour
{
    public float dis;
    [SerializeField] float spawnInRange;
    [SerializeField] int spawnDelay;
    [SerializeField] float spawnInterval;
    [SerializeField] int spawnAmountPerInterval;
    [SerializeField] int maxSpawnAmount;
    [SerializeField] GameObject spawnObject;

    [SerializeField] bool singleUse;

    bool toggle = false;

    GameObject player;

    List<GameObject> spawnList = new List<GameObject>();

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        //StartCoroutine(Spawn());
    }

    private void Update()
    {
        dis = Vector3.Distance(gameObject.transform.position, player.transform.position);

        if (dis <= spawnInRange && !toggle)
        {
            print("WHYY");
            StartCoroutine(Spawn());
            toggle = true;
        }

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
