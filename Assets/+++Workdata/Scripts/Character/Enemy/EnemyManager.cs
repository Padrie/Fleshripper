using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static List<GameObject> enemyList = new List<GameObject>();

    private static EnemyManager _instance;

    public static EnemyManager Instance { get { return _instance; } }

    static int totalKills = 0;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public static void AddToEnemyList(GameObject enemy)
    {
        enemyList.Add(enemy);
    }

    public static void AddKills(int value)
    {
        totalKills += value;
    }

    public static int GetKills()
    {
        return totalKills;
    }

    public static void RemoveFromEnemyList(GameObject enemy)
    {
        //Destroy(enemy);
        enemyList.Remove(enemy);
    }

    public static List<GameObject> GetEnemyList()
    {
        return enemyList;
    }

    private void Update()
    {
        enemyList.RemoveAll(e => e == null);
    }
}