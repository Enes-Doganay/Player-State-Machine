using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerAbilityState
{
    public bool CanDodge { get; private set; }
    private bool isHolding;
    private bool dashInputStop;
    private Vector2 dodgeDirection;
    private float lastDodgeTime;
    private Vector3 asd;

    public PlayerDodgeState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        CanDodge = false;
        dodgeDirection = Vector2.right * player.FacingDirection;
        player.InputHandler.UseDodgeInput();
        startTime = Time.time;
        asd = player.transform.eulerAngles;
        player.transform.Rotate(0, player.transform.rotation.y, 90);
        player.SetColliderSize(playerData.dodgeColliderSizeX,playerData.dodgeColliderSizeY);
        player.SetGravity(20f);
    }
    public override void Exit()
    {
        base.Exit();
        player.transform.eulerAngles = asd;
        player.SetColliderSize(playerData.standColliderSizeX,playerData.standColliderSizeY);
        player.SetGravity(1f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!isExitingState)
        {
            if (isHolding)
            {
                dashInputStop = player.InputHandler.DashInputStop;
                if (dashInputStop || Time.time >= startTime + playerData.dodgeMaxHoldTime)
                {
                    isHolding = false;
                    startTime = Time.time;
                    player.SetVelocity(playerData.dodgeVelocity, dodgeDirection);
                }
            }
            else
            {
                player.SetVelocity(playerData.dodgeVelocity, dodgeDirection);
                if(Time.time >= startTime + playerData.dodgeTime)
                {
                    isAbilityDone = true;
                    lastDodgeTime = Time.time;
                }
            }
        }
    }

    public bool CheckIfCanDodge()
    {
        return CanDodge && Time.time >= lastDodgeTime + playerData.dodgeCooldown;
    }
    public void ResetCanDodge() => CanDodge = true;

}
