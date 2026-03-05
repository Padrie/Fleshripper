using UnityEngine;

public enum BulletEffectType
{
    None,
    Normal,
    Explosion,
    Lightning,
}

public class Bullet : MonoBehaviour
{
    public BulletEffectType bulletEffectType = BulletEffectType.Normal;
    public ScriptableObject effectAsset;

    public float bulletSpeed = 1000f;
    public float bulletDamage = 50f;
    public float bulletLifeTime = 2f;
    public double penetration; // :)

    public ParticleSystem spawnEffect;

    private IBulletEffect bulletEffect;
    private int currentPenetration;

    private Rigidbody rb;
    private Vector3 bulletDir = Vector3.zero;

    private void Awake()
    {
        bulletEffect = effectAsset as IBulletEffect;
        currentPenetration = (int)penetration;

        Invoke("Disappear", bulletLifeTime);
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            Disappear();
        }

        if (other.CompareTag("Enemy"))
        {
            if (currentPenetration <= 0)
            {
                Disappear();
            }

            //if (currentFork)
            //{
            //    for (int i = 0; i < bulletForkAmount; i++)
            //    {
            //        print("A");
            //        var newBullet = Instantiate(gameObject, transform.position, Quaternion.identity);

            //        newBullet.GetComponent<Bullet>().AddVelocity(bulletDir + Random.insideUnitSphere);
            //    }
            //}

            if (other.TryGetComponent<IDamagable>(out var damagable))
                damagable.TakeDamage(bulletDamage);


            bulletEffect?.ApplyEffect(this, other.gameObject);

            currentPenetration--;
        }
    }

    public void AddVelocity(Vector3 velocity)
    {
        bulletDir = velocity;
        rb.AddForce(bulletDir * bulletSpeed);
    }

    public void Disappear()
    {
        Destroy(gameObject);
    }
}