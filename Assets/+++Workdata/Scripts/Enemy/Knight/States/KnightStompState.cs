using ___Workdata.Scripts.Enemy.States;
using UnityEngine;

namespace ___Workdata.Scripts.Enemy.Knight.States
{
    public class KnightStompState : E_State
    {
        
        private KnightEnemy knight;
        private Player target;
        
        public override void Enter()
        {
            knight = (KnightEnemy)enemy;
            target = knight.target;

            if (!enemy.isDead)
                enemy.animator.Play("Stomp");

            Invoke(nameof(PerformStomp), knight.stompDuration);
        }

        public override void StateUpdate()
        {
            knight.agent.isStopped = true;
            
            knight.HandleRotation();
        }

        public void PerformStomp()
        {
            Debug.Log("Stomp");
            knight.agent.isStopped = false;
            knight.Stomp();
            End("EnemyIdleState");
        }
        
    }
}