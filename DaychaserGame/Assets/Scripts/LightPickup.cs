using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPickup : MonoBehaviour {

    [HideInInspector]
    public string currentPhase = "night";

    public GameObject directLight;
    public Material daySkybox;

    public GameObject[] LightSourcesInScene;
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
            RenderSettings.skybox = daySkybox;
            directLight.GetComponent<Light>().enabled = true;
        }
    }

    GameObject currentObj;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "collectableLightSource")
        {
            if (hit.gameObject != currentObj)
            {
                Destroy(hit.gameObject);
                collectedLightSources += 1;
                Debug.Log("I'm collecting");
            }
            currentObj = hit.gameObject;
        }
    }
}
