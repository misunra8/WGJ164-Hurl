using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public KeyCode forward, left, right, backwards;

    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(forward) && !Input.GetKey(backwards)) {
            direction += transform.forward;
        } else if (Input.GetKey(backwards) && !Input.GetKey(forward)) {
            direction -= transform.forward;
        }

        if (Input.GetKey(right) && !Input.GetKey(left)) {
            direction += transform.right;
        } else if (Input.GetKey(left) && !Input.GetKey(right)) {
            direction -= transform.right;
        }

        transform.position += direction.normalized * speed * Time.deltaTime;

        Vector3 rotateValue = new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0f);
        transform.eulerAngles = transform.eulerAngles - rotateValue;
        //transform.rotation = Quaternion.Euler(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0f);
    }
}
