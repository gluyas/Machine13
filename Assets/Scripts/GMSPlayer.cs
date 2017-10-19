/**
 * - Edited by NovaSurfer (31.01.17).
 *   -----------------------------
 *   Rewriting from JS to C#
 *   Deleting "Spawn" and "Explode" methods, deleting unused varibles
 *   -----------------------------
 * Just some side notes here.
 *
 * - Should keep in mind that idTech's cartisian plane is different to Unity's:
 *    Z axis in idTech is "up/down" but in Unity Z is the local equivalent to
 *    "forward/backward" and Y in Unity is considered "up/down".
 *
 * - Code's mostly ported on a 1 to 1 basis, so some naming convensions are a
 *   bit fucked up right now.
 *
 * - UPS is measured in Unity units, the idTech units DO NOT scale right now.
 *
 * - Default values are accurate and emulates Quake 3's feel with CPM(A) physics.
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// Contains the command the user wishes upon the character
struct Cmd
{
    public float forwardMove;
    public float rightMove;
    public float upMove;
}

public class GMSPlayer : MonoBehaviour
{
    public static GMSPlayer Instance { get; private set; }
    
    public Transform playerView;                    // Camera
    public Transform user;                          // Player
    public float playerViewYOffset = 0.6f;          // The height at which the camera is bound to
    public static float xMouseSensitivity = 150f;
    public static float yMouseSensitivity = 150f;
    public float maxTilt = 3f;
    public float tiltSpeed = 0.2f;
    private float viewTilt = 0f;

    //
    /*Frame occuring factors*/
    public float gravity = 20.0f;

    public float friction = 6; //Ground friction

    /* Movement stuff */
    public float moveSpeed = 10f;                  // Ground move speed
    public float runAcceleration = 10.0f;           // Ground accel
    public float runDeacceleration = 10.0f;         // Deacceleration that occurs when running on the ground
    public float airAcceleration = 0.02f;           // Air accel
    public float airDecceleration = 2.0f;           // Deacceleration experienced when opposite strafing
    public float airControl = 0.3f;                 // How precise air control is
    public float sideStrafeAcceleration = 50.0f;    // How fast acceleration occurs to get up to sideStrafeSpeed
    public float sideStrafeSpeed = 0.75f;            // What the max speed to generate when side strafing
    public float jumpSpeed = 7.0f;                  // The speed at which the character's up axis gains when hitting jump
    public float moveScale = 1.0f;

    /* Shotgun jump stuff */
    public float shotJumpSpeed = 15.0f;             // The speed at which the character's up axis gains when hitting jump and shoots their weapon at the floor
    public float shotJumpSetTime = 0.15f;            // The amount of time after jumping the player can shotJump
    public float shotJumpMinAngle = 0.6f;

    private float shotJumpRemainTime;
    private bool jumpSuccess = false;

    /* Bouncepad stuff */

    private float bounceCount = 0f;
    private bool wishBounce = false;

    /*print() style */
    public GUIStyle style;

    /*FPS Stuff */
    public float fpsDisplayRate = 4.0f;         // 4 updates per sec

    private int frameCount = 0;
    private float dt = 0.0f;
    private float fps = 0.0f;

    private CharacterController _controller;

    // Camera rotations
    private float rotX = 0.0f;
    private float rotY = 0.0f;

    private Vector3 moveDirectionNorm = Vector3.zero;
    private Vector3 playerVelocity = Vector3.zero;
    private float playerTopVelocity = 0.0f;

    // Q3: players can queue the next jump just before he hits the ground
    private bool wishJump = false;

    // Used to display real time fricton values
    private float playerFriction = 0.0f;

    // Player commands, stores wish commands that the player asks for (Forward, back, jump, etc)
    private Cmd _cmd;

    public bool KickBack;
    public int KickBackTime;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        // Hide the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Put the camera inside the capsule collider
        playerView.position = new Vector3(
            transform.position.x,
            transform.position.y + playerViewYOffset,
            transform.position.z);

        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {

        // Get player position
        Vector3 playerposition = user.transform.position;

        // Do FPS calculation
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0 / fpsDisplayRate)
        {
            fps = Mathf.Round(frameCount / dt);
            frameCount = 0;
            dt -= 1.0f / fpsDisplayRate;
        }

        /* Ensure that the cursor is locked into the screen */
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                Cursor.lockState = CursorLockMode.Locked;
        }

        /* Camera rotation stuff, mouse controls this shit */
        rotX -= Input.GetAxis("Mouse Y") * xMouseSensitivity * 0.02f;
        rotY += Input.GetAxis("Mouse X") * yMouseSensitivity * 0.02f;

        // Clamp the X rotation
        if (rotX < -90)
            rotX = -90;
        else if (rotX > 90)
            rotX = 90;

        this.transform.rotation = Quaternion.Euler(0, rotY, 0); // Rotates the collider
        playerView.rotation = Quaternion.Euler(rotX, rotY, viewTilt); // Rotates the camera




        /* Movement, here's the important part */
        QueueJump();


        if (_controller.isGrounded)
        {
            GroundMove();
        }
        else if (!_controller.isGrounded)
        {
            AirMove();
        }

        /* Jumpshot and Bouncepad mechanic */
        ShotJump();
        BouncePad();

        // Move the controller
        _controller.Move(playerVelocity * Time.deltaTime);

        /* Calculate top velocity */
        Vector3 udp = playerVelocity;
        udp.y = 0.0f;
        if (playerVelocity.magnitude > playerTopVelocity)
            playerTopVelocity = playerVelocity.magnitude;

        //Need to move the camera after the player has been moved because otherwise the camera will clip the player if going fast enough and will always be 1 frame behind.
        // Set the camera's position to the transform
        playerView.position = new Vector3(
            transform.position.x,
            transform.position.y + playerViewYOffset,
            transform.position.z);

        if (KickBackTime >= 50)
        {
            KickBack = false;
            KickBackTime = 0;
        }
        if (KickBack)
        {
            KnockBack();
            KickBackTime++;
        }
    }

    /*******************************************************************************************************\
   |* MOVEMENT
   \*******************************************************************************************************/

    /// <summary>
    /// Knocks the player back on railgun fire
    /// </summary>
    public void KnockBack()
    {
        Accelerate(-user.forward, 10, 90);
    }
    
    /**
     * Sets the movement direction based on player input
     */
    private void SetMovementDir()
    {
        _cmd.forwardMove = Input.GetAxis("Vertical");
        _cmd.rightMove = Input.GetAxis("Horizontal");
    }

    /**
     * Queues the next jump just like in Q3
     */
    private void QueueJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !wishJump)
            wishJump = true;
        if (Input.GetKeyUp(KeyCode.Space))
            wishJump = false;
    }

    /**
     * 
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hitbox" || other.gameObject.tag == "Hazard") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("A " + other + " killed you");
        }
		if (other.gameObject.tag == "BouncePad") {

            wishBounce = true;
        //    bounceCount = 0;
        //    Destroy(other.transform.parent.gameObject);
        }
    }

    /**
     * Excecutes a Bounce if requirements are met
     */
    private void BouncePad()
    {
        if (wishBounce) {
            bounceCount += 1 * Time.deltaTime;
            //if (bounceCount >= 0.05f) {
            FindObjectOfType<AudioManager>().Play("bouncePad");
            playerVelocity.y = shotJumpSpeed;
            wishBounce = false;
            // }
        }
    }

    /**
     * Excecutes a shotjump if requirements are met
     */
    private void ShotJump()
    {
        if (jumpSuccess)
        {
            bool shotReady = GameObject.Find("Gun").GetComponent<BulletEmitter>().ShotReady;
            shotJumpRemainTime -= 1 * Time.deltaTime;

            // When a shotjump is successful
            if (shotJumpRemainTime > 0 && Input.GetMouseButtonUp(1) && rotX > shotJumpMinAngle && shotReady) {
				FindObjectOfType<AudioManager> ().Play ("shotgunAccent");
				playerVelocity.y = shotJumpSpeed;
				shotJumpRemainTime = 0;
			}
        }
    }
    /**
     * Execs when the player is in the air
    */
    private void AirMove()
    {
        FindObjectOfType<DynamicFOV>().fovPunch(4f, 1f, 2f);
        Vector3 wishdir;
        float wishvel = airAcceleration;
        float accel;

        float scale = CmdScale();

        SetMovementDir();

        wishdir = new Vector3(_cmd.rightMove, 0, _cmd.forwardMove);
        wishdir = transform.TransformDirection(wishdir);

        float wishspeed = wishdir.magnitude;
        wishspeed *= moveSpeed;

        wishdir.Normalize();
        moveDirectionNorm = wishdir;
        wishspeed *= scale;

        // CPM: Aircontrol
        float wishspeed2 = wishspeed;
        if (Vector3.Dot(playerVelocity, wishdir) < 0)
            accel = airDecceleration;
        else
            accel = airAcceleration;
        //Tilts screen slower while moving in air than ground
        if (_cmd.rightMove == -1 && viewTilt < maxTilt)
            viewTilt += tiltSpeed/8;
        if (_cmd.rightMove == 1 && viewTilt > -maxTilt)
            viewTilt -= tiltSpeed/8;
        if (_cmd.rightMove == 0 && viewTilt > 0)
            viewTilt -= tiltSpeed/16;
        if (_cmd.rightMove == 0 && viewTilt < 0)
            viewTilt += tiltSpeed/16;
        // If the player is ONLY strafing left or right
        if (_cmd.forwardMove == 0 && _cmd.rightMove != 0)
        {
            if (wishspeed > sideStrafeSpeed)
                wishspeed = sideStrafeSpeed;
            accel = sideStrafeAcceleration;
        }

        Accelerate(wishdir, wishspeed, accel);
        if (airControl > 0)
            AirControl(wishdir, wishspeed2);
        // !CPM: Aircontrol

        // Apply gravity
        playerVelocity.y -= gravity * Time.deltaTime;

    }

    /**
     * Air control
     */
    private void AirControl(Vector3 wishdir, float wishspeed)
    {
        float zspeed;
        float speed;
        float dot;
        float k;
        //int i;

        // Can't control movement if not moving forward or backward
        if (Mathf.Abs(_cmd.forwardMove) < 0.001 || Mathf.Abs(wishspeed) < 0.001)
            return;
        zspeed = playerVelocity.y;
        //playerVelocity.y = 0;
        /* Next two lines are equivalent to idTech's VectorNormalize() */
        speed = playerVelocity.magnitude;
        playerVelocity.Normalize();

        dot = Vector3.Dot(playerVelocity, wishdir);
        k = 32;
        k *= airControl * dot * dot * Time.deltaTime;

        // Change direction while slowing down
        if (dot > 0)
        {
            playerVelocity.x = playerVelocity.x * speed + wishdir.x * k;
            playerVelocity.y = playerVelocity.y * speed + wishdir.y * k;
            playerVelocity.z = playerVelocity.z * speed + wishdir.z * k;

            playerVelocity.Normalize();
            moveDirectionNorm = playerVelocity;
        }

        playerVelocity.x *= speed;
        playerVelocity.y = zspeed; // Note this line
        playerVelocity.z *= speed;
    }

    /**
     * Called every frame when the engine detects that the player is on the ground
     */
    private void GroundMove()
    {
        Vector3 wishdir;

        //Tilts the view when moving left or right
        if (_cmd.rightMove == -1 && viewTilt < maxTilt)
            viewTilt += tiltSpeed;
        if (_cmd.rightMove == 1 && viewTilt > -maxTilt)
            viewTilt -= tiltSpeed;
        if (_cmd.rightMove == 0 && viewTilt > 0)
            viewTilt -= tiltSpeed;
        if (_cmd.rightMove == 0 && viewTilt < 0)
            viewTilt += tiltSpeed;

        // Do not apply friction if the player is queueing up the next jump
        if (!wishJump)
            ApplyFriction(1f);
        else
            ApplyFriction(0);

        float scale = CmdScale();

        wishdir = new Vector3(_cmd.rightMove, 0, _cmd.forwardMove);
        wishdir = transform.TransformDirection(wishdir);
        wishdir.Normalize();
        moveDirectionNorm = wishdir;

        var wishspeed = wishdir.magnitude;
        wishspeed *= moveSpeed;

        Accelerate(wishdir, wishspeed, runAcceleration);

        // Reset the gravity velocity
			playerVelocity.y = 0;

		// Reset Jumpshot mechanic
        jumpSuccess = false;
        shotJumpRemainTime = shotJumpSetTime;

        if (wishJump)
        {
            jumpSuccess = true;
            playerVelocity.y = jumpSpeed;
            FindObjectOfType<AudioManager>().Play("jump");
            shotJumpRemainTime -= Time.deltaTime;
            wishJump = false;
        }
    }

    /**
     * Applies friction to the player, called in both the air and on the ground
     */
    private void ApplyFriction(float t)
    {
        Vector3 vec = playerVelocity; // Equivalent to: VectorCopy();
        float speed;
        float newspeed;
        float control;
        float drop;

        vec.y = 0.0f;
        speed = vec.magnitude;
        drop = 0.0f;

        /* Only if the player is on the ground then apply friction */
        if (_controller.isGrounded)
        {
            control = speed < runDeacceleration ? runDeacceleration : speed;
            drop = control * friction * Time.deltaTime * t;
        }

        newspeed = speed - drop;
        playerFriction = newspeed;
        if (newspeed < 0)
            newspeed = 0;
        if (speed > 0)
            newspeed /= speed;

        playerVelocity.x *= newspeed;
        // playerVelocity.y *= newspeed;
        playerVelocity.z *= newspeed;
    }

    private void Accelerate(Vector3 wishdir, float wishspeed, float accel)
    {
        float addspeed;
        float accelspeed;
        float currentspeed;

        currentspeed = Vector3.Dot(playerVelocity, wishdir);
        addspeed = wishspeed - currentspeed;
        if (addspeed <= 0)
            return;
        accelspeed = accel * Time.deltaTime * wishspeed;
        if (accelspeed > addspeed)
            accelspeed = addspeed;

        playerVelocity.x += accelspeed * wishdir.x;
        playerVelocity.z += accelspeed * wishdir.z;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 400, 100), "FPS: " + fps, style);
        var ups = _controller.velocity;
        ups.y = 0;
        GUI.Label(new Rect(0, 15, 400, 100), "Speed: " + Mathf.Round(ups.magnitude * 100) / 100 + "ups", style);
        GUI.Label(new Rect(0, 30, 400, 100), "Top Speed: " + Mathf.Round(playerTopVelocity * 100) / 100 + "ups", style);
    }


    private float CmdScale()
    {
        int max;
        float total;
        float scale;

        max = (int)Mathf.Abs(_cmd.forwardMove);
        if (Mathf.Abs(_cmd.rightMove) > max)
            max = (int)Mathf.Abs(_cmd.rightMove);
        if (max <= 0)
            return 0;

        total = Mathf.Sqrt(_cmd.forwardMove * _cmd.forwardMove + _cmd.rightMove * _cmd.rightMove);
        scale = moveSpeed * max / (moveScale * total);

        return scale;
    }
}