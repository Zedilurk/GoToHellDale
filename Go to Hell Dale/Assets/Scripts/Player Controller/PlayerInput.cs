using UnityEngine;
using System.Collections;
using Luminosity.IO;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    Player player;
    int playerNum = 1;

    public enum ControllerTypeEnum { None, PS4, Xbox }

    void Start()
    {
        player = GetComponent<Player>();
        string[] joysticks = InputManager.GetJoystickNames();
    }

    bool _IsJumpAxisInUse = false;
    bool _IsReloadAxisInUse = false;
    bool _IsDashAxisInUse = false;

    public void SetPlayerNumber (int number)
    {
        playerNum = number;
    }

    void Update()
    {
        Vector2 directionalInput = new Vector2(InputManager.GetAxis("Horizontal"), InputManager.GetAxis("Vertical"));

        player.SetDirectionalInput(directionalInput);

        if (InputManager.GetButtonDown("Dash"))
            if (!_IsDashAxisInUse)
            {
                player.OnDashInputDown();
                _IsDashAxisInUse = true;
            }

        if (InputManager.GetButtonUp("Dash"))
            _IsDashAxisInUse = false;


        if (InputManager.GetButtonDown("Reload"))
            if (!_IsReloadAxisInUse)
            {
                player.OnReloadPantsInputDown();
                _IsReloadAxisInUse = true;
            }

        if (InputManager.GetButtonUp("Reload"))
            _IsReloadAxisInUse = false;

        if (InputManager.GetButtonDown("Jump"))
        {
            if (!_IsJumpAxisInUse)
            {
                player.OnJumpInputDown();
                _IsJumpAxisInUse = true;
            }
        }

        if (InputManager.GetButtonUp("Jump"))
        {
            player.OnJumpInputUp();
            _IsJumpAxisInUse = false;
        }
    }
}