using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ___Workdata.Scripts.StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        
        public State currentState;
        public State lastState;
        public List<State> States;
        
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected void Start()
        {
            currentState.Enter();
            
            States.AddRange(GetComponents<State>());
            
            Initalize();
        }
        virtual internal void Initalize() {}
        

        // Update is called once per frame
        protected void Update()
        {
            currentState.StateUpdate();
        }

        protected void FixedUpdate()
        {
            currentState.StateFixedUpdate();
        }

        protected void LateUpdate()
        {
            CheckEnd();
        }
        
        //Kontrolliert ob der state vorbei ist wenn dies der fall ist wird er beendet und es wird auf den nächsten übergegriffen und der alte wird gespeichert
        public void CheckEnd()
        {
            if (currentState.getNextState() != null || currentState.getEndCalled())
            {
                lastState = currentState;
                currentState = currentState.getNextState();
                
                currentState.Enter();
                currentState.Reset();
               
            }
        }
        
    }
}