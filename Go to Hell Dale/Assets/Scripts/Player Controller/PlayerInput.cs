using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    Player player;
    int playerNum = 1;

    public enum ControllerTypeEnum { None, PS4, Xbox }

    void Start()
    {
        player = GetComponent<Player>();
        string[] joysticks = Input.GetJoystickNames();
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
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal_" + playerNum), Input.GetAxisRaw("Vertical_" + playerNum));
        player.SetDirectionalInput(directionalInput);

        if (Input.GetAxisRaw("Dash_" + playerNum) != 0)
            if (!_IsDashAxisInUse)
            {
                player.OnDashInputDown();
                _IsDashAxisInUse = true;
            }

        if (Input.GetAxisRaw("Dash_" + playerNum) == 0)
            _IsDashAxisInUse = false;


        if (Input.GetAxisRaw("Reload_" + playerNum) != 0)
            if (!_IsReloadAxisInUse)
            {
                player.OnReloadPantsInputDown();
                _IsReloadAxisInUse = true;
            }

        if (Input.GetAxisRaw("Reload_" + playerNum) == 0)
            _IsReloadAxisInUse = false;


        if (Input.GetAxisRaw("Jump_" + playerNum) != 0)
            if (!_IsJumpAxisInUse)
            {
                player.OnJumpInputDown();
                _IsJumpAxisInUse = true;
            }                     

        if (Input.GetAxisRaw("Jump_" + playerNum) == 0)
        {
            player.OnJumpInputUp();
            _IsJumpAxisInUse = false;
        }
            
        /*
        if (Input.GetKeyDown(KeyCode.Space))
            player.OnJumpInputDown();

        if (Input.GetKeyUp(KeyCode.Space))      
            player.OnJumpInputUp();
        */
    }
}