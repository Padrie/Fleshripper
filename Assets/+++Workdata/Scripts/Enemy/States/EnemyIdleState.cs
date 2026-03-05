using UnityEngine;

public class EnemyIdleState : E_State
{
    public E_State spotState;

    public override void StateUpdate()
    {
        enemy.Idle();

        if(!enemy.isDead)
            enemy.animator.Play("Idle");

        if (enemy.targetInRange)
        {
            End(spotState);
            if (!enemy.isDead)
                enemy.animator.Play("Walk");
        }
    }
}