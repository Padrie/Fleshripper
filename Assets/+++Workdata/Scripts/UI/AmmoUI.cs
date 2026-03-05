using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] TextMeshProUGUI lightningAmmoText;
    [SerializeField] TextMeshProUGUI exploAmmoText;
    [SerializeField] TextMeshProUGUI pressButtonToPickup;
    [SerializeField] Image progressBar;

    [Space(15)]
    [SerializeField] GameObject smallDefaultIcon;
    [SerializeField] GameObject smallLightningIcon;
    [SerializeField] GameObject smallExplosionIcon;

    [SerializeField] GameObject smallDefaultIconGreyOut;
    [SerializeField] GameObject smallLightningIconGreyOut;
    [SerializeField] GameObject smallExplosionIconGreyOut;

    [Space(15)]
    [SerializeField] GameObject bigDefaultIcon;
    [SerializeField] GameObject bigLightningIcon;
    [SerializeField] GameObject bigExplosionIcon;

    [Space(15)]
    [SerializeField] GameObject overlayNoWeaponSwitch;
    [SerializeField] GameObject overlayWithWeaponSwitch;
    [SerializeField] GameObject overlayWithWeaponSwitchAndAmmoType1;
    [SerializeField] GameObject overlayWithWeaponSwitchAndAmmoType2;

    [HideInInspector] public static Gun gun;
    [HideInInspector] public GunManager gunManager;

    bool progressBarFill = false;

    bool shotgunCollected = false;
    bool machineGunCollected = false;
    bool firstCollectable = false;

    private void Awake()
    {
        gun = FindAnyObjectByType<Gun>();
        gunManager = FindAnyObjectByType<GunManager>();
        pressButtonToPickup.gameObject.SetActive(false);

        smallDefaultIcon.SetActive(false);
        smallLightningIcon.SetActive(false);
        smallExplosionIcon.SetActive(false);

        smallDefaultIconGreyOut.SetActive(false);
        smallExplosionIconGreyOut.SetActive(false);
        smallLightningIconGreyOut.SetActive(false);

        bigDefaultIcon.SetActive(false);

        overlayNoWeaponSwitch.gameObject.SetActive(false);
        overlayWithWeaponSwitch.gameObject.SetActive(false);
        overlayWithWeaponSwitchAndAmmoType1.SetActive(false);
        overlayWithWeaponSwitchAndAmmoType2.SetActive(false);
    }

    private void Start()
    {
        if (TutorialManager.shotgunUnlocked == true && !firstCollectable)
        {
            overlayWithWeaponSwitch.gameObject.SetActive(true);
        }

        if (TutorialManager.explosionPickupUnlocked && !TutorialManager.lightningPickupUnlocked)
        {
            overlayNoWeaponSwitch.gameObject.SetActive(false);
            overlayWithWeaponSwitch.gameObject.SetActive(false);
            overlayWithWeaponSwitchAndAmmoType1.SetActive(true);
            overlayWithWeaponSwitchAndAmmoType2.gameObject.SetActive(false);

            smallDefaultIcon.SetActive(true);
            smallExplosionIcon.SetActive(true);
        }
        else if (TutorialManager.explosionPickupUnlocked && TutorialManager.lightningPickupUnlocked)
        {
            overlayNoWeaponSwitch.gameObject.SetActive(false);
            overlayWithWeaponSwitch.gameObject.SetActive(false);
            overlayWithWeaponSwitchAndAmmoType1.SetActive(false);
            overlayWithWeaponSwitchAndAmmoType2.gameObject.SetActive(true);

            smallDefaultIcon.SetActive(true);
            smallExplosionIcon.SetActive(true);
            smallLightningIcon.SetActive(true);
        }
    }

    private void Update()
    {
        UpdateCurrentAmmo();

        if (gunManager.currentAmmoAmount == 0 && gun.isReloading && !progressBarFill)
        {
            //StartCoroutine(fillProgressBar());
        }

        if (gunManager.ammoSlot == 1)
        {
            if (TutorialManager.machineGunUnlocked)
                bigDefaultIcon.SetActive(true);
            bigExplosionIcon.SetActive(false);
            bigLightningIcon.SetActive(false);

            if (TutorialManager.explosionPickupUnlocked)
                smallDefaultIcon.SetActive(true);

            if (TutorialManager.explosionPickupUnlocked)
            {
                smallDefaultIconGreyOut.SetActive(false);
                smallExplosionIconGreyOut.SetActive(true);

                if (TutorialManager.lightningPickupUnlocked)
                    smallLightningIconGreyOut.SetActive(true);
            }
        }
        if (gunManager.ammoSlot == 2)
        {
            bigDefaultIcon.SetActive(false);
            bigExplosionIcon.SetActive(true);
            bigLightningIcon.SetActive(false);

            if (TutorialManager.explosionPickupUnlocked)
            {
                smallDefaultIconGreyOut.SetActive(true);
                smallExplosionIconGreyOut.SetActive(false);

                if (TutorialManager.lightningPickupUnlocked)
                    smallLightningIconGreyOut.SetActive(true);
            }
        }
        if (gunManager.ammoSlot == 3)
        {
            bigDefaultIcon.SetActive(false);
            bigExplosionIcon.SetActive(false);
            bigLightningIcon.SetActive(true);

            if (TutorialManager.lightningPickupUnlocked)
            {
                smallDefaultIconGreyOut.SetActive(true);
                smallExplosionIconGreyOut.SetActive(true);
                smallLightningIconGreyOut.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        Pickup.OnPickupTrigger += TogglePickupUI;
        WeaponPickup.OnPickupTrigger += TogglePickupUI;
        WeaponPickup.OnMachineGunCollected += MachineGunCollected;
        WeaponPickup.OnShotgunCollected += ShotgunCollected;
    }

    private void OnDisable()
    {
        Pickup.OnPickupTrigger -= TogglePickupUI;
        WeaponPickup.OnPickupTrigger -= TogglePickupUI;
        WeaponPickup.OnMachineGunCollected -= MachineGunCollected;
        WeaponPickup.OnShotgunCollected -= ShotgunCollected;
    }

    public void TogglePickupUI(bool toggle)
    {
        firstCollectable = true;

        if (gunManager.explosionAmmoUnlocked)
        {
            smallExplosionIcon.SetActive(true);
            TutorialManager.explosionPickupUnlocked = true;
            overlayWithWeaponSwitch.gameObject.SetActive(false);
            overlayWithWeaponSwitchAndAmmoType1.SetActive(true);
        }
        if (gunManager.lightningAmmoUnlocked)
        {
            smallLightningIcon.SetActive(true);
            TutorialManager.lightningPickupUnlocked = true;
            overlayWithWeaponSwitchAndAmmoType1.SetActive(false);
            overlayWithWeaponSwitchAndAmmoType2.SetActive(true);
        }
    }

    public void UpdateCurrentAmmo()
    {
        ammoText.text = $"{gunManager.currentAmmoAmount}/{gun.stats[gun.level].ammoAmount}";
        lightningAmmoText.text = $"{gunManager.currentLightningAmmoAmount}/{gunManager.maxLightningAmmoAmount}";
        exploAmmoText.text = $"{gunManager.currentExplosionAmmoAmount}/{gunManager.maxExplosionAmmoAmount}";
    }

    private void MachineGunCollected()
    {
        TutorialManager.Instance.MachineGunCollected();
        overlayNoWeaponSwitch.SetActive(true);
        print("ENABLE THE FUCKING OVERLAY");
        machineGunCollected = true;
    }

    private void ShotgunCollected()
    {
        overlayNoWeaponSwitch.SetActive(false);
        overlayWithWeaponSwitch.SetActive(true);
        shotgunCollected = true;
    }

    IEnumerator fillProgressBar()
    {
        progressBarFill = true;
        float elapsed = 0f;
        float normalizedTime = 0f;

        progressBar.fillAmount = 0f;
        progressBar.color = Color.HSVToRGB(0.51f, 1f, .1f);

        while (elapsed < gun.stats[gun.level].reloadTime)
        {
            elapsed += Time.deltaTime;
            normalizedTime = Mathf.Clamp01(elapsed / gun.stats[gun.level].reloadTime);
            progressBar.color = Color.HSVToRGB(.51f, 1f, normalizedTime);
            progressBar.fillAmount = normalizedTime;
            yield return null;
        }

        progressBar.fillAmount = 1f;
        progressBar.color = Color.HSVToRGB(.51f, 1f, 1f);
        progressBarFill = false;
    }

}
