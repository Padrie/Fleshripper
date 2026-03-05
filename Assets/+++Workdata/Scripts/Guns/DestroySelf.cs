using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float disableIn = 1f;

    private void OnEnable()
    {
        Invoke("DisableSelf", disableIn);
    }

    private void DisableSelf()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.ParticleSystems);
    }
}
