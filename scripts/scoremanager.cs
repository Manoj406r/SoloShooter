using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class scoremanager : MonoBehaviour
{
    [Header("score manager")]
    public int kills;
    public int enemykills;
    public Text playerkillcounter;
    public Text enemykillcounter;
    public Text maintext;

    private void Update()
    {
        StartCoroutine(winorlose());
    }
    IEnumerator winorlose()
    {
        playerkillcounter.text = "" + kills;
        enemykillcounter.text = "" + enemykills;

        if(kills >= 20)
        {
            maintext.text = "Blue Team Victory";
            PlayerPrefs.SetInt("kills", kills);
            Time.timeScale = 0f;
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("TDM room");
        }

        else if(enemykills >= 20)
        {
            maintext.text = "Red Team Victory";
            PlayerPrefs.SetInt("enemykills", enemykills);
            Time.timeScale = 0f;
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("TDM room");
        }
    }

}
