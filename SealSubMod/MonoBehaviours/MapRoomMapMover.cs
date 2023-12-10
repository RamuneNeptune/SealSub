﻿using static SealSubMod.MonoBehaviours.Prefab.MapRoomFunctionalitySpawner;

namespace SealSubMod.MonoBehaviours;

public class MapRoomMapMover : MonoBehaviour, IInputHandler
{
    public static float MoveSpeed = 0.02f;
    public bool Active { get; private set; }
    internal MiniWorldPosition miniWorld;

    public void Toggle()
    {
        if (Active) Disable();
        else Enable();
    }

    public void Disable()
    {
        Active = false;

        Player.main.ExitLockedMode(false, false);
    }

    public void Enable()
    {
        Active = true;

        InputHandlerStack.main.Push(this);
        Player.main.EnterLockedMode(null, false);
    }

    public bool HandleInput()
    {
        var moveDirection = GameInput.GetMoveDirection();
        var direction = MainCamera.camera.transform.rotation * moveDirection;
        direction.y = 0;
        direction.Normalize();
        direction.y = moveDirection.y;

        miniWorld.offset += direction * MoveSpeed;

        if (GameInput.GetButtonDown(GameInput.Button.Exit)) Disable();

        return Active;
    }

    bool IInputHandler.HandleLateInput()
    {
        return true;
    }

    void IInputHandler.OnFocusChanged(InputFocusMode mode)
    {

    }
}
