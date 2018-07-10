using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour {

    Light testLight;
    public float minWaitTime;
    public float maxWaitTime;

    void Start() {
		if(minWaitTime == 0f && maxWaitTime == 0f) {
			minWaitTime = Random.Range(0.1f, 0.3f);
			maxWaitTime = Random.Range(0.3f, 5);
		}
        testLight = GetComponent<Light>();
        StartCoroutine(Flashing());
    }

    IEnumerator Flashing()
    {
        while (true)
        {
            // can change waitforseconds() to be an actual time in seconds
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            testLight.enabled = !testLight.enabled;

        }
    }
}
