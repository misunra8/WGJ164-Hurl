using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour {

    [SerializeField]
    private ThrowableItem throwing;

    [SerializeField]
    private GameObject targetPrefab;

    [SerializeField]
    private LayerMask targeting;

    [SerializeField]
    private float maxDist;

    [SerializeField]
    private float throwSpeed;

    private Camera cam;

    private Switch currentTarget;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDist, targeting.value)) {
                if (hit.collider.GetComponent<Switch>()) {
                    Switch newTarget = hit.collider.GetComponent<Switch>();
                    if (currentTarget && currentTarget != newTarget) {
                        currentTarget.SetAim(false);
                    }
                    currentTarget = newTarget;
                    currentTarget.SetAim(true);
                }
            }
        }
        if (Input.GetMouseButtonDown(0) && currentTarget) {
            ThrowableItem ti = Instantiate(throwing, transform.position, Quaternion.identity).GetComponent<ThrowableItem>();
            ti.SetTarget(currentTarget.transform);
            ti.Throw(transform.position, cam.transform.forward, throwSpeed);
        }
    }
}
