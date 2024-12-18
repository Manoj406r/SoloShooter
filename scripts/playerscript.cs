using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerscript : MonoBehaviour
{
    [Header("player health things")]
    private float playerhealth = 1000f;
    private float presenthealth;
    public healthbar healthbar;




    [Header("player movement")]
    public float playerspeed = 1.9f;
    public float currentplayerspeed = 0f;
    public float playersprint = 3f;
    public float currentplayersprint = 0f;

    [Header("player camera")]
    public Transform playercamera;

    [Header("player animator and gravity")]
    public CharacterController cc;
    public float gravity = -9.8f;
    public Animator animator;

    [Header("player jumping & velocity")]
    public float jumprange = 1f;
    public float turncalmtime = 0.1f;
    float turncalmvelocity;
    Vector3 velocity;
    public Transform surfacecheck;
    bool onsurface;
    public float surfacedistance = 0.4f;
    public LayerMask surfacemask;

    public bool mobileinputs;
    public FixedJoystick joystick;
    public FixedJoystick sprintjoystick;
    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        presenthealth = playerhealth;
        healthbar.givefullhealth(playerhealth);
    }
    void Update()
    {
        if(currentplayerspeed >0)
        {
            sprintjoystick = null;
        }
        else
        {
            FixedJoystick sprintjs = GameObject.Find("playersprintjoystick").GetComponent<FixedJoystick>();
            sprintjoystick = sprintjs;
        }
        //surfacecheck
        onsurface = Physics.CheckSphere(surfacecheck.position, surfacedistance, surfacemask);
        if(onsurface && velocity.y <0)
        {
            velocity.y = -2f;
        }
        //gravity
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
        playmove();
        jump();
        sprint();
    }
    void playmove()
    {
        if (mobileinputs == true)
        {
            float horizontal_axis = joystick.Horizontal;
            float vertical_axis = joystick.Vertical;

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("walk", true);
                animator.SetBool("running", false);
                animator.SetBool("idle", false);
                animator.SetTrigger("jump");
                animator.SetBool("aimwalk", false);
                animator.SetBool("idleaim", false);

                float targetangle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playercamera.eulerAngles.y;
                //player rotation
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turncalmvelocity, turncalmtime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                //player movement
                Vector3 movedirection = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;
                cc.Move(movedirection.normalized * playerspeed * Time.deltaTime);
                currentplayerspeed = playerspeed;
            }
            else
            {
                animator.SetBool("idle", true);
                animator.SetTrigger("jump");
                animator.SetBool("walk", false);
                animator.SetBool("running", false);
                animator.SetBool("aimwalk", false);
                currentplayerspeed = 0f;

            }
        }
        else
        {
            //get axis
            float horizontal_axis = Input.GetAxisRaw("Horizontal");
            float vertical_axis = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("walk", true);
                animator.SetBool("running", false);
                animator.SetBool("idle", false);
                animator.SetTrigger("jump");
                animator.SetBool("aimwalk", false);
                animator.SetBool("idleaim", false);

                float targetangle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playercamera.eulerAngles.y;
                //player rotation
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turncalmvelocity, turncalmtime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                //player movement
                Vector3 movedirection = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;
                cc.Move(movedirection.normalized * playerspeed * Time.deltaTime);
                currentplayerspeed = playerspeed;
            }
            else
            {
                animator.SetBool("idle", true);
                animator.SetTrigger("jump");
                animator.SetBool("walk", false);
                animator.SetBool("running", false);
                animator.SetBool("aimwalk", false);
                currentplayerspeed = 0f;

            }
        }
    }

    void jump()
    {
        if(Input.GetButtonDown() && onsurface)
        {
            animator.SetBool("walk", false);
            animator.SetTrigger("jump");
            velocity.y = Mathf.Sqrt(jumprange * -2 * gravity);
        }
        else
        {
            animator.ResetTrigger("jump");
        }
    }
    void sprint()
    {
        if (mobileinputs == true)
        {
            float horizontal_axis = sprintjoystick.Horizontal;
            float vertical_axis = sprintjoystick.Vertical;

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if (direction.magnitude >= 0.1f)
            { 
                animator.SetBool("running", true);
             
                animator.SetBool("idle", false);
                animator.SetBool("walk", false);
                animator.SetBool("idleaim", false);

                float targetangle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playercamera.eulerAngles.y;
                //player rotation
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turncalmvelocity, turncalmtime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                //player movement
                Vector3 movedirection = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;
                cc.Move(movedirection.normalized * playersprint* Time.deltaTime);
                currentplayersprint = playersprint;
            }
            else
            {
                animator.SetBool("idle", true);
                animator.SetBool("walk", false);
                currentplayersprint = 0f;

            }
        }
        else
        {
            //get axis
            float horizontal_axis = Input.GetAxisRaw("Horizontal");
            float vertical_axis = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("running", true);

                animator.SetBool("idle", false);
                animator.SetBool("walk", false);
                animator.SetBool("idleaim", false);

                float targetangle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playercamera.eulerAngles.y;
                //player rotation
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turncalmvelocity, turncalmtime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                //player movement
                Vector3 movedirection = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;
                cc.Move(movedirection.normalized * playersprint * Time.deltaTime);
                currentplayersprint = playersprint;
            }
            else
            {
                animator.SetBool("idle", true);
                animator.SetBool("walk", false);
                currentplayersprint = 0f;

            }
        }
    }
    //player damage
    public void playerhitdamage(float takedamage)
    {
        presenthealth -= takedamage;
        healthbar.sethealth(presenthealth);
        if(presenthealth <= 0)
        {
            playerdie();
        }
    }
    private void playerdie()
    {
        Cursor.lockState = CursorLockMode.None;
        Object.Destroy(gameObject);
    }
}
