using UnityEngine;

public class WalkingState : P_State
{
    public override void StateUpdate()
    {
        if (player.inputX == 0 && player.inputZ == 0)
        {
            End("IdleState");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) || player.bufferJump && player.isGrounded)
        {
            End("JumpState");
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && player.canDash)
        {
            End("DashState");
            return;
        }

        if (player.velocity.y < 0 && !player.isGrounded)
        {
            End("FallState");
            return;
        }
    }

    public override void StateFixedUpdate()
    {
        player.HandleMovement();
    }
}