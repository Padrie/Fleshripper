using System.Collections;
using ___Workdata.Scripts.Entity;
using UnityEngine;

namespace ___Workdata.Scripts.Enemy
{
    public interface IEnemy : IEntity , IDamagable
    {
        public void SearchWalkPoint();
        public void CheckTarget();
        public void HandleRotation();
        public void Idle();
        public void Approach();
        public void Attack();
        public IEnumerator Contact();
        public void Death();
    }
}