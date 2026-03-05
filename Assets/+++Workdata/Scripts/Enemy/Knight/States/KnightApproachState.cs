using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(KnightEnemy))]
public class KnightApproachState : E_State
{
    private KnightEnemy knight;
    private Player target;
    
    public override void Enter()
    {
        knight = (KnightEnemy)enemy;
        if(!enemy.isDead)
            enemy.animator.Play("Walk");
        target = knight.target;
    }

    public override void StateUpdate()
    {
        enemy.Approach();

        if (knight.DistanceTo(target.transform.position) > knight.dashDistance)
        {
            End("KnightDashState");
            return;
        }

        if (!knight.targetInRange)
        {
            End("EnemyIdleState");
            return;
        }
    }
}