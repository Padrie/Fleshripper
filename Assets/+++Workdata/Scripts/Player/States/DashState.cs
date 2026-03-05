using UnityEngine;

public class DashState : P_State
{
    public override void Enter()
    {
        player.HandleDash();
        player.speedFov *= 2;

        Invoke(nameof(ResetDash), player.dashTime);
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && player.isGrounded && player.stamina != 0)
        {
            player.velocity.x *= 0.75f;
            player.velocity.z *= 0.75f;

            End("JumpDashState");
            player.canDash = true;
            return;
        }
    }

    public void ResetDash()
    {
        if (player.isGrounded)
        {
            End("IdleState");
        }
        else End("FallState");
    }

    public override void Exit()
    {
        player.speedFov /= 2;
    }
}