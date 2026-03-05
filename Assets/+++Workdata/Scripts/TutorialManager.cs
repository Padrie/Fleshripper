using Unity.VisualScripting;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static bool machineGunUnlocked = false;
    public static bool shotgunUnlocked = false;
    public static bool explosionPickupUnlocked = false;
    public static bool lightningPickupUnlocked = false;

    public bool cheatMode = false;

    public static TutorialManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        if (cheatMode)
        {
            machineGunUnlocked = true;
            shotgunUnlocked = true;
            explosionPickupUnlocked = true;
            lightningPickupUnlocked = true;
        }

        DontDestroyOnLoad(gameObject);
    }


    private void Update()
    {
        if (cheatMode)
        {
            machineGunUnlocked = true;
            shotgunUnlocked = true;
            explosionPickupUnlocked = true;
            lightningPickupUnlocked = true;
        }
    }

    private void OnEnable()
    {
        WeaponPickup.OnMachineGunCollected += MachineGunCollected;
        WeaponPickup.OnShotgunCollected += ShotgunCollected;
    }

    private void OnDisable()
    {
        WeaponPickup.OnMachineGunCollected -= MachineGunCollected;
        WeaponPickup.OnShotgunCollected -= ShotgunCollected;
    }

    public void MachineGunCollected()
    {
        machineGunUnlocked = true;
    }

    public void ShotgunCollected()
    {
        shotgunUnlocked = true;
    }
}
