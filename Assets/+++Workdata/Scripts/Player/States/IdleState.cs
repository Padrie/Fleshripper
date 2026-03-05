using UnityEngine;

public class IdleState : P_State
{
    public override void StateUpdate()
    {
        if (player.inputX != 0 || player.inputZ != 0)
        {
            End("WalkingState");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.isGrounded)
        {
            End("JumpState");
            return;
        }
    }
}