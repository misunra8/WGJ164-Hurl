using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField]
    private float riseTime = 1.5f;

    [SerializeField]
    private float waitTime = 1.5f;

    private float currentRiseTime, currentWaitTime;

    private bool rising;

    private float startY;

    // Start is called before the first frame update
    void Start()
    {
        rising = false;
        currentRiseTime = 0f;
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentRiseTime < riseTime) {
            currentRiseTime = Mathf.Clamp(currentRiseTime + Time.deltaTime, 0f, riseTime);
            float completed = currentRiseTime / riseTime;
            if (!rising) {
                completed = 1f - completed;
            }
            Vector3 pos = transform.position;
            pos.y = Mathf.Lerp(startY - 5f, startY, completed);
            transform.position = pos;
            if (currentRiseTime == riseTime) {
                currentWaitTime = 0f;
            }
        } else {
            currentWaitTime = Mathf.Clamp(currentWaitTime + Time.deltaTime, 0f, waitTime);
            if (currentWaitTime == waitTime) {
                currentRiseTime = 0;
                rising = !rising;
            }
        }
    }
}
