using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPickup : MonoBehaviour {

    [HideInInspector]
    public string currentPhase = "night";

    public GameObject directLight;

    public GameObject[] LightSourcesInScene;
    public Material daySkybox;
    int collectedLightSources = 0;
    int gotAllChecker = 0;

    bool changePhaseOnce = true;

    private void Start()
    {
        LightSourcesInScene = GameObject.FindGameObjectsWithTag("collectableLightSource");
        gotAllChecker = LightSourcesInScene.Length;
    }

    private void Update()
    {
        if (collectedLightSources == gotAllChecker && changePhaseOnce == true)
        {
            currentPhase = "day";
            directLight.GetComponent<Light>().enabled = true;
            RenderSettings.skybox = daySkybox;
            changePhaseOnce = false;
        }
    }

    GameObject currentObj;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "collectableLightSource")
        {
            if(collision.gameObject != currentObj)
            {
                Destroy(collision.gameObject);
                collectedLightSources += 1;
                Debug.Log("I'm collecting");
            }
            currentObj = collision.gameObject;
        }
    }
}
