using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    public GameObject testPrefab;
    public LayerMask targeting;
    public float maxDist;
    private Camera cam;
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
                Debug.DrawLine(ray.origin, hit.point);
                Instantiate(testPrefab, hit.point, Quaternion.identity);
            }
        }
    }
}
