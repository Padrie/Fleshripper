

public class AirDashState : P_State
{
    public override void Enter()
    {
        player.speedFov *= 4;
        player.HandleAirDash();
        player.noGravity = true;

        Invoke(nameof(ResetDash), player.airDashTime);
    }

    public override void StateUpdate()
    {
        if (player.isGrounded)
        {
            End("IdleState");
            return;
        }
    }

    public override void StateFixedUpdate()
    {
        if (player.inputX != 0 || player.inputZ != 0)
        {
            player.HandleMovement();
        }
    }

    public void ResetDash()
    {
        End("FallState");
    }

    public override void Exit()
    {
        player.noGravity = false;
        player.speedFov /= 4;
    }
}