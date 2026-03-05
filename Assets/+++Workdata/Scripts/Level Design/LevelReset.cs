using System;
using UnityEngine;

public class LevelReset : MonoBehaviour
{
    public static event Action onLevelFinished;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            onLevelFinished?.Invoke();
    }
}
