using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objects : MonoBehaviour
{
    public float objecthealth = 100f;
    public void objecthitdamage(float amount)
    {
        objecthealth -= amount;
        if(objecthealth <= 0f)
        {
            die();
        }
    }
    void die()
    {
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
