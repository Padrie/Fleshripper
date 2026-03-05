using UnityEngine;


public class JumpDashState : P_State
{
    public override void Enter()
    {
        player.HandleJump(true);
    }

    public override void StateUpdate()
    {
        if (player.velocity.y < 0)
        {
            End("FallState");
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && player.canDash && !player.noInput())
        {
            End("AirDashState");
            return;
        }
    }

    public override void StateFixedUpdate()
    {
        if (player.inputX != 0 || player.inputZ != 0)
        {
            player.HandleMomentum();
        }
    }
}