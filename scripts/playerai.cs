using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class playerai : MonoBehaviour
{

    // Start is called before the first frame update
    [Header("player health and damage")]
    private float playerhealth = 120f;
    private float presenthealth;
    public float givedamage = 5f;
    public float playerspeed;

    [Header("player things")]
    public NavMeshAgent playeragent;
    public Transform lookpoint;
    public GameObject shootingraycastarea;
    public Transform enemybody;
    public LayerMask enemylayer;
    public Transform spawn;
    public Transform playercharacter;

    [Header("player shooting var")]
    public float timebtwshoot;
    bool previouslyshoot;

    [Header("player animation and spark effect")]
    public Animator anim;
    public ParticleSystem muzzlespark;

    [Header("player states")]
    public float visionradius;
    public float shootingradius;
    public bool enemyinvisionradius;
    public bool enemyinshootingradius;
    [Header("sounds")]
    public AudioSource audioSource;
    public AudioClip shootingsound;

    public scoremanager scoremanager;
    
    // Start is called before the first frame update
    private void Awake()
    {
        playeragent = GetComponent<NavMeshAgent>();
        presenthealth = playerhealth;
    }

    // Update is called once per frame
    private void Update()
    {
        enemyinvisionradius = Physics.CheckSphere(transform.position, visionradius, enemylayer);
        enemyinshootingradius = Physics.CheckSphere(transform.position, shootingradius, enemylayer);

        if (enemyinvisionradius && !enemyinshootingradius) pursueenemy();
        if (enemyinvisionradius && enemyinshootingradius) shootenemy();
    }
    private void pursueenemy()
    {
        if (playeragent.SetDestination(enemybody.position))
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
    private void shootenemy()
    {
        playeragent.SetDestination(transform.position);
        transform.LookAt(lookpoint);

        if (!previouslyshoot)
        {
            muzzlespark.Play();
            audioSource.PlayOneShot(shootingsound);
            RaycastHit hit;
            if (Physics.Raycast(shootingraycastarea.transform.position, shootingraycastarea.transform.forward, out hit, shootingradius))
            {
                Debug.Log("shooting" + hit.transform.name);

                enemy enemy = hit.transform.GetComponent<enemy>();
                if (enemy != null)
                {
                    enemy.enemyhitdamage(givedamage);
                    
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
    public void playeraihitdamage(float takedamage)
    {
        presenthealth -= takedamage;
        if (presenthealth <= 0)
        {
            StartCoroutine(respawn());
        }
    }
    IEnumerator respawn()
    {
        playeragent.SetDestination(transform.position);
        playerspeed = 0f;
        shootingradius = 0f;
        visionradius = 0f;
        enemyinvisionradius = false;
        enemyinshootingradius = false;
        anim.SetBool("die", true);
        anim.SetBool("running", false);
        anim.SetBool("shooting", false);
        //animations
        Debug.Log("dead");
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        scoremanager.enemykills += 1;
        yield return new WaitForSeconds(5f);
        Debug.Log("spawn");
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        presenthealth = 120f;
        playerspeed = 1f;
        shootingradius = 10f;
        visionradius = 100f;
        enemyinvisionradius = true;
        enemyinshootingradius = false;

        // animations
        anim.SetBool("die", false);
        anim.SetBool("running", true);

        //spawnpoint
        playercharacter.transform.position = spawn.transform.position;
        pursueenemy();
    }
}
