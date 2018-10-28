using UnityEngine;

public class CameraSystem : MonoBehaviour
{

    public enum CameraStateEnum { Follow, Zone };
    public CameraStateEnum CameraState = CameraStateEnum.Follow;

    public GameObject ZoneObject;
    public float DefaultCameraSize = 10;

    Vector3 Velocity = Vector3.zero;
    public Transform Player;
    Player _PlayerScript;

    public bool drawDebugLines = true;
    public float cameraDamping = .2f;
    public float maxCameraSpeed = 40f;
    ScreenZone FocusZone;
    ScreenZone PanicZone;

    public float FocusX = 40;
    public float FocusY = 60;
    public float FocusWidth = 12;
    public float FocusHeight = 8;

    public float PanicX = 20;
    public float PanicY = 20;
    public float PanicWidth = 60;
    public float PanicHeight = 60;

    Vector3 LastCameraPosition = new Vector3(0, 0, 0);
    Vector3 TargetPosition = new Vector3(0, 0, 5);
    Vector2 LastLookDirection = new Vector2(1, 0);

    // Use this for initialization
    void Start()
    {
        FocusZone = new ScreenZone(FocusX, FocusY, FocusWidth, FocusHeight);
        PanicZone = new ScreenZone(PanicX, PanicY, PanicWidth, PanicHeight);

        _PlayerScript = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FlipScreenZoneOnX(FocusZone, _PlayerScript.PlayerLookDirection);
        LastLookDirection = _PlayerScript.PlayerLookDirection;

        if (CameraState == CameraStateEnum.Follow)
        {
            SizeCamera();
            bool useYPanic = false;
            bool useXPanic = false;

            if (IsPlayerOutsideFocusCrossX())
            {
                float distance = CalculateVectorRelativeToLastCameraPosition(Player.position);
                float cameraSpeed = distance;

                if (cameraSpeed > maxCameraSpeed)
                    cameraSpeed = maxCameraSpeed;

                float xPos = Player.transform.position.x;
                float yPos;

                if (IsPlayerOutsideFocusCrossY() && Player.GetComponent<Controller2D>().collisions.below)
                    yPos = Player.transform.position.y;
                else if (IsPlayerOutsidePanicCrossY())
                {
                    yPos = Player.transform.position.y;
                    useYPanic = true;
                }                    
                else
                    yPos = transform.position.y;

                if (IsPlayerOutsidePanicCrossX())
                    useXPanic = true;

                TargetPosition = new Vector3(xPos, yPos, -5);
            }

            if (useYPanic || useXPanic)
                transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref Velocity, (cameraDamping * .75f));
            else
                transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref Velocity, cameraDamping);


            LastCameraPosition = transform.position;
        }
        else if (CameraState == CameraStateEnum.Zone)
        {
            SizeCamera();

            TargetPosition = new Vector3(ZoneObject.transform.position.x, ZoneObject.transform.position.y, -5);
            transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref Velocity, 1 + cameraDamping);
            LastCameraPosition = transform.position;
        }
    }

    private void SizeCamera()
    {
        if (CameraState == CameraStateEnum.Zone)
        {
            BoxCollider2D zoneCollider = ZoneObject.GetComponent<BoxCollider2D>();
            Vector3 minPoint = Camera.main.WorldToViewportPoint(zoneCollider.bounds.min);
            Vector3 maxPoint = Camera.main.WorldToViewportPoint(zoneCollider.bounds.max);

            if (minPoint.x < 0 || minPoint.x > 1 || minPoint.y < 0 || minPoint.y > 1)
            {
                Camera.main.orthographicSize += (1 * cameraDamping);
            }
        }
        else if (CameraState == CameraStateEnum.Follow)
        {
            if (Camera.main.orthographicSize > DefaultCameraSize)
            {
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, DefaultCameraSize, cameraDamping);
            }
        }

    }

    public void SetZoneTarget(GameObject zone)
    {
        ZoneObject = zone;
        CameraState = CameraStateEnum.Zone;
    }

    public void ClearZoneTarget()
    {
        CameraState = CameraStateEnum.Follow;
        ZoneObject = null;
    }


    private bool IsPlayerOutsideFocusCrossX()
    {
        Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(Player.transform.position);

        if (playerScreenPos.x > FocusZone.X && playerScreenPos.x < FocusZone.X + FocusZone.Width)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool IsPlayerOutsideFocusCrossY()
    {
        Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(Player.transform.position);

        if (playerScreenPos.y > FocusZone.Y && playerScreenPos.y < FocusZone.Y + FocusZone.Height)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool IsPlayerOutsidePanicCrossX()
    {
        Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(Player.transform.position);

        if (playerScreenPos.x > PanicZone.X && playerScreenPos.x < PanicZone.X + PanicZone.Width)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool IsPlayerOutsidePanicCrossY()
    {
        Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(Player.transform.position);

        if (playerScreenPos.y > PanicZone.Y && playerScreenPos.y < PanicZone.Y + PanicZone.Height)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    private float CalculateVectorRelativeToLastCameraPosition(Vector3 vector)
    {
        return Vector3.Distance(LastCameraPosition, vector);
    }

    private void FlipScreenZoneOnX(ScreenZone zone, Vector2 direction)
    {
        if (LastLookDirection == direction)
        {
            return;
        }

        if (zone.X == zone.InvertX)
        {
            zone.X = zone.NormalX;
        }
        else
        {
            zone.X = zone.InvertX;
        }
    }

    private void OnGUI()
    {
        if (!drawDebugLines)
        {
            return;
        }

        int width = Screen.width;
        int height = Screen.height;
        Color red = new Color(1.0f, 0.0f, 0.0f);
        Color yellow = new Color(1.0f, 1.0f, 0.0f);
        Color green = new Color(0.0f, 1.0f, 0.0f);

        Vector3 center = new Vector3(width / 2, height / 2, 0);

        Drawing.DrawLine(new Vector3(center.x, 0, 0), new Vector3(center.x, height, 0), green);



        Drawing.DrawLine(new Vector3(0, FocusZone.Y, 0), new Vector3(width, FocusZone.Y, 0), yellow); // Top Horizontal
        Drawing.DrawLine(new Vector3(0, FocusZone.Y + FocusZone.Height, 0), new Vector3(width, FocusZone.Y + FocusZone.Height, 0), yellow); // Bottom Horizontal

        Drawing.DrawLine(new Vector3(FocusZone.X, 0, 0), new Vector3(FocusZone.X, height, 0), yellow); // Left Vertical
        Drawing.DrawLine(new Vector3(FocusZone.X + FocusZone.Width, 0, 0), new Vector3(FocusZone.X + FocusZone.Width, height, 0), yellow); // Right Vertical


        Drawing.DrawLine(new Vector3(0, PanicZone.Y, 0), new Vector3(width, PanicZone.Y, 0), red); // Top Horizontal
        Drawing.DrawLine(new Vector3(0, PanicZone.Y + PanicZone.Height, 0), new Vector3(width, PanicZone.Y + PanicZone.Height, 0), red); // Bottom Horizontal

        Drawing.DrawLine(new Vector3(PanicZone.X, 0, 0), new Vector3(PanicZone.X, height, 0), red); // Left Vertical
        Drawing.DrawLine(new Vector3(PanicZone.X + PanicZone.Width, 0, 0), new Vector3(PanicZone.X + PanicZone.Width, height, 0), red); // Right Vertical

        /*
        Vector3 panicLeft = new Vector3(width * (panicZoneLeft / 100f), height / 2, 0);
        Vector3 panicRight = new Vector3(width * (panicZoneRight / 100f), height / 2, 0);
        Vector3 panicTop = new Vector3(0, height * (panicZoneTop / 100f), 0);
        Vector3 panicBottom = new Vector3(width, height * (panicZoneBottom / 100f), 0);

        Vector3 deadLeft = new Vector3(width * (deadZoneLeft / 100f), height / 2, 0);
        Vector3 deadRight = new Vector3(width * (deadZoneRight / 100f), height / 2, 0);
        Vector3 deadTop = new Vector3(0, height * (deadZoneTop / 100f), 0);
        Vector3 deadBottom = new Vector3(width, height * (deadZoneBottom / 100f), 0);
        

        Drawing.DrawLine(new Vector3(panicLeft.x, 0, 0), new Vector3(panicLeft.x, height, 0), yellow);
        Drawing.DrawLine(new Vector3(panicRight.x, 0, 0), new Vector3(panicRight.x, height, 0), yellow);
        Drawing.DrawLine(new Vector3(0, panicTop.y, 0), new Vector3(width, panicTop.y, 0), yellow);
        Drawing.DrawLine(new Vector3(0, panicBottom.y, 0), new Vector3(width, panicBottom.y, 0), yellow);

        Drawing.DrawLine(new Vector3(deadLeft.x, 0, 0), new Vector3(deadLeft.x, height, 0), green);
        Drawing.DrawLine(new Vector3(deadRight.x, 0, 0), new Vector3(deadRight.x, height, 0), green);
        Drawing.DrawLine(new Vector3(0, deadTop.y, 0), new Vector3(width, deadTop.y, 0), green);
        Drawing.DrawLine(new Vector3(0, deadBottom.y, 0), new Vector3(width, deadBottom.y, 0), green);
        */
    }
}

public class ScreenZone
{
    public float NormalX;
    public float InvertX;

    public float X;
    public float Y;

    public float Width;
    public float Height;

    public ScreenZone(float x, float y, float width, float height)
    {
        int _width = Screen.width;
        int _height = Screen.height;

        NormalX = _width * (x / 100);

        X = _width * (x / 100);
        Y = _height * (y / 100);
        Width = _width * (width / 100);
        Height = _height * (height / 100);

        InvertX = _width * ((100 - x) / 100) - Width;
    }
}