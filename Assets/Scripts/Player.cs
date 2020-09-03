using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float xSensitivity;
    [SerializeField]
    private float ySensitivity;

    private Camera cam;

    private float time = 0.0f;
    public float timeBetween = 0.5f;

    private Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Theme", gameObject);
        cam = FindObjectOfType<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        rigidbody = GetComponent<Rigidbody>();
        Physics.IgnoreLayerCollision(9, 10);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);
        Vector3 direction = Quaternion.Euler(0f, cam.transform.eulerAngles.y, 0f) * new Vector3(playerInput.x, 0f, playerInput.y);

        direction.y = rigidbody.velocity.y / speed;

        if (time >= Time.deltaTime) {
            time = time - timeBetween;
            if (direction == Vector3.zero) {
            }
            else {
                AkSoundEngine.PostEvent("Footsteps", gameObject);
            }

        }

        rigidbody.velocity = direction * speed;

        Vector3 rotateValue = new Vector3(0f, -Input.GetAxis("Mouse X"), 0f) * xSensitivity;
        transform.eulerAngles -= rotateValue;
        rotateValue = new Vector3(Input.GetAxis("Mouse Y"), 0f) * ySensitivity;
        Vector3 currentCamRotation = cam.transform.eulerAngles - rotateValue;
        if (currentCamRotation.x < 271 && currentCamRotation.x > 180) {
            currentCamRotation.x = 271;
        } else if (currentCamRotation.x > 89 && currentCamRotation.x < 180) {
            currentCamRotation.x = 89;
        }
        cam.transform.eulerAngles = currentCamRotation;
    }
}
