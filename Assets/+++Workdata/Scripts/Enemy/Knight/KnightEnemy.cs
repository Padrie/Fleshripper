using UnityEngine;

public class KnightEnemy : Enemy
{
    [Header("Knight Targeting")]
    public float dashDistance;

    [Header("Knight Movement")]
    public float dashSpeed;
    public float dashDuration;

    [Header("Knight Attack")]
    public float stompDuration;
    public GameObject stompSpell;

    public void Dash()
    {
        agent.velocity = DirectionTo(target.transform.position) * dashSpeed;
    }

    public void Stomp()
    {
        Spell.SpawnSpell(stompSpell, transform.position, transform.rotation);
    }
}