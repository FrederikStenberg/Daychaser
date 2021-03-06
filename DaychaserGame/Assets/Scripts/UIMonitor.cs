﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMonitor : MonoBehaviour
{

    public static int healthValue;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public Text lightText;
    public LightPickup lightScript;
    public BossTemp bossScript;
    public Text bossText;



    void Start()
    {
        healthValue = GameObject.Find("Player").GetComponent<Player>().maxHealth;
    }


    void Update()
    {
        healthValue = GameObject.Find("Player").GetComponent<Player>().currentHealth;

        if (healthValue == 3)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
        }
        if (healthValue == 2)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(false);
        }
        if (healthValue == 1)
        {
            heart1.SetActive(true);
            heart2.SetActive(false);
            heart3.SetActive(false);
        }
        if (healthValue == 0)
        {
            heart1.SetActive(false);
            heart2.SetActive(false);
            heart3.SetActive(false);
        }

        lightText.text = lightScript.collectedLightSources.ToString() + "/" + lightScript.LightSourcesInScene.Length.ToString();
        bossText.text = bossScript.currentHealth.ToString();
    }
}