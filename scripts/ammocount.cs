using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ammocount : MonoBehaviour
{
    public Text ammunitiontext;
    public Text magtext;


    public static ammocount occurence;
    private void Awake()
    {
        occurence = this;
    }
    public void updateammotext(int presentammunition)
    {
        ammunitiontext.text = "Ammo." + presentammunition;
    }
    public void updatemagtext(int mag)
    {
        magtext.text = "Magazines." + mag;
    }

}
