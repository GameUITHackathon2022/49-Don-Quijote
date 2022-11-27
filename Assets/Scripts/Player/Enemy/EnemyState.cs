using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleFoxLite
{
    public abstract class BaseStateEnemy
    {
        public abstract void EnterState(PlayerController player);
        public abstract void UpdateState(PlayerController player);
        public abstract void ExitState(PlayerController player);
    }
    public class NormalStateEnemy : BaseState
    {
        public override void EnterState(PlayerController player)
        {



        }
        public override void UpdateState(PlayerController player)
        {

        }
        public override void ExitState(PlayerController player)
        {



        }
    }
    public class AttckEnemy : BaseState
    {
        public override void EnterState(PlayerController player)
        {

        }
        public override void UpdateState(PlayerController player)
        {

        }
        public override void ExitState(PlayerController player)
        {


        }
    }
}
