using System;
using ___Workdata.Scripts.StateMachine;


public class P_State : State
{
    protected Player player;

    private void Start()
    {
        player = GetComponent<Player>();
        stateMachine = GetComponent<StateMachine>();
    }
}