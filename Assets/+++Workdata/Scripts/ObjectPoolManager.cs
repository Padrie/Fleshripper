using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;
using static ObjectPoolManager;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] bool addToDontDestroyOnLoad = false;

    GameObject emptyHolder;

    static GameObject particleSystemsEmpty;
    static GameObject gameObjectsEmpty;
    static GameObject soundFXEmpty;

    static Dictionary<GameObject, ObjectPool<GameObject>> objectPools;
    static Dictionary<GameObject, GameObject> cloneToPrefabMap;

    public enum PoolType
    {
        ParticleSystems,
        GameObjects,
        SoundFX
    }
    public static PoolType poolingType;

    private void Awake()
    {
        objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
        cloneToPrefabMap = new Dictionary<GameObject, GameObject>();

        SetupEmpties();
    }

    void SetupEmpties()
    {
        emptyHolder = new GameObject("Objects Pools");

        particleSystemsEmpty = new GameObject("Particle Effects");
        particleSystemsEmpty.transform.SetParent(emptyHolder.transform);

        gameObjectsEmpty = new GameObject("GameObjects");
        gameObjectsEmpty.transform.SetParent(emptyHolder.transform);

        soundFXEmpty = new GameObject("Sound FX");
        soundFXEmpty.transform.SetParent(emptyHolder.transform);

        if (addToDontDestroyOnLoad)
            DontDestroyOnLoad(gameObjectsEmpty.transform.root);
    }

    static void CreatePool(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, pos, rot, poolType),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
            );

        objectPools.Add(prefab, pool);
    }

    static GameObject CreateObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects)
    {
        prefab.SetActive(false);

        GameObject obj = Instantiate(prefab, pos, rot);

        prefab.SetActive(true);

        GameObject parentObject = SetParentObject(poolType);
        obj.transform.SetParent(parentObject.transform);

        return obj;
    }

    static void OnGetObject(GameObject obj)
    {
        // optional logic for when we get the object
    }

    static void OnReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    static void OnDestroyObject(GameObject obj)
    {
        if (cloneToPrefabMap.ContainsKey(obj))
        {
            cloneToPrefabMap.Remove(obj);
        }
    }

    static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.ParticleSystems:
                return particleSystemsEmpty;

            case PoolType.GameObjects:
                return gameObjectsEmpty;

            case PoolType.SoundFX:
                return soundFXEmpty;

            default:
                return null;
        }
    }

    static T SpawnObject<T>(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) where T : Object
    {
        if (!objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, spawnPos, spawnRotation, poolType);
        }

        GameObject obj = objectPools[objectToSpawn].Get();

        if (obj != null)
        {
            if (!cloneToPrefabMap.ContainsKey(obj))
            {
                cloneToPrefabMap.Add(obj, objectToSpawn);
            }

            obj.transform.position = spawnPos;
            obj.transform.rotation = spawnRotation;
            obj.SetActive(true);

            if (typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }

            T component = obj.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError($"Object {objectToSpawn.name} doesn't have component of type {typeof(T).Name}");
                return null;
            }

            return component;
        }

        return null;
    }

    public static T SpawnObject<T>(T typePrefab, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) where T : Component
    {
        return SpawnObject<T>(typePrefab.gameObject, spawnPos, spawnRotation, poolType);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects)
    {
        return SpawnObject<GameObject>(objectToSpawn, spawnPos, spawnRotation, poolType);
    }

    public static void ReturnObjectToPool(GameObject obj, PoolType poolType = PoolType.GameObjects)
    {
        if (cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
        {
            GameObject parentObject = SetParentObject(poolType);
            if (obj.transform.parent != parentObject.transform)
            {
                obj.transform.SetParent(parentObject.transform);
            }

            if (objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }
        }
        else
        {
            Debug.LogWarning("Trying to return an Objec that is not pooled: " + obj.name);
        }
    }
}
