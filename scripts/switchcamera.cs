using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchcamera : MonoBehaviour
{
    [Header("camera to assign")]
    public GameObject aimcam;
    public GameObject aimcanvas;
    public GameObject thirdpercamera;
    public GameObject thirdpercanvas;

    [Header("camera animator")]
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire2")&& Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("idle", false);
            animator.SetBool("idleaim", true);
            animator.SetBool("aimwalk", true);
            animator.SetBool("walk", true);

            thirdpercamera.SetActive(false);
            thirdpercanvas.SetActive(false);
            aimcam.SetActive(true);
            aimcanvas.SetActive(true);
        }
        else if(Input.GetButton("Fire2"))
        {
            animator.SetBool("idle", false);
            animator.SetBool("idleaim", true);
            animator.SetBool("aimwalk", false);
            animator.SetBool("walk", false);



            thirdpercamera.SetActive(false);
            thirdpercanvas.SetActive(false);
            aimcam.SetActive(true);
            aimcanvas.SetActive(true);


        }
        else
        {
            animator.SetBool("idle", true);
            animator.SetBool("idleaim", false);
            animator.SetBool("aimwalk", false);
         

            thirdpercamera.SetActive(true);
            thirdpercanvas.SetActive(true);
            aimcam.SetActive(false);
            aimcanvas.SetActive(false);
        }
    }
}
