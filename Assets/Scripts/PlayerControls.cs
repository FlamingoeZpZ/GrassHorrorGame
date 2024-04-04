using UnityEngine;

public static class PlayerControls
{
    private static ImSoWet _wetness;
    
    public static void Init(Player p)
    {
        _wetness = new ImSoWet();

        _wetness.Socks.Move.performed += x => p.SetMoveDirection(x.ReadValue<Vector3>());
        _wetness.Socks.Look.performed += x => p.MoveCamera(x.ReadValue<Vector2>());
        _wetness.Socks.Water.performed += x => p.SetWateringState(x.ReadValueAsButton());
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        EnableGameControls();
    }

    public static void EnableGameControls()
    {
        _wetness.Socks.Enable();
    }
}
