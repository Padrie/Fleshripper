using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile")]
public class ProjectileStats : ScriptableObject
{
    public enum ProjectileType
    {
        Fast,
        Slow,
        Explosion
    }

    public ProjectileType projectileType;

    public int damage;
    public int penetration;
    public float projectileForce;
}
