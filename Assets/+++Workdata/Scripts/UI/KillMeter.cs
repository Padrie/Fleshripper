using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class KillMeter : MonoBehaviour
{
    public GameObject segmentPrefab;
    public GameObject segments;
    public Slider killMeter;
    public Slider killTimer;
    public int segmentAmount = 3;
    public float clearTime = 2f;
    public int currentCappedKillCombo;

    public int totalKills;
    public int totalEnemies;

    private float currentClearTime;
    private int currentSegmentAmount;

    private bool CRrunning = false;

    private void Start()
    {
        currentClearTime = clearTime;
        currentSegmentAmount = segmentAmount;
        SetSegmentAmount();
    }
    
    private void Update()
    {
        totalKills = EnemyManager.GetKills();
        totalEnemies = EnemyManager.GetEnemyList().Count;
    }

    void SetSegmentAmount()
    {
        for (int i = 0; i < currentSegmentAmount; i++)
        {
            var segment = Instantiate(segmentPrefab, segments.transform);
            killMeter.maxValue++;
        }
        killMeter.maxValue--;
    }

    void AddSegment(int value)
    {
        for (int i = 0; i < value; i++)
        {
            var segment = Instantiate(segmentPrefab, segments.transform);
            killMeter.maxValue++;
            currentSegmentAmount++;
        }
    }

    public void AddPoint()
    {
        if (killMeter.value >= currentSegmentAmount)
        {
            print("FULL");
            return;
        }

        Mathf.Clamp(currentCappedKillCombo++, 0, currentSegmentAmount);
        killMeter.value = currentCappedKillCombo;

        if (!CRrunning)
            StartCoroutine(StartTimer());
    }

    public void CrossedSegment()
    {
        StartCoroutine(Shake());
        print("Crossed Segment");
    }

    public void ClearMeter()
    {
        killMeter.value = 0;
        killTimer.value = 0;
        currentCappedKillCombo = 0;
    }

    IEnumerator Shake()
    {
        float elapsed = 0f;
        float shakeAmount = 8f;
        float shakeTime = .05f;
        Vector3 originalPos = gameObject.transform.position;

        while (elapsed < shakeTime)
        {
            Vector3 randomPoint = originalPos + UnityEngine.Random.insideUnitSphere * shakeAmount;
            gameObject.transform.position = randomPoint;
            elapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.position = originalPos;

        yield return null;
    }

    IEnumerator StartTimer()
    {
        float elapsed = 0f;
        float savedSegmentIndex = 1 / (float)segmentAmount;
        float segmentIndex = savedSegmentIndex;
        CRrunning = true;

        while (elapsed < 1 && killMeter.value != currentSegmentAmount)
        {
            killTimer.value = elapsed;
            elapsed += Time.deltaTime / clearTime;

            if (elapsed > segmentIndex)
            {
                CrossedSegment();
                segmentIndex += savedSegmentIndex;
            }

            yield return null;
        }

        ClearMeter();
        CRrunning = false;
        yield return null;
    }
}
