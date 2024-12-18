using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    [Header("enemy health and damage")]
    private float enemyhealth = 120f;
    private float presenthealth;
    public float givedamage = 5f;
    public float enemyspeed;

    [Header("enemy things")]
    public NavMeshAgent enemyagent;
    public Transform lookpoint;
    public GameObject shootingraycastarea;
    public Transform playerbody;
    public LayerMask playerlayer;
    public Transform spawn;
    public Transform enemycharacter;

    [Header("enemy shooting var")]
    public float timebtwshoot;
    bool previouslyshoot;

    [Header("enemy animation and spark effect")]
    public Animator anim;
    public ParticleSystem muzzlespark;

    [Header("enemy states")]
    public float visionradius;
    public float shootingradius;
    public bool playerinvisionradius;
    public bool playerinshootingradius;
    public bool isplayer = false;

    [Header("sounds")]
    public AudioSource audioSource;
    public AudioClip shootingsound;

    public scoremanager scoremanager;
    // Start is called before the first frame update
    private void Awake()
    {
        enemyagent = GetComponent<NavMeshAgent>();
        presenthealth = enemyhealth;
    }

    // Update is called once per frame
    private void Update()
    {
        playerinvisionradius = Physics.CheckSphere(transform.position, visionradius, playerlayer);
        playerinshootingradius = Physics.CheckSphere(transform.position, shootingradius, playerlayer);

        if (playerinvisionradius && !playerinshootingradius) pursueplayer();
        if (playerinvisionradius && playerinshootingradius) shootplayer();
    }
    private void pursueplayer()
    {
        if(enemyagent.SetDestination(playerbody.position))
        {
            //animation
            anim.SetBool("running", true);
            anim.SetBool("shooting", false);

        }
        else
        {
            anim.SetBool("running", false);
            anim.SetBool("shooting", false);
        }
     
    }
    private void shootplayer()
    {
        enemyagent.SetDestination(transform.position);
        transform.LookAt(lookpoint);

        if(!previouslyshoot)
        {
            muzzlespark.Play();
            audioSource.PlayOneShot(shootingsound);
            RaycastHit hit;
            if(Physics.Raycast(shootingraycastarea.transform.position,shootingraycastarea.transform.forward,out hit,shootingradius))
            {
                Debug.Log("shooting" + hit.transform.name);
                if(isplayer == true)
                {
                    playerscript playerbody = hit.transform.GetComponent<playerscript>();
                    if(playerbody != null)
                    {
                        playerbody.playerhitdamage(givedamage);
                    }
                }
                else
                {
                    playerai playerbody = hit.transform.GetComponent<playerai>();
                    if (playerbody != null)
                    {
                        playerbody.playeraihitdamage(givedamage);
                    }
                }

                anim.SetBool("running", false);
                anim.SetBool("shooting", true);
            }


            previouslyshoot = true;
            Invoke(nameof(activeshooting), timebtwshoot);
        }

    }
    private void activeshooting()
    {
        previouslyshoot = false;
    }
    public void enemyhitdamage(float takedamage)
    {
        presenthealth -= takedamage;
        if (presenthealth <= 0)
        {
            StartCoroutine(respawn());
        }
    }
    IEnumerator respawn()
    {
        enemyagent.SetDestination(transform.position);
        enemyspeed = 0f;
        shootingradius = 0f;
        visionradius = 0f;
        playerinvisionradius = false;
        playerinshootingradius = false;
        anim.SetBool("die", true);
        anim.SetBool("running", false);
        anim.SetBool("shooting", false);
        //animations
        Debug.Log("dead");
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        scoremanager.kills += 1;
        yield return new WaitForSeconds(5f);
        Debug.Log("spawn");
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        presenthealth = 120f;
        enemyspeed = 1f;
        shootingradius = 10f;
        visionradius = 100f;
        playerinvisionradius = true;
        playerinshootingradius = false;

        // animations
        anim.SetBool("die", false);
        anim.SetBool("running", true);

        //spawnpoint
        enemycharacter.transform.position = spawn.transform.position;
        pursueplayer();
    }
}
