﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPickup : MonoBehaviour {

    [HideInInspector]
    public string currentPhase = "night";
    public int collectedLightSources = 0;

    public GameObject directLight;
    public GameObject[] LightSourcesInScene;
    public GameObject ghost;
    public GameObject ghostSpawn;
    public GameObject dayCycle;
    public GameObject toggleBossHealth;
    public float distanceForGhostEffect;
    public float ghostPushForce;
    public float playerPushForce;

    float skyboxLerpDuration;
    float skyboxBlend;
    float fogDensity = 0.04f;
    float fogDensityDuration = 0;   
    int gotAllChecker = 0;
    bool lerpMaterial = false;

    private void Start()
    {
        LightSourcesInScene = GameObject.FindGameObjectsWithTag("collectableLightSource");
        gotAllChecker = LightSourcesInScene.Length;
        FindObjectOfType<AudioManager>().Play("Nightmusic");
    }

    private void Update()
    {
        if (collectedLightSources == gotAllChecker || (Input.GetKey(KeyCode.P)))
        {
            FindObjectOfType<AudioManager>().sounds[5].source.Pause();
            currentPhase = "day";
            lerpMaterial = true;
            dayCycle.SetActive(true);
            toggleBossHealth.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Ghost"));
        }

        if(lerpMaterial == true)
        {
            skyboxLerpDuration += 0.3f * Time.deltaTime;
            fogDensityDuration -= 0.002f * Time.deltaTime;
            skyboxBlend = Mathf.Lerp(0, 1, skyboxLerpDuration);
            fogDensity = Mathf.Lerp(0, 0.6f, fogDensityDuration);
            directLight.GetComponent<Light>().intensity = Mathf.Lerp(0, 1, skyboxLerpDuration);
        }
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.skybox.SetFloat("_Blend", skyboxBlend);       
    }

    private void OnApplicationQuit()
    {
        RenderSettings.skybox.SetFloat("_Blend", 0);
    }

    GameObject currentObj;

    Vector3 tempVel;
    Vector3 playerTempVel;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "collectableLightSource")
        {
            if (hit.gameObject != currentObj)
            {
                Destroy(hit.gameObject);
                collectedLightSources += 1;
                if(Vector3.Distance(ghost.transform.position, transform.position) < distanceForGhostEffect)
                FindObjectOfType<AudioManager>().Play("pickup");
                {
                    ghost.GetComponent<GhostSteeringScript>().enabled = false;
                    tempVel = ghost.GetComponent<Rigidbody>().velocity;
                    ghost.GetComponent<Rigidbody>().velocity = -transform.forward * ghostPushForce;
                }
            }
            currentObj = hit.gameObject;
        }
    }

    public IEnumerator dontSpamDie()
    {
        GetComponent<Player>().TakeDamage(1);
        GetComponent<CharacterController>().Move(new Vector3(1,1,0) * 50 * Time.deltaTime);
        yield return new WaitForSeconds(2);
        if(currentPhase == "night")
        {
            Instantiate(ghost, ghostSpawn.transform.position, Quaternion.identity);
        }       
    }
}
