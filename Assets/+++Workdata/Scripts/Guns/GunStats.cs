using UnityEngine;

[CreateAssetMenu(fileName = "Gun Stats")]

public class GunStats : ScriptableObject
{
    public int ammoAmount = 30;
    public float reloadTime = 1f;
    [Range(0f, 1f)] public float bulletSpread = 0.05f;
    public float shootSpeed = 0.1f;
    public int bulletAmount = 1;
}
