using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider healthbarslider;


    public void givefullhealth(float health)
    {
        healthbarslider.maxValue = health;
        healthbarslider.value = health;
    }
    public void sethealth(float health)
    {
        healthbarslider.value = health;

    }
}
