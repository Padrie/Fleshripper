using UnityEngine;

public class FallState : P_State
{
    public override void Enter()
    {
        if (stateMachine.lastState == GetState("WalkingState") || stateMachine.lastState == GetState("DashState"))
        {
            player.canRecover = true;
            Invoke(nameof(recoverTimer), player.recoverTime);
        }
    }

    public override void StateUpdate()
    {
        if (player.isGrounded)
        {
            if (player.bufferJump)
            {
                End("JumpState");
            }
            else End("IdleState");

            return;
        }

        if (player.canRecover && Input.GetKeyDown(KeyCode.Space))
        {
            End("JumpState");
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && player.canDash && !player.noInput())
        {
            End("AirDashState");
        }
    }

    public void recoverTimer()
    {
        player.canRecover = false;
    }

    public override void StateFixedUpdate()
    {
        if (player.inputX != 0 || player.inputZ != 0)
        {
            player.HandleMovement();
        }
    }

    public override void Exit()
    {
        player.canRecover = false;
    }
}