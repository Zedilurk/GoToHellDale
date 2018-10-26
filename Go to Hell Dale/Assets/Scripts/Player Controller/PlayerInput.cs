using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{

    Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    bool _IsJumpAxisInUse = false;
    bool _IsReloadAxisInUse = false;
    bool _IsDashAxisInUse = false;
    void Update()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.SetDirectionalInput(directionalInput);

        if (Input.GetAxisRaw("Dash") != 0)
            if (!_IsDashAxisInUse)
            {
                player.OnDashInputDown();
                _IsDashAxisInUse = true;
            }

        if (Input.GetAxisRaw("Dash") == 0)
            _IsDashAxisInUse = false;


        if (Input.GetAxisRaw("Reload") != 0)
            if (!_IsReloadAxisInUse)
            {
                player.OnReloadPantsInputDown();
                _IsReloadAxisInUse = true;
            }

        if (Input.GetAxisRaw("Reload") == 0)
            _IsReloadAxisInUse = false;


        if (Input.GetAxisRaw("Jump") != 0)
            if (!_IsJumpAxisInUse)
            {
                player.OnJumpInputDown();
                _IsJumpAxisInUse = true;
            }                     

        if (Input.GetAxisRaw("Jump") == 0)
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