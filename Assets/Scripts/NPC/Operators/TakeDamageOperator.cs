using System;
using FluidHTN;
using FluidHTN.Operators;
using UnityEngine;

namespace Rioters.Operators
{
    public class TakeDamageOperator : IOperator
    {
        //Look at fluid htn bridge troll example
        
        private float _oldHealth;   //how to get?
        private float _CurrentHealth;
        private bool _deathFlag;

        public TaskStatus Update(IContext ctx)
        {
            if (ctx is NpcHtnContext c)
            {
                //...
            }
            return TaskStatus.Failure;
        }

        public void Stop(IContext ctx)
        {
            if (ctx is NpcHtnContext c)
            {
                c.NavAgent.isStopped = true;
                //c.anim.SetBool("IsRunning", false);
            }
        }
    }
}