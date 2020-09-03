using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private Door counterDoor;

    [SerializeField]
    private float transitionTime;

    private float currentTransitionTime;

    private bool open = false;

    private Transform locker, bottom, top;

    private Vector3 lockStart = new Vector3(0f, 1f);
    private Vector3 bottomStart = new Vector3(0f, 0.5f);
    private Vector3 topStart = new Vector3(0f, 1.75f);

    private Vector3 lockEnd = Vector3.zero;
    private Vector3 bottomEnd = new Vector3(0f, -0.5f);
    private Vector3 topEnd = new Vector3(0f, 3.25f);

    private BoxCollider doorCollider;
    // Start is called before the first frame update
    void Start()
    {
        currentTransitionTime = transitionTime;
        foreach (Transform t in transform) {
            if (t.name == "Lock") {
                locker = t;
            } else if (t.name == "Bottom") {
                bottom = t;
            } else if (t.name == "Top") {
                top = t;
            }
        }
        if (counterDoor) {
            counterDoor.SetOpen(!open);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTransitionTime < transitionTime) {
            currentTransitionTime = Mathf.Clamp(currentTransitionTime + Time.deltaTime, 0f, transitionTime);
            float completed = currentTransitionTime / transitionTime;
            if (!open) {
                completed = 1f - completed;
            }
            if (completed > 0.5f) {
                float moveComplete = completed * 2f - 1f;
                locker.localPosition = Vector3.Lerp(lockStart, lockEnd, moveComplete);
                bottom.localPosition = Vector3.Lerp(bottomStart, bottomEnd, moveComplete);
                top.localPosition = Vector3.Lerp(topStart, topEnd, moveComplete);
            } else {
                float x = Mathf.Lerp(0, 180f, completed * 2f);
                locker.localEulerAngles = new Vector3(x, -90f, 90f);
            }
        }
    }

    public void SetOpen(bool state) {
        if (state) {
            AkSoundEngine.PostEvent("DoorOpen", gameObject);
        } else {
            AkSoundEngine.PostEvent("DoorClose", gameObject);
        }
        if (open != state) {
            open = state;
            currentTransitionTime = 0f;
            if (doorCollider == null) {
                doorCollider = GetComponent<BoxCollider>();
            }
            doorCollider.enabled = !open;
            if (counterDoor) {
                counterDoor.SetOpen(!open);
            }
        }
    }
    
}
