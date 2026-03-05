using System;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] Shotgun shotgun;
    [SerializeField] SuperShotgun superShotgun;
    [SerializeField] MachineGun machineGun;
    [Space(15)]
    public int currentAmmoAmount;
    public GameObject normalAmmo;
    [Space(15)]
    public GameObject lightningAmmo;
    public int currentLightningAmmoAmount;
    public int maxLightningAmmoAmount;
    public bool lightningAmmoUnlocked = false;
    [Space(15)]
    public GameObject explosiveAmmo;
    public int currentExplosionAmmoAmount;
    public int maxExplosionAmmoAmount;
    public bool explosionAmmoUnlocked = false;
    [HideInInspector] public int ammoSlot = 1;

    [HideInInspector] public bool machineGunUnlocked = false;
    [HideInInspector] public bool shotgunUnlocked = false;

    private void Awake()
    {
        machineGun.gameObject.SetActive(false);
        shotgun.gameObject.SetActive(false);
    }
    private void Start()
    {
        machineGun.normalProjectileType = normalAmmo;
        machineGun.explosionProjectileType = explosiveAmmo;
        machineGun.lightningExplosionType = lightningAmmo;

        shotgun.normalProjectileType = normalAmmo;
        shotgun.explosionProjectileType = explosiveAmmo;
        shotgun.lightningExplosionType = lightningAmmo;

        superShotgun.normalProjectileType = normalAmmo;
        superShotgun.explosionProjectileType = explosiveAmmo;
        superShotgun.lightningExplosionType = lightningAmmo;


        if (TutorialManager.machineGunUnlocked == true)
        {
            MachineGunCollected();
        }

        if (TutorialManager.shotgunUnlocked == true)
        {
            ShotgunCollected();
        }

        AmmoUI.gun = machineGun;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (AmmoUI.gun == machineGun && TutorialManager.shotgunUnlocked && !AmmoUI.gun.isReloading)
            {
                machineGun.gameObject.SetActive(false);
                superShotgun.gameObject.SetActive(true);

                AmmoUI.gun = superShotgun;
            }
            else if (AmmoUI.gun == superShotgun && TutorialManager.machineGunUnlocked && !AmmoUI.gun.isReloading)
            {
                machineGun.gameObject.SetActive(true);
                superShotgun.gameObject.SetActive(false);

                AmmoUI.gun = machineGun;
            }
        }
    }

    private void OnEnable()
    {
        Pickup.OnLightningPickupCollected += AddLightningAmmo;
        Pickup.OnExplosionPickupCollected += AddExplosionAmmo;
        WeaponPickup.OnMachineGunCollected += MachineGunCollected;
        WeaponPickup.OnShotgunCollected += ShotgunCollected;
    }

    private void OnDisable()
    {
        Pickup.OnLightningPickupCollected -= AddLightningAmmo;
        Pickup.OnExplosionPickupCollected -= AddExplosionAmmo;
        WeaponPickup.OnMachineGunCollected -= MachineGunCollected;
        WeaponPickup.OnShotgunCollected -= ShotgunCollected;
    }

    public void MachineGunCollected()
    {
        machineGunUnlocked = true;
        machineGun.gameObject.SetActive(true);
        TutorialManager.Instance.MachineGunCollected();
    }

    public void ShotgunCollected()
    {
        shotgunUnlocked = true;
        TutorialManager.Instance.ShotgunCollected();
    }

    public void AddLightningAmmo(int amount)
    {
        maxLightningAmmoAmount += amount;
        EvaluateLightningAmmo();
        lightningAmmoUnlocked = true;
    }

    public void AddExplosionAmmo(int amount)
    {
        maxExplosionAmmoAmount += amount;
        EvaluateExplosionAmmo();
        explosionAmmoUnlocked = true;
    }

    public void EvaluateExplosionAmmo()
    {
        if (currentExplosionAmmoAmount > 0) return;

        currentExplosionAmmoAmount = maxExplosionAmmoAmount;
        currentExplosionAmmoAmount = Mathf.Clamp(currentExplosionAmmoAmount, 0, machineGun.stats[0].ammoAmount);
        maxExplosionAmmoAmount -= currentExplosionAmmoAmount;
    }

    public void EvaluateLightningAmmo()
    {
        if (currentLightningAmmoAmount > 0) return;

        currentLightningAmmoAmount = maxLightningAmmoAmount;
        currentLightningAmmoAmount = Mathf.Clamp(currentLightningAmmoAmount, 0, machineGun.stats[0].ammoAmount);
        maxLightningAmmoAmount -= currentLightningAmmoAmount;
    }
}
