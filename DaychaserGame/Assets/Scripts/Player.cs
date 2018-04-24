using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IPlayer{

    public int currentHealth;
    public int maxHealth;
	void Start () {
        if (maxHealth <= 0 || maxHealth > 3)
            maxHealth = 3;
        currentHealth = maxHealth;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void TakeDamage(int amount)
    {
        FindObjectOfType<AudioManager>().Play("grunt");
        Debug.Log("Player takes " + amount + " damage!");   
        currentHealth -= amount;
        if (currentHealth <= 0)
            die();

    }

    public GameObject deadItem;
    public GameObject sword;

    void die()
    {
        Debug.Log("You dead son");
        GetComponent<CharacterController>().enabled = false;
        GetComponent<Phase2Controller>().enabled = false;
        Destroy(sword);
        deadItem.SetActive(true);
    }

    public void PerformAttack()
    {

    }
}
