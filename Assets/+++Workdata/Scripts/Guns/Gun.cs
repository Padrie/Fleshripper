using System;
using Unity.Cinemachine;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [HideInInspector] public GameObject normalProjectileType;
    [HideInInspector] public GameObject explosionProjectileType;
    [HideInInspector] public GameObject lightningExplosionType;
    [HideInInspector] public float projectileForce;

    public int level = 0;

    public GunStats[] stats;
    public ParticleSystem shootParticle;

    [Header("Gun Movement")]
    public float recoilKickBack = 0.1f;
    public float recoilRotationStrength = 1f;
    public float recoilDuration = 0.05f;
    [Range(0f, 1f)] public float swayAmount = 1f;

    [Header("Camera Shake")]
    public float shakeFrequency = 200f;
    public float shakeStrength = 0.35f;
    public float shakeDuration = 0.1f;

    [HideInInspector] public bool isReloading = false;
    [HideInInspector] public GunManager gunManager;
    [HideInInspector] public Player player;

    private void Awake()
    {
        if(GetComponentInParent<GunManager>() != null)
            gunManager = GetComponentInParent<GunManager>();

        player = FindAnyObjectByType<Player>();
    }
}
