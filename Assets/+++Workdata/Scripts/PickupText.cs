using TMPro;
using UnityEngine;

public class PickupText : MonoBehaviour
{
    public TextMeshProUGUI pickupText;

    private void OnEnable()
    {
        Pickup.OnPickupTrigger += TogglePickupUI;
        WeaponPickup.OnPickupTrigger += TogglePickupUI;
    }

    private void OnDisable()
    {
        Pickup.OnPickupTrigger -= TogglePickupUI;
        WeaponPickup.OnPickupTrigger -= TogglePickupUI;
    }

    public void TogglePickupUI(bool toggle)
    {
        pickupText.gameObject.SetActive(toggle);
    }
}
