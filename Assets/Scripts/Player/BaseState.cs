using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleFoxLite
{
    public abstract class BaseState
    {
        public abstract void EnterState(PlayerController player);
        public abstract void UpdateState(PlayerController player);
        public abstract void ExitState(PlayerController player);
    }
    public class EventState : BaseState
    {
        public override void EnterState(PlayerController player)
        {
            if (player.playerAnimator != null)
                player.ChangeAnimatorState(player.aniStateEvent, true);

        }
        public override void UpdateState(PlayerController player)
        {

        }
        public override void ExitState(PlayerController player)
        {
            player.ChangeAnimatorState(player.aniStateEvent, false);

        }
    }
    public class NormalState : BaseState
    {
        public override void EnterState(PlayerController player)
        {
            player.jumpAction += player.HandleJump;
            if (player.playerAnimator != null)
                player.ChangeAnimatorState(player.aniStateNormal, true);


        }
        public override void UpdateState(PlayerController player)
        {
            player.HandleMovement();
        }
        public override void ExitState(PlayerController player)
        {
            player.jumpAction -= player.HandleJump;
            player.ChangeAnimatorState(player.aniStateNormal, false);

        }
    }
    public class ShootState : BaseState
    {
        public override void EnterState(PlayerController player)
        {
            if (player.playerAnimator != null)
            player.ChangeAnimatorState(player.aniStateShoot, true);
            player.ShowGun();
            player.jumpAction += player.HandleJumpInShoot;
        }
        public override void UpdateState(PlayerController player)
        {
            player.HandleMovementShoot();
            player.Shoot();
        }
        public override void ExitState(PlayerController player)
        {
            player.StartCoroutine(player.HideGun());
            player.jumpAction -= player.HandleJumpInShoot;
            player.ChangeAnimatorState(player.aniStateShoot, false);


        }
    }
}
