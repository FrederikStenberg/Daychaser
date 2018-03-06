using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPickup : MonoBehaviour {

    [HideInInspector]
    public string currentPhase = "night";

    GameObject[] LightSourcesInScene;
    int collectedLightSources = 0;
    int gotAllChecker = 0;

    private void Start()
    {
        LightSourcesInScene = GameObject.FindGameObjectsWithTag("collectableLightSource");
        gotAllChecker = LightSourcesInScene.Length;
    }

    private void Update()
    {
        if (collectedLightSources == gotAllChecker)
        {
            currentPhase = "day";
            Debug.Log("IT'S DAY");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "collectableLightSource")
        {
            Debug.Log("I'm collecting");
            Destroy(collision.gameObject);
            collectedLightSources += 1;
        }
    }
}
