using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public HealthBar healthBar;
    public DashMeter dashMeter;
    public AmmoUI ammoUi;
    public StatusScreen statusScreen;

    public void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        dashMeter = GetComponentInChildren<DashMeter>();
        ammoUi = GetComponentInChildren<AmmoUI>();
        statusScreen = GetComponentInChildren<StatusScreen>();

        if (TutorialManager.machineGunUnlocked) return;

        healthBar.gameObject.SetActive(false);
        dashMeter.gameObject.SetActive(false);
        //ammoUi.gameObject.SetActive(false);
        statusScreen.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        WeaponPickup.OnMachineGunCollected += SetUIActive;
    }

    private void OnDisable()
    {
        WeaponPickup.OnMachineGunCollected -= SetUIActive;
    }

    void SetUIActive()
    {
        healthBar.gameObject.SetActive(true);
        dashMeter.gameObject.SetActive(true);
        ammoUi.gameObject.SetActive(true);
        statusScreen.gameObject.SetActive(true);
    }
}
