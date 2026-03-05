using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DashMeter : MonoBehaviour
{
    public int dashSegmentAmount = 3;
    public int killsPerDashSegment = 6;
    public List<Slider> sliderList;

    public bool dash = false;

    Player player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        int sliderIndex = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (child.TryGetComponent<Slider>(out var slider))
            {
                sliderList.Add(slider);

                slider.minValue = sliderIndex * killsPerDashSegment;
                slider.maxValue = (sliderIndex + 1) * killsPerDashSegment;
                slider.value = 0;

                sliderIndex++;
            }
        }

        player.currentPoints = 0;
    }

    private void OnEnable()
    {
        Player.OnPlayerDashed += RemovePoints;
        Enemy.OnEnemyDied += AddPoint;
        FlyingEnemy.OnFlyingEnemyDied += AddPoint;
    }

    private void OnDisable()
    {
        Player.OnPlayerDashed -= RemovePoints;
        Enemy.OnEnemyDied -= AddPoint;
        FlyingEnemy.OnFlyingEnemyDied -= AddPoint;
    }

    private void Update()
    {
        for (int i = player.stamina; i < sliderList.Count; i++)
        {
            if (sliderList[i].value == sliderList[i].maxValue)
            {
                Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
                AudioManager.instance.Play("DashRecharged");
                player.stamina++;
            }
        }

        if (dash)
        {
            RemovePoints();

            dash = false;
        }

        player.stamina = Mathf.Clamp(player.stamina, 0, sliderList.Count);
    }

    private void AddPoint(Enemy enemy)
    {
        player.currentPoints++;
        player.currentPoints = Mathf.Clamp(player.currentPoints, 0, killsPerDashSegment * sliderList.Count);

        for (int i = 0; i < sliderList.Count; i++)
        {
            sliderList[i].value = player.currentPoints;
        }
    }    
    
    private void AddPoint(FlyingEnemy enemy)
    {
        player.currentPoints++;
        player.currentPoints = Mathf.Clamp(player.currentPoints, 0, killsPerDashSegment * sliderList.Count);

        for (int i = 0; i < sliderList.Count; i++)
        {
            sliderList[i].value = player.currentPoints;
        }
    }

    private void RemovePoints()
    {
        if (killsPerDashSegment <= player.currentPoints)
        {
            player.currentPoints -= killsPerDashSegment;
            player.stamina--;

            foreach (var slider in sliderList)
            {
                slider.value = player.currentPoints;
            }
        }
    }
}
