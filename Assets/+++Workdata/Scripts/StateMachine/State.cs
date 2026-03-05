using System;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace ___Workdata.Scripts.StateMachine
{ 
    [RequireComponent(typeof(StateMachine))]
    public abstract class State : MonoBehaviour
    {
        protected State nextState;
        protected bool endCalled;
        protected StateMachine stateMachine;  

        public virtual void Enter() {}

        public virtual void Initialize() { }
    
        public virtual void StateUpdate() { }
    
        public virtual void StateFixedUpdate() { }
    
        public virtual void Exit() {}
        
        public void End(State state)
        {
            endCalled = true;
    
            if (nextState == null)
            {
                if (state == null)
                {
                    throw new ArgumentNullException("State");
                }
                    
                nextState = state;
                Exit();
            }
        }
        
        public void End(string state)
        {
            endCalled = true;
            State nState = GetState(state);
    
            if (nextState == null)
            {
                if (state == null)
                {
                    throw new ArgumentNullException("State");
                }
                
                nextState = nState;
                Exit();
            }
        }
        
        public State GetState(string statename)
        {

            if (stateMachine.States.Find(state => state.ToString() == statename) == null)
            {
                throw new Exception("Cannot find state " + statename);
                return null;
            }
            
            return stateMachine.States.Find(state => state.ToString() == statename);
        }
    
        public void Reset()
        {
            nextState = null;
            endCalled = false;
        }

        public State getNextState()
        {
            return nextState;
        }
    
        public bool getEndCalled() 
        {
            return endCalled;
        }

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}

