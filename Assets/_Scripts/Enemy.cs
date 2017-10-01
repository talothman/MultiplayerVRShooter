using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    float rotationSpeedx;
    float rotationSpeedy;
    float rotationSpeedz;

    Renderer mRenderer;

    float timePassed = 0;
    float randomTimeToPass;

	// Use this for initialization
	void Start () {
        rotationSpeedx = Random.Range(0f, 1f);
        rotationSpeedy = Random.Range(0f, 1f);
        rotationSpeedz = Random.Range(0f, 1f);

        mRenderer = GetComponent<Renderer>();

        randomTimeToPass = Random.Range(0f, 3f);
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(rotationSpeedx, rotationSpeedy, rotationSpeedz));

        if (timePassed >= randomTimeToPass)
        {
            mRenderer.material.color = Random.ColorHSV();
            timePassed = 0;
        }

        timePassed += Time.deltaTime;
    }
}
