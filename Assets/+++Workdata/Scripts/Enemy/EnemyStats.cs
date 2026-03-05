using System;

[Serializable]
public class EnemyStats
{
    public float Health;
    public int Damage;
    public int AttackSpeed;
    public int Speed;
    
    //Default Stats
    private float d_Health;
    private int d_Damage;
    private int d_AttackSpeed;
    private int d_Speed;


    public EnemyStats(float health, int damage, int attackSpeed, int speed)
    {
        Health = health;
        Damage = damage;
        AttackSpeed = attackSpeed;
        Speed = speed;
        
        d_Health = health;
        d_Damage = damage;
        d_AttackSpeed = attackSpeed;
        d_Speed = speed;
        
    }

    public void Reset()
    {
        Health = d_Health;
        Damage = d_Damage;
        AttackSpeed = d_AttackSpeed;
        Speed = d_Speed;
    }
}