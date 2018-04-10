// Jimmy Vegas Unity Tutorials
// This Script will track your health value in episode 005

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMonitor : MonoBehaviour
{

    public static int healthValue;
    public int internalHealth;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public Text lightText;
    public LightPickup lightScript;
    public BossTemp bossScript;
    public Text bossText;



    void Start()
    {
        healthValue = 3;
        if (internalHealth != 0)
            healthValue = internalHealth;
    }


    void Update()
    {
        internalHealth = healthValue;

        if (healthValue >= 1)
        {
            heart1.SetActive(true);
        }
        if (healthValue >= 2)
        {
            heart2.SetActive(true);
        }
        if (healthValue >= 3)
        {
            heart3.SetActive(true);
        }

        lightText.text = lightScript.LightSourcesInScene.Length.ToString();
        bossText.text = bossScript.currentHealth.ToString();
    }
}