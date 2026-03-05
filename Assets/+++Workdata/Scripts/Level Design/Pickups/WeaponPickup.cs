using System;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public static event Action OnMachineGunCollected;
    public static event Action OnShotgunCollected;
    public static event Action<bool> OnPickupTrigger;
    public enum WeaponType { MachineGun, ShotGun }
    public WeaponType ammoType;
    public Animator doorAnimator;
    bool inPickup;


    public void Update()
    {
        if (inPickup && Input.GetKeyDown(KeyCode.E))
        {
            if (ammoType == WeaponType.MachineGun)
            {
                OnMachineGunCollected?.Invoke();

                if (doorAnimator != null)
                {
                    doorAnimator.Play("small_Grate_Open");
                    doorAnimator.Play("big_Grate_Open");
                }
            }
            else if (ammoType == WeaponType.ShotGun)
            {
                OnShotgunCollected?.Invoke();

                if (doorAnimator != null)
                {
                    doorAnimator.Play("small_Grate_Open");
                    doorAnimator.Play("big_Grate_Open");
                }
            }

            print("In pickup");
            PickUpTrigger(false);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inPickup = true;
            PickUpTrigger(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inPickup = false;
            PickUpTrigger(false);
        }
    }

    public void PickUpTrigger(bool toggle)
    {
        OnPickupTrigger?.Invoke(toggle);
    }
}
