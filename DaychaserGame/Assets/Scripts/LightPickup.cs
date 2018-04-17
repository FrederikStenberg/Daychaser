using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPickup : MonoBehaviour {

    [HideInInspector]
    public string currentPhase = "night";

    public GameObject directLight;
    public GameObject[] LightSourcesInScene;

    float skyboxLerpDuration;
    float skyboxBlend;
    int collectedLightSources = 0;
    int gotAllChecker = 0;
    bool lerpMaterial = false;

    private void Start()
    {
        LightSourcesInScene = GameObject.FindGameObjectsWithTag("collectableLightSource");
        gotAllChecker = LightSourcesInScene.Length;
    }

    private void Update()
    {
        if (collectedLightSources == gotAllChecker || (Input.GetKey(KeyCode.P)))
        {
            currentPhase = "day";
            Debug.Log("IT'S DAY");
            lerpMaterial = true;           
        }

        if(lerpMaterial == true)
        {
            Debug.Log(skyboxBlend);
            skyboxLerpDuration += 0.3f * Time.deltaTime;
            skyboxBlend = Mathf.Lerp(0, 1, skyboxLerpDuration);
            directLight.GetComponent<Light>().intensity = Mathf.Lerp(0, 1, skyboxLerpDuration);
        }

        RenderSettings.skybox.SetFloat("_Blend", skyboxBlend);       
    }

    private void OnApplicationQuit()
    {
        RenderSettings.skybox.SetFloat("_Blend", 0);
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
