using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rifle : MonoBehaviour
{
    [Header("rifle")]
    public Camera cam;
    public float givedamage = 10f;
    public float shootingrange = 100f;
    public float firecharge = 15f;
    public playerscript player;
    public Animator animator;

    [Header("rifle ammunition and shooting")]
    private float nexttimetoshoot = 0f;
    private int maximumammunition = 20;
    private int mag = 15;
    private int presentammunition;
    public float reloadingtime = 1.3f;
    public bool setreloading = false;

    [Header("rifle effects")]
    public ParticleSystem muzzlespark;
    public GameObject woodedeffect;
    public GameObject goreeffect;
    [Header("sound effects")]
    public AudioSource audioSource;
    public AudioClip shootingsound;
    public AudioClip reloadingsound;
    private void Awake()
    {
        presentammunition = maximumammunition;
    }


    // Start is called before the first frame update

    private void Update()
    {
        if (setreloading)
            return;

        if(presentammunition <= 0)
        {
            StartCoroutine(reload());
            return;
        }
        if(Input.GetButtonDown("Fire1") && Time.time >= nexttimetoshoot)
        {
            animator.SetBool("fire", true);
            animator.SetBool("idle", false);
            nexttimetoshoot = Time.time + 1f / firecharge;
            shoot();
        }
        else if (Input.GetButton("Fire1") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("idle", false);
            animator.SetBool("firewalk", true);
          
        }
        else if(Input.GetButton("Fire1") && Input.GetButton("Fire2"))
        {
            animator.SetBool("idle", false);
            animator.SetBool("idleaim", true);
            animator.SetBool("firewalk", true);
            animator.SetBool("walk", true);
            animator.SetBool("reloading", false);
        }
        else
        {
            animator.SetBool("fire", false);
            animator.SetBool("idle", true);
            animator.SetBool("firewalk", false);
        }
    }
    void shoot()
    {
        if(mag ==0)
        {
            //show ammo out text
        }
        presentammunition--;

        if(presentammunition == 0)
        {
            mag--;
        }
        //update ui
        ammocount.occurence.updateammotext(presentammunition);
        ammocount.occurence.updatemagtext(mag);
        muzzlespark.Play();
        audioSource.PlayOneShot(shootingsound);
        RaycastHit hitinfo;

        if(Physics.Raycast(cam.transform.position,cam.transform.forward,out hitinfo,shootingrange))
        {
            Debug.Log(hitinfo.transform.name);
            objects objects = hitinfo.transform.GetComponent<objects>();
            enemy enemy = hitinfo.transform.GetComponent<enemy>();
            if (objects != null)
            {
                objects.objecthitdamage(givedamage);
                GameObject woodgo = Instantiate(woodedeffect, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
                Destroy(woodgo, 1f);
            }
            else if(enemy != null)
            {
                enemy.enemyhitdamage(givedamage);
                GameObject gorego = Instantiate(goreeffect, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
                Destroy(gorego, 1f);
            }
        }
    }
    IEnumerator reload()
    {
        player.playerspeed = 0f;
        player.playersprint = 0f;
        setreloading = true;
        Debug.Log("reloading");
        //animation and audio
        animator.SetBool("reloading", true);
        audioSource.PlayOneShot(reloadingsound);
        yield return new WaitForSeconds(reloadingtime);
        //animation
        animator.SetBool("reloading", false);
        presentammunition = maximumammunition;
        player.playerspeed = 1.9f;
        player.playersprint = 3f;
        setreloading = false;
    }
    // Update is called once per frame
   
}
