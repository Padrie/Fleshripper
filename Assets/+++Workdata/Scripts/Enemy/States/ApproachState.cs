using ___Workdata.Scripts.StateMachine;

public class ApproachState : E_State
{
    public E_State attackState;

    public override void StateUpdate()
    {
        enemy.Approach();

        if (!enemy.targetInRange)
        {
            End("EnemyIdleState");
        }
    }
}