using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    private Rigidbody rb;
    private MovementManager pm;
    private RaycastHit LeftWallhit;
    private RaycastHit RightWallhit;

    [SerializeField] private float WallrunForce;
    [SerializeField] private float WallRunDistance;
    [SerializeField] private Transform PlayerObj;
    [SerializeField] private float MinHight;
    [SerializeField] private float MaxHight;

    public LayerMask isWall;
    [System.NonSerialized] public bool wallLeft;
    [System.NonSerialized] public bool wallRight;
    [System.NonSerialized] public bool exitingWall;

    [SerializeField] private float CamTilt;
    private float initialCamTilt = 0;

    public float tilt {get; set;}

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<MovementManager>();
        Transform Cam =  transform.GetChild(0).gameObject.GetComponent<Transform>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
            WallRunningMovement();
    }
    
    private void CheckForWall()
    {
        wallLeft = Physics.Raycast(transform.position, -PlayerObj.right, out LeftWallhit, WallRunDistance, isWall);
        wallRight = Physics.Raycast(transform.position, PlayerObj.right, out RightWallhit, WallRunDistance, isWall);
    }

    //  条件に応じた処理をする処理
    private void StateMachine()
    {
        if ((wallLeft || wallRight) && pm.z > 0 && transform.position.y >= MinHight && transform.position.y <= MaxHight && !exitingWall)
        {
            if(!pm.wallrunning)
                StartWallRun();
        }
        else if (exitingWall)
        {
            if(pm.wallrunning)
                StopWallRun();
        }
        else
        {
            if (pm.wallrunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        rb.useGravity = false;
        pm.wallrunning = true;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); 

        if (wallRight)
            tilt = Mathf.LerpAngle(initialCamTilt, CamTilt, Time.time);
        else if (wallLeft)
            tilt = Mathf.LerpAngle(initialCamTilt, -CamTilt, Time.time);

    }

    private void StopWallRun()
    {
        rb.useGravity = true;
        pm.wallrunning = false;
        pm.JumpCount = 0;

       tilt = Mathf.LerpAngle(tilt, initialCamTilt, Time.time);
    }

    private void WallRunningMovement()
    {   
        Vector3 wallNormal = wallRight ? RightWallhit.normal : LeftWallhit.normal;  // 左右壁判定

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);              // 外積

        if((PlayerObj.forward - wallForward).magnitude > (PlayerObj.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        rb.AddForce(wallForward * WallrunForce, ForceMode.Impulse);
    }
}
