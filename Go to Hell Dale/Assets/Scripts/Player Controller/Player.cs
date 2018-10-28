using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    public enum PlayerStateEnum
    {
        Respawning = 0,
        Idle = 1,
        Running = 2,
        Jumping = 4,
        WallSlide = 8,
        SlodeSlide = 16,
        PartialReload = 32,
        FullReload = 64,
        FailedReload = 128,
        Dead = 256,
        PantsDown = 512
    };

    #region Player Movement Variables
    [SerializeField]
    private PlayerStateEnum _PlayerState = PlayerStateEnum.Respawning;
    public PlayerStateEnum PlayerState
    {
        get { return _PlayerState; }
        set
        {
            _PlayerState = value;
            PlayerStateChanged(_PlayerState);            
        }
    }

    public float MaxJumpHeight = 25;
    public float MinJumpHeight = 12;
    public float TimeToJumpApex = .4f;
    float _AccelerationTimeAirborne = .2f;
    float _AccelerationTimeGrounded = .1f;
    public float MoveSpeed = 20;
    public float DashDistance = 40f;
    public bool HardStopOnKeyUp = false;

    public int MaxDashCharges = 3;
    public int PerfectReloadBonus = 1;
    public int CurrentDashCharges = 3;

    public int MaxJumps = 2;
    public int _AvailableJumps = 0;
    public float MaxJumpResetAngle = 20;

    public Vector2 WallJumpClimb = new Vector2(2,25);
    public Vector2 WallJumpOff = new Vector2(5,5);
    public Vector2 WallLeap = new Vector2(15,20);

    public float WallSlideSpeedMax = 8;
    public float WallStickTime = .25f;
    float TimeToWallUnstick;

    public float Gravity = -70;
    public float MaxJumpVelocity = 25;
    public float MinJumpVelocity = 12;
    [HideInInspector]
    public Vector3 Velocity;
    float VelocityXSmoothing;

    Controller2D _Controller;

    Vector2 DirectionalInput;
    bool WallSliding;
    int WallDirX;

    [HideInInspector]
    public Vector2 PlayerLookDirection = new Vector2(1,0);
    #endregion

    #region Reloading Variables
    private bool _IsReloading = false;
    public bool IsReloading
    {
        get { return _IsReloading; }
        set
        {
            _IsReloading = value;
            if (value == false)
            {
                StopCoroutine(_ReloadRoutine);
                _ReloadRoutine = null;
            }
        }
    }


    public float PartialReloadTime = 1.5f;
    public float FullReloadTime = 2.5f;
    public float ActiveReloadStartPercent = 30f;
    public float ActiveReloadEndPercent = 50f;
    public float MaxFailedReloadBonusTime = 4f;
    private PantsChargeUIManager PantsChargeUIManager;
    #endregion

    #region Restrictions / Modifications
    public bool _CanMove = true;
    public bool _CanJump = true;
    float _CurentMaxMoveSpeed = 20f;

    public bool AllowMovementDuringPartialReload = true;
    public float MaxMoveSpeedDuringPartialReload = 5f;
    public bool AllowMovementDuringFullReload = false;
    public float MaxMoveSpeedDuringFullReload = 3f;


    public float MaxMoveSpeedWithPantsDown = 10f;
    #endregion

    public AudioClip JumpClip;
    AudioSource _PlayerAudioSource;  


    #region Setup
    void Start()
    {
        _Controller = GetComponent<Controller2D>();
        _PlayerAudioSource = GetComponent<AudioSource>();
        _PlayerAudioSource.clip = JumpClip;

        PantsChargeUIManager = GameObject.Find("Pants Charge Manager").GetComponent<PantsChargeUIManager>();

        OnPlayerStateChanged += Player_OnPlayerStateChanged;

        _CurentMaxMoveSpeed = MoveSpeed;
    }

    private void AutomaticallyCalculateGravity ()
    {
        Gravity = -(2 * MaxJumpHeight) / Mathf.Pow(TimeToJumpApex, 2);
        MaxJumpVelocity = Mathf.Abs(Gravity) * TimeToJumpApex;
        MinJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(Gravity) * MinJumpHeight);
    }
    #endregion


    void FixedUpdate()
    {
        CalculateVelocity();
        HandleWallSliding();

        _Controller.Move(Velocity * Time.deltaTime, DirectionalInput);

        if (_Controller.collisions.below)
            if (_Controller.collisions.slopeAngle <= MaxJumpResetAngle)
                _AvailableJumps = MaxJumps;

        if (_Controller.collisions.above || _Controller.collisions.below)
            if (_Controller.collisions.slidingDownMaxSlope)
                Velocity.y += _Controller.collisions.slopeNormal.y * -Gravity * Time.deltaTime;
            else
                Velocity.y = 0;
    }


    #region Handle Inputs
    public void SetDirectionalInput(Vector2 input)
    {
        DirectionalInput = input;

        if (input.x > 0)
            PlayerLookDirection.x = 1;
        else if (input.x < 0)
            PlayerLookDirection.x = -1;
    }
    public void OnJumpInputDown()
    {
        if (WallSliding)
        {
            if (WallDirX == DirectionalInput.x)
            {
                Velocity.x = -WallDirX * WallJumpClimb.x;
                Velocity.y = WallJumpClimb.y;
                _AvailableJumps--;
                _PlayerAudioSource.PlayOneShot(JumpClip);
            }
            else if (DirectionalInput.x == 0)
            {
                Velocity.x = -WallDirX * WallJumpOff.x;
                Velocity.y = WallJumpOff.y;
                _AvailableJumps--;
                _PlayerAudioSource.PlayOneShot(JumpClip);
            }
            else
            {
                Velocity.x = -WallDirX * WallLeap.x;
                Velocity.y = WallLeap.y;
                _AvailableJumps--;
                _PlayerAudioSource.PlayOneShot(JumpClip);
            }
        }

        if (_CanJump)
        {
            if (_Controller.collisions.below)
            {
                if (_Controller.collisions.slidingDownMaxSlope)
                {
                    if (DirectionalInput.x != -Mathf.Sign(_Controller.collisions.slopeNormal.x))
                    { // not jumping against max slope
                        Velocity.y = MaxJumpVelocity * _Controller.collisions.slopeNormal.y;
                        Velocity.x = MaxJumpVelocity * _Controller.collisions.slopeNormal.x;
                        _AvailableJumps--;
                        _PlayerAudioSource.PlayOneShot(JumpClip);
                    }
                }
                else
                {
                    Velocity.y = MaxJumpVelocity;
                    _AvailableJumps--;
                    _PlayerAudioSource.PlayOneShot(JumpClip);
                }
            }
            else
            {
                if (_AvailableJumps > 0)
                {
                    if (CurrentDashCharges > 0)
                    {
                        Velocity.y = MaxJumpVelocity;
                        CurrentDashCharges--;
                        PantsChargeUIManager.ChargesUpdated(CurrentDashCharges);

                        _AvailableJumps--;
                        _PlayerAudioSource.PlayOneShot(JumpClip);
                    }
                    else
                    {

                    }
                }
            }
        }
    }
    public void OnJumpInputUp()
    {
        if (Velocity.y > MinJumpVelocity)
            Velocity.y = MinJumpVelocity;
    }
    public void OnDashInputDown()
    {
        if (CurrentDashCharges > 0)
        {
            if (Velocity.x > 0 || Velocity.x < 0)
            {
                if (Velocity.x > 0)
                    Velocity.x = DashDistance;
                else if (Velocity.x < 0)
                    Velocity.x = -DashDistance;

                CurrentDashCharges--;
                PantsChargeUIManager.ChargesUpdated(CurrentDashCharges);
            }                        
        }
    }

    #region Reload Private Variables
    Coroutine _ReloadRoutine = null;
    float _ReloadStartTime = 0;
    float _ReloadInstanceTime = 0;
    float _HotspotStartTime = 0;
    float _HotspotEndtime = 0; 
    #endregion
    public void OnReloadPantsInputDown()
    {
        //Only allow reloading on the ground
        if (_Controller.collisions.below)
        {
            if (_ReloadRoutine == null && !IsReloading)
            {
                if (CurrentDashCharges < MaxDashCharges)
                {
                    _ReloadStartTime = Time.time;

                    if (CurrentDashCharges == 0)
                    {
                        _ReloadInstanceTime = FullReloadTime;
                        PlayerState = PlayerStateEnum.FullReload;                        
                    }
                    else
                    {
                        _ReloadInstanceTime = PartialReloadTime;
                        PlayerState = PlayerStateEnum.PartialReload;                       
                    }

                    _HotspotStartTime = (_ReloadInstanceTime * (ActiveReloadStartPercent / 100));
                    _HotspotEndtime = (_ReloadInstanceTime * (ActiveReloadEndPercent / 100));

                    _ReloadRoutine = StartCoroutine(EndReload(_ReloadInstanceTime));

                    PantsChargeUIManager.StartReload(_ReloadInstanceTime, ActiveReloadStartPercent, ActiveReloadEndPercent);
                    IsReloading = true;
                }
            }
            else if (IsReloading)
            {
                if (CheckActiveReloadSuccess())
                {
                    PantsChargeUIManager.StopReload(true);
                    IsReloading = false;
                    CurrentDashCharges = MaxDashCharges + PerfectReloadBonus;
                    PantsChargeUIManager.ChargesUpdated(CurrentDashCharges);
                    PlayerState = PlayerStateEnum.Idle;
                }
                else
                {
                    //Failed reload
                    StopCoroutine(_ReloadRoutine);

                    //Calculate additional time to add on to reload based on percent
                    float percent = (Time.time - _ReloadStartTime) / _ReloadInstanceTime;
                    //invert to get the multiplier for bonus time
                    percent = 100 - percent;
                    float totalBonusTime = MaxFailedReloadBonusTime * (percent / 100);

                    PlayerState = PlayerStateEnum.FailedReload;

                    PantsChargeUIManager.StopReload(false);
                    _ReloadRoutine = StartCoroutine(EndReload(totalBonusTime));
                }

            }
        }
    }
    #endregion


    #region Reload Utility Methods
    private bool CheckActiveReloadSuccess()
    {
        float currentTime = Time.time;
        float timeSinceReloadStart = currentTime - _ReloadStartTime;

        if (timeSinceReloadStart > _HotspotStartTime && timeSinceReloadStart < _HotspotEndtime)
            return true;
        else
            return false;
    }
    private IEnumerator EndReload(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);

        CurrentDashCharges = 3;
        PantsChargeUIManager.ChargesUpdated(CurrentDashCharges);
        PantsChargeUIManager.StopReload(true);
        IsReloading = false;
        PlayerState = PlayerStateEnum.Idle;
    }
    #endregion


    #region Movement Utility Methods
    void HandleWallSliding()
    {
        WallDirX = (_Controller.collisions.left) ? -1 : 1;
        WallSliding = false;
        if ((_Controller.collisions.left || _Controller.collisions.right) && !_Controller.collisions.below && Velocity.y < 0)
        {
            WallSliding = true;

            if (Velocity.y < -WallSlideSpeedMax)
                Velocity.y = -WallSlideSpeedMax;

            if (TimeToWallUnstick > 0)
            {
                VelocityXSmoothing = 0;
                Velocity.x = 0;

                if (DirectionalInput.x != WallDirX && DirectionalInput.x != 0)
                    TimeToWallUnstick -= Time.deltaTime;
                else
                    TimeToWallUnstick = WallStickTime;
            }
            else
                TimeToWallUnstick = WallStickTime;
        }
    }
    void CalculateVelocity()
    {
        float targetVelocityX;

        if (_CanMove)
        {
            targetVelocityX = DirectionalInput.x * MoveSpeed;

            if (PlayerState == PlayerStateEnum.PartialReload)
                if (targetVelocityX > 0 && targetVelocityX > MaxMoveSpeedDuringPartialReload)
                    targetVelocityX = MaxMoveSpeedDuringPartialReload;
                else if (targetVelocityX < 0 && targetVelocityX < (MaxMoveSpeedDuringPartialReload * -1))
                    targetVelocityX = (MaxMoveSpeedDuringPartialReload * -1);

            if (PlayerState == PlayerStateEnum.FullReload)
                if (targetVelocityX > 0 && targetVelocityX > MaxMoveSpeedDuringFullReload)
                    targetVelocityX = MaxMoveSpeedDuringFullReload;
                else if (targetVelocityX < 0 && targetVelocityX < (MaxMoveSpeedDuringFullReload * -1))
                    targetVelocityX = (MaxMoveSpeedDuringFullReload * -1);

            if (_Controller.collisions.below && DirectionalInput.x == 0)
            {
                targetVelocityX = 0;

                if (HardStopOnKeyUp)
                    Velocity.x = 0;
            }
                
        }            
        else
            targetVelocityX = 0;        

        Velocity.x = Mathf.SmoothDamp(Velocity.x, targetVelocityX, ref VelocityXSmoothing, (_Controller.collisions.below) ? _AccelerationTimeGrounded : _AccelerationTimeAirborne);
        Velocity.y += Gravity * Time.deltaTime;
    } 
    #endregion


    private void Player_OnPlayerStateChanged(object sender, PlayerStateChangedEventArgs e)
    {
        //Handle any logic entering the new state
        switch (e.NewPlayerState)
        {
            case PlayerStateEnum.Dead:
                _CanMove = false;
                _CanJump = false;
                break;
            case PlayerStateEnum.Idle:
                _CanMove = true;
                _CanJump = true;
                _PlayerState = PlayerStateEnum.Idle;
                break;
            case PlayerStateEnum.Jumping:              
                break;
            case PlayerStateEnum.PantsDown:
                _CurentMaxMoveSpeed = MaxMoveSpeedWithPantsDown;
                _CanMove = true;
                _CanJump = false;
                break;
            case PlayerStateEnum.PartialReload:
                if (AllowMovementDuringPartialReload)
                {
                    _CanMove = true;
                    _CanJump = true;
                }
                else
                {
                    _CanMove = false;
                    _CanJump = false;
                }
                break;
            case PlayerStateEnum.FullReload:
                if (AllowMovementDuringFullReload)
                {
                    _CanMove = true;
                    _CanJump = true;
                }
                else
                {
                    _CanMove = false;
                    _CanJump = false;
                }
                break;
            case PlayerStateEnum.FailedReload:
                _CanMove = false;
                _CanJump = false;
                break;
            case PlayerStateEnum.Respawning:
                break;
            case PlayerStateEnum.Running:
                _CanMove = true;
                _CanJump = true;
                break;
            case PlayerStateEnum.SlodeSlide:
                break;
            case PlayerStateEnum.WallSlide:
                break;
        }
    }


    #region Events
    public delegate void PlayerStateChangedEventHandler(object sender, PlayerStateChangedEventArgs e);
    public event PlayerStateChangedEventHandler OnPlayerStateChanged;

    private void PlayerStateChanged(PlayerStateEnum playerState)
    {
        if (OnPlayerStateChanged == null) return;
        OnPlayerStateChanged(this, new PlayerStateChangedEventArgs(PlayerState, playerState));
    } 
    #endregion
}

public class PlayerStateChangedEventArgs : EventArgs
{
    public Player.PlayerStateEnum OldPlayerState { get; set; }
    public Player.PlayerStateEnum NewPlayerState { get; set; }

    public PlayerStateChangedEventArgs(Player.PlayerStateEnum oldState, Player.PlayerStateEnum newState)
    {
        OldPlayerState = oldState;
        NewPlayerState = newState;
    }
}