using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField]
    private float acceptedAngle = 30f;

    private bool on = false;

    private Transform lever;

    [SerializeField]
    private float switchTime = 0.5f;

    private float currentSwitchTime;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in transform) {
            if (t.name == "Lever") {
                lever = t;
            }
        }
        currentSwitchTime = switchTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSwitchTime < switchTime) {
            currentSwitchTime = Mathf.Clamp(currentSwitchTime + Time.deltaTime, 0f, switchTime);
            float completed = currentSwitchTime / switchTime;
            if (on) {
                completed = 1f - completed;
            }
            float currentAngle = Mathf.Lerp(-60f, -120f, completed);
            lever.localEulerAngles = new Vector3(currentAngle, 0f);
        }
    }

    private void SwapLever() {
        on = !on;
        currentSwitchTime = 0f;
    }

    public void CheckLeverSwitch(Vector3 direction) {
        float angle;
        if (on) {
            angle = Vector3.Angle(direction, -transform.forward);
        } else {
            angle = Vector3.Angle(direction, transform.forward);
        }
        if (angle <= acceptedAngle) {
            SwapLever();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        // perfect
        Gizmos.DrawCube(transform.position + transform.forward, Vector3.one);
    }
}
