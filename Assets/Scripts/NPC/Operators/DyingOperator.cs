using System;
using FluidHTN;
using FluidHTN.Operators;
using UnityEngine;

namespace Rioters.Operators
{
    public class DyingOperator : IOperator
    {
        public TaskStatus Update(IContext ctx)
        {
            if (ctx is NpcHtnContext c)
            {
                if (c.GenericTimer <= 0f)
                {
                    //Debug.Log("dying");
                    //c.anim.SetTrigger("takeDamage");
                    c.NavAgent.isStopped = true; //needed?
                    c.anim.SetTrigger("Dying");
                    //c.anim.SetBool("IsDying", true);
                    var clipInfo = c.anim.GetCurrentAnimatorClipInfo(0);
                    if (clipInfo.Length > 0)
                    {
                        var clip = clipInfo[0].clip;
                        c.GenericTimer = c.Time + clip.length;
                    }

                    return TaskStatus.Continue;
                }

                if (c.Time < c.GenericTimer)
                {
                    return TaskStatus.Continue;
                }

                c.GenericTimer = -1f;
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }

        public void Stop(IContext ctx)
        {
            if (ctx is NpcHtnContext c)
            {
                //c.NavAgent.isStopped = false; //needed?
                //c.anim.SetBool("IsDying", false);
                c.GenericTimer = -1f;
            }
        }
    }
}
