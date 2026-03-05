using UnityEngine;

public class KnightDashState : E_State
{
    private KnightEnemy knight;
    private Player target;

    public override void Enter()
    {
        knight = (KnightEnemy)enemy;
        target = knight.target;

        knight.Dash();

        Invoke(nameof(ResetDash), knight.dashDuration);
    }

    public void ResetDash()
    {

        if (knight.DistanceTo(target.transform.position) <= knight.attackRange * 2)
        {
            End("KnightStompState");
            return;
        }
        
        End("EnemyIdleState");
    }
}