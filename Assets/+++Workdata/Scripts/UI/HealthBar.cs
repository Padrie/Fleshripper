using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image healthBar;
    Player player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    
    private void Update()
    {
        healthBar.fillAmount = Mathf.InverseLerp(0, player.maxHealth, player.currentHealth);
    }
}
