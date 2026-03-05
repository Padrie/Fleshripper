using System;
using ___Workdata.Scripts.Enemy;
using ___Workdata.Scripts.StateMachine;


public class E_State : State
{
    protected Enemy enemy;

    private void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
        stateMachine = gameObject.GetComponent<StateMachine>();
    }
}