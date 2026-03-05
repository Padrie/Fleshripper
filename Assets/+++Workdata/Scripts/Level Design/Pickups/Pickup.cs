using System;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public static event Action<int> OnLightningPickupCollected;
    public static event Action<int> OnExplosionPickupCollected;
    public static event Action<bool> OnPickupTrigger;
    public enum AmmoType { Lightning, Explosion }
    public AmmoType ammoType;
    public int replenishAmount = 30;
    bool inPickup;


    public void Update()
    {
        if (inPickup && Input.GetKeyDown(KeyCode.E))
        {
            if (ammoType == AmmoType.Lightning)
            {
                OnLightningPickup(replenishAmount);
            }
            else if (ammoType == AmmoType.Explosion)
            {
                OnExplosionPickup(replenishAmount);
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

    public void OnLightningPickup(int amount)
    {
        OnLightningPickupCollected?.Invoke(amount);
    }

    public void OnExplosionPickup(int amount)
    {
        OnExplosionPickupCollected?.Invoke(amount);
    }

    public void PickUpTrigger(bool toggle)
    {
        OnPickupTrigger?.Invoke(toggle);
    }
}
