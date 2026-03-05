using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] List<VFX> vfx;
    public static VFXManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public GameObject Play(string name)
    {
        VFX v = vfx.Find(v =>  v.name == name);

        if (v == null)
        {
            Debug.LogWarning($"Particle System {v.name} does not exist.");
        }

        return v.vfx[UnityEngine.Random.Range(0, v.vfx.Length)];
    }
}
