using System;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{

    public float lifetime;
    public float maxLifetime;
    
    public int damage;

    private void Start()
    {
        OnSpawn();
    }

    private void OnDestroy()
    {
        OnDeath();
    }

    public void Update()
    {
        UpdateSpell();
        
        UpdateLifetime();
    }

    public void UpdateLifetime()
    {
        lifetime += Time.deltaTime;

        if (lifetime >= maxLifetime)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            OnPlayerCollision(other.gameObject.GetComponent<Player>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            OnPlayerCollision(other.gameObject.GetComponent<Player>());
        }
    }

    public virtual void OnSpawn() {}
    
    public virtual void UpdateSpell() {}
    
    public virtual void OnDeath() {}
    
    public virtual void OnPlayerCollision(Player player) {}

    public static Spell SpawnSpell(GameObject spell , Vector3 position , Quaternion rotation)
    {
        return Instantiate(spell , position , rotation).gameObject.GetComponent<Spell>();
    }

}
