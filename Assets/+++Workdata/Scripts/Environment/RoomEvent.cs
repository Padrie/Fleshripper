using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class RoomEvent : MonoBehaviour
{
    [Header("List of Enemies")]
    public List<Enemy> enemies;

    [Header("List of Doors")]
    public List<Animator> doorAnimators;

    [Header("List of Lights")]
    public List<Light> lights;

    [Space(15)]
    public float doorOpenDelay = 0.5f;
    public float dimLightSpeed = 10f;
    private bool doorsOpened = false;

    [Header("Sound")] 
    public EventReference gateOpenSound;

    private void OnEnable()
    {
        Enemy.OnEnemyDied += RemoveEnemyFromList;
    }    
    
    private void OnDisable()
    {
        Enemy.OnEnemyDied -= RemoveEnemyFromList;
    }

    public void RemoveEnemyFromList(Enemy enemy)
    {
        enemies.Remove(enemy);
    }    

    void Update()
    {
        if (doorsOpened)
            return;

        enemies.RemoveAll(enemy => enemy == null);
        lights.RemoveAll(light => light == null);
        doorAnimators.RemoveAll(animator => animator == null);

        if (enemies.Count == 0)
        {
            StartCoroutine(OpenDoors());
            StartCoroutine(DimLights());
            doorsOpened = true;
        }
    }

    private IEnumerator OpenDoors()
    {
        yield return new WaitForSeconds(doorOpenDelay);

        foreach (Animator animator in doorAnimators)
        {
            if (animator != null)
            {
                RuntimeManager.PlayOneShot(gateOpenSound , animator.transform.position);
                animator.SetTrigger("Open");
            } 
        }
        //gameObject.SetActive(false);
    }

    private IEnumerator DimLights()
    {
        if(lights.Count == 0)
            yield break;

        float elapsed = 0f;

        List<float> startIntensities = new List<float>();
        foreach (Light light in lights)
        {
            startIntensities.Add(light.intensity);
        }

        while (elapsed < dimLightSpeed)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dimLightSpeed);

            for (int i = 0; i < lights.Count; i++)
            {
                if (lights[i] != null)
                    lights[i].intensity = Mathf.Lerp(startIntensities[i], 0f, t);
            }

            yield return null;
        }

        foreach (Light light in lights)
        {
            if (light != null)
                light.intensity = 0f;
        }
    }
}
