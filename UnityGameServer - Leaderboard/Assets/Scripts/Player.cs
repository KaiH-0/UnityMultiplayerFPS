using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    //public CharacterController controller;
    public Transform shootOrigin;
    public Quaternion headAngle;
    public bool walking;
    public bool shot;
    public Vector3 gunpos;
    public int weapon;
    public bool holding;
    public Quaternion gunrot;
    public Quaternion fakeCam;
    public Vector3 fakeCamPos;
    //public float gravity = -9.81f;
    //public float moveSpeed = 6f;
    //public float moveRunSpeed = 10f;
    //public float jumpSpeed = 6f;



    //Wallrunning
    public LayerMask whatIsWall;
    public float wallrunForce, maxWallrunTime, maxWallSpeed;
    bool isWallRight, isWallLeft;
    public bool isWallRunning;
    public float maxWallRunCameraTilt, wallRunCameraTilt;
    //Assingables
    //public Transform playerCam;
    public Transform orientation;

    //Other
    private Rigidbody rb;

    //Rotation and look
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

    //Movement
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    private float startMaxSpeed;
    public bool grounded;
    public LayerMask whatIsGround;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    //Crouch & Slide
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;
    public float crouchGravityMultiplier;

    //Jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;

    public int startDoubleJumps = 1;
    int doubleJumpsLeft;

    //Input
    public float x, y;
    bool jumping, sprinting, crouching;

    //AirDash
    public float dashForce;
    public float dashCooldown;
    public float dashTime;
    bool allowDashForceCounter;
    bool readyToDash;
    int wTapTimes = 0;
    Vector3 dashStartVector;

    //RocketBoost
    public float maxRocketTime;
    public float rocketForce;
    bool rocketActive, readyToRocket;
    bool alreadyInvokedRockedStop;
    float rocketTimer;

    //Sliding
    private Vector3 normalVector = Vector3.up;

    //SonicSpeed
    public float maxSonicSpeed;
    public float sonicSpeedForce;
    public float timeBetweenNextSonicBoost;
    float timePassedSonic;

    //flash
    public float flashCooldown, flashRange;
    public int maxFlashesLeft;
    bool alreadySubtractedFlash;
    public int flashesLeft = 3;

    //Climbing
    public float climbForce, maxClimbSpeed;
    public LayerMask whatIsLadder;
    bool alreadyStoppedAtLadder;


    public float throwForce = 600f;
    public float health;
    public float maxHealth = 100f;
    public int itemAmount = 0;
    public int maxItemAmount = 3;
    public Animator anim;
    public GameObject head;
    public GameObject player;
    //<<Summary>> Checks the distance from walls and takes the wall that is the closest to the player
    private bool[] inputs;
    private float yVelocity = 0;
    public Vector2 _inputDirection = Vector2.zero;
    public string otheruser;
    public string kill;
    public string newkill;
    public int count;
    public int timer = 60;
    public KillManager killman;

    public int kills;
    public int deaths;

    void Awake()
    {
        killman = GameObject.Find("KillManager").GetComponent<KillManager>();
        rb = GetComponent<Rigidbody>();
        startMaxSpeed = maxSpeed;
    }

    private void FixedUpdate()
    {
        Movement();

        if (inputs[0])
        {
            _inputDirection.y = 1;
        }

        if (inputs[1])
        {
            _inputDirection.y = -1;
        }

        if (inputs[2])
        {
            _inputDirection.x = -1;
        }

        if (inputs[3])
        {
            _inputDirection.x = 1;
        }

        if (!inputs[0] && !inputs[1])
        {
            _inputDirection.y = 0;
        }

        if (!inputs[2] && !inputs[3])
        {
            _inputDirection.x = 0;
        }
    }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;

        inputs = new bool[8];
        //killman.personalData.Add(id, deaths);
        //killman.playerinfo.Add(new KillManager.PlayerInfo(id, _username, deaths));
        killman.playerinfo.Add(_id, new KillManager.PlayerInfo(id, username, deaths));
        killman.AddInfo();
        //killman.UpdateKillTest();
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= 1;
        }

        if (timer <= 0)
        {
            //killman.UpdateKillTest();
            timer = 60;
        }

        MyInput();
        Look();
        CheckForWall();
        //SonicSpeed();
        WallRunInput();
        anim.SetBool("isWalking", walking);
        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    private void WallRunInput() //make sure to call in void Update
    {
        //Wallrun
        if (inputs[3] && isWallRight) StartWallrun();
        if (inputs[2] && isWallLeft) StartWallrun();
    }

    private void StartWallrun()
    {
        rb.useGravity = false;
        isWallRunning = true;
        allowDashForceCounter = false;

        if (rb.velocity.magnitude <= maxWallSpeed)
        {
            rb.AddForce(orientation.forward * wallrunForce * Time.deltaTime);

            //Make sure char sticks to wall
            if (isWallRight)
                rb.AddForce(orientation.right * wallrunForce / 5 * Time.deltaTime);
            else
                rb.AddForce(-orientation.right * wallrunForce / 5 * Time.deltaTime);
        }
    }
    private void StopWallRun()
    {
        isWallRunning = false;
        rb.useGravity = true;
    }

    void OnDestroy()
    {
        //if (id > 0 && id < killman.playerinfo.Count)
        //{
        //killman.oldid += 1;
        //killman.playerinfo.Remove(killman.playerinfo[id]);
        //if (id < killman.playerinfo.Count)
        //{
        int _id = id;
            killman.playerinfo.Remove(_id);
         //   killman.oldid += 1;
        //}
        //else
        //{
        //    Debug.Log("ID = " + id + "Whereas amount of people = " + killman.playerinfo.Count);
        //}
        //}
        //else
        //{
        //    killman.playerinfo.Remove(killman.playerinfo[id - killman.oldid]);
        //}
        //killman.playerinfo[id].ID = 0;
        //killman.playerinfo[id].Username = "Gone";
        //killman.UpdateKillTest(this);
        //killman.UpdatePlayers(this, id);
        //killman.oldid -= 1;
        //killman.playerinfo.Remove(killman.playerinfo[id]);
        //killman.oldid -= 1;
        //killman.ClientCount = 0;
        //killman.UpdateKillTest();
    }

    private void CheckForWall() //make sure to call in void Update
    {
        isWallRight = Physics.Raycast(transform.position, orientation.right, 2.5f, whatIsWall);
        isWallLeft = Physics.Raycast(transform.position, -orientation.right, 2.5f, whatIsWall);
        Debug.DrawRay(transform.position, orientation.right,Color.green);
        Debug.DrawRay(transform.position, -orientation.right, Color.red);

        //leave wall run
        if (!isWallLeft && !isWallRight) StopWallRun();
        //reset double jump (if you have one :D)
        if (isWallLeft || isWallRight) doubleJumpsLeft = startDoubleJumps;
    }

    private void MyInput()
    {
        //_inputDirection.x = Input.GetAxisRaw("Horizontal");
        //_inputDirection.y = Input.GetAxisRaw("Vertical");
        x = _inputDirection.x;
        y = _inputDirection.y;
        crouching = inputs[5];
        //Double Jumping
        if (inputs[4] && grounded == true)
        {
            Jump();
            //doubleJumpsLeft--;
        }

        //Crouching
        if (inputs[6])
            StartCrouch();
        if (inputs[7])
            StopCrouch();
    }

    private void StartCrouch()
    {
        //transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (rb.velocity.magnitude > 0.5f)
        {
            if (grounded)
            {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }

    private void StopCrouch()
    {
        //transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private void Movement()
    {
        //Extra gravity
        //Needed that the Ground Check works better!
        float gravityMultiplier = 170f;

        if (crouching) gravityMultiplier = crouchGravityMultiplier;

        rb.AddForce(Vector3.down * Time.deltaTime * gravityMultiplier);

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(x, y, mag);

        //If holding jump && ready to jump, then jump
        if (readyToJump && jumping && grounded && !rocketActive) Jump();

        //ResetStuff when touching ground
        if (grounded)
        {
            //readyToDash = true;
            readyToRocket = true;
            doubleJumpsLeft = startDoubleJumps;
        }

        //Set max speed
        float maxSpeed = this.maxSpeed;

        //If sliding down a ramp, add force down so player stays grounded and also builds speed
        if (crouching && grounded && readyToJump)
        {
            //rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }

        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;

        // Movement in air
        if (!grounded)
        {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        // Movement while sliding
        if (grounded && crouching) multiplierV = 0f;

        //Apply forces to move player
        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
    }

    private void Jump()
    {
        if (grounded)
        {
            readyToJump = false;

            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (!grounded)
        {
            readyToJump = false;

            //Add jump forces
            rb.AddForce(orientation.forward * jumpForce * 1f);
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            //Reset Velocity
            rb.velocity = Vector3.zero;

            //Disable dashForceCounter if doublejumping while dashing
            allowDashForceCounter = false;

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //Walljump
        if (isWallRunning)
        {
            readyToJump = false;

            //normal jump
            if (isWallLeft && !inputs[3] || isWallRight && !inputs[2])
            {
                rb.AddForce(Vector2.up * jumpForce * 1.5f);
                rb.AddForce(normalVector * jumpForce * 0.5f);
            }

            //sidwards wallhop
            if (isWallRight || isWallLeft && inputs[2] || inputs[3]) rb.AddForce(-orientation.up * jumpForce * 1f);
            if (isWallRight && inputs[2]) rb.AddForce(-orientation.right * jumpForce * 3.2f);
            if (isWallLeft && inputs[3]) rb.AddForce(orientation.right * jumpForce * 3.2f);

            //Always add forward force
            //rb.AddForce(orientation.forward * jumpForce * 1f);

            //Disable dashForceCounter if doublejumping while dashing
            allowDashForceCounter = false;

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void ActivateGravity()
    {
        rb.useGravity = true;

        //Counter currentForce
        if (allowDashForceCounter)
        {
            rb.AddForce(dashStartVector * -dashForce * 0.5f);
        }
    }

    private void Look()
    {
        orientation.transform.localRotation = Quaternion.Euler(0, player.transform.localRotation.y, 0);
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || jumping) return;

        //Slow down sliding
        if (crouching)
        {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    private bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;

    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private void OnCollisionStay(Collision other)
    {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++)
        {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal))
            {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded()
    {
        grounded = false;
    }

    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        inputs = _inputs;
        transform.rotation = _rotation;
    }

    public void Shoot(Vector3 _viewDirection)
    {
        if (health <= 0f)
        {
            return;
        }

        if (Physics.Raycast(shootOrigin.position, _viewDirection, out RaycastHit _hit, 25f))
        {
            Debug.Log("Hit " + _hit.transform.name);
            if (_hit.collider.CompareTag("Player"))
            {
                if (_hit.collider.GetComponentInParent<Player>().id != id)
                {
                    _hit.collider.GetComponentInParent<Player>().otheruser = username;
                    _hit.collider.GetComponentInParent<Player>().TakeDamage(50f);
                }
            }
        }
    }

    public void ThrowItem(Vector3 _viewDirection)
    {
        if (health <= 0f)
        {
            //ServerSend.PlayerKill(this);
            return;
        }

        if (itemAmount > 0)
        {
            itemAmount--;
            NetworkManager.instance.InstantiateProjectile(shootOrigin).Initialize(_viewDirection, throwForce, id);
        }
    }

    public void TakeDamage(float _damage)
    {
        if (health <= 0f)
        {
            return;
        }

        health -= _damage;
        if (health <= 0f)
        {
            deaths += 1;
            //killman.playerinfo[id].Deaths = deaths;
            //killman.UpdateDeaths(this);
            //Debug.Log(killman.playerinfo[id].Deaths);
            //Debug.Log(killman.personalData[id] + " " + killman.personalData[deaths]);
            NetworkManager.instance.InstantiatePlayerDeath(transform.position);
            health = 0f;
            //controller.enabled = false;
            transform.position = new Vector3(0f, 25f, 0f);
            ServerSend.PlayerPosition(this);
            kill = "\n" + otheruser + " Killed " + username;
            newkill = kill;
            Debug.Log(newkill);
            ServerSend.PlayerKill(this);
            //ServerSend.PlayerExploded(this)
            //killman.UpdateKillTest();
            StartCoroutine(Respawn());
        }

        ServerSend.PlayerHealth(this);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);

        health = maxHealth;
        //controller.enabled = true;
        ServerSend.PlayerRespawned(this);
    }

    public bool AttemptPickupItem()
    {
        if (itemAmount >= maxItemAmount)
        {
            return false;
        }

        itemAmount++;
        return true;
    }
}
