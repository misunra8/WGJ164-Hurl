using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField]
    private float acceptedAngle = 30f;

    private bool on = false;

    private Transform lever;

    private Transform baseBlock;
    private Transform leverBlock;

    [SerializeField]
    private float switchTime = 0.5f;

    private float currentSwitchTime;

    [SerializeField]
    private Door doorTarget;


    [SerializeField]
    private Shader baseShader;

    [SerializeField]
    private Shader selectShader;

    private bool aim;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in transform) {
            if (t.name == "Lever") {
                lever = t;
            } else if (t.name == "Base") {
                baseBlock = t;
            }
        }
        foreach (Transform t in lever) {
            if (t.name == "LeverBlock") {
                leverBlock = t;
            }
        }
        currentSwitchTime = switchTime;
        aim = false;
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
        if (doorTarget) {
            doorTarget.SetOpen(on);
        }
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

    public void SetAim(bool aim) {
        if (aim) {
            baseBlock.GetComponent<MeshRenderer>().materials[0].shader = selectShader;
            leverBlock.GetComponent<MeshRenderer>().materials[0].shader = selectShader;
        } else {
            baseBlock.GetComponent<MeshRenderer>().materials[0].shader = baseShader;
            leverBlock.GetComponent<MeshRenderer>().materials[0].shader = baseShader;
        }
        this.aim = aim;
        Debug.Log("Aim is now " + aim + " pointing at " + this);
    }
}
