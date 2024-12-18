using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footsteps : MonoBehaviour
{
    public AudioSource audioSource;
    [Header("footsteps sources")]
    public AudioClip[] footstepsound;

    private AudioClip getrandomfootstep()
    {
        return footstepsound[Random.Range(0, footstepsound.Length)];
    }
    private void step()
    {
        AudioClip clip = getrandomfootstep();
        audioSource.PlayOneShot(clip);
    }
    // Start is called before the first frame update
    
}
