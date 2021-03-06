﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableItem : MonoBehaviour
{
    private Vector3 startPoint;
    private Vector3 startDirection;
    private Transform endTarget;

    // Set like so, we won't get into anything until Throw is called
    private float travelTime = -1f;
    private float currentTravelTime = 0f;

    private Vector3 circleCentre;
    private float radius;
    private float angle;

    private Vector3 currentDirection;

    [SerializeField]
    private float rotateSpeed;

    private Vector3 planeNormal;
    
    public void SetTarget(Transform target) {
        endTarget = target;
    }

    public void Throw(Vector3 start, Vector3 direction, float speed) {
        // Calculate the arc, then we will use whatever the current time is to
        // interpolate along the arc
        Vector3 endPoint = endTarget.position;
        startPoint = start;
        startDirection = direction;
        float distance = Vector3.Distance(startPoint, endPoint);
        Plane arcPlane = new Plane(startPoint, endPoint, 
                                    startPoint + startDirection.normalized);
        Vector3 normal = Vector3.Cross(direction.normalized, arcPlane.normal);
        Vector3 perpToAim = Vector3.Cross(endPoint - startPoint, arcPlane.normal);
        Vector3 midPoint = (startPoint + endPoint) * 0.5f;
        radius =
            ((perpToAim.y * (startPoint.x - midPoint.x)) -
            (perpToAim.x * (startPoint.y - midPoint.y))) /
            (normal.y * perpToAim.x - normal.x * perpToAim.y);
        circleCentre = startPoint + (radius * normal);

        // To find travelTime, we need to find out how far it has to go!
        Vector3 startAngle = startPoint - circleCentre;
        Vector3 endAngle = endPoint - circleCentre;
        angle = Vector3.Angle(startAngle, endAngle);
        bool reverse = Mathf.Abs(Vector3.Angle(direction, endPoint - startPoint)) > 90;
        if (reverse) {
            // Angle now acts as our reverse boolean, will be greater than 180
            // if we were aiming away from the target
            angle = 360f - angle;
        }
        float travelDist = 2 * Mathf.PI * radius * (angle / 360f);
        travelTime = travelDist / speed;
        currentTravelTime = 0f;

        planeNormal = arcPlane.normal;
        transform.rotation = Quaternion.FromToRotation(transform.right, arcPlane.normal) * transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTravelTime < travelTime) {
            AkSoundEngine.PostEvent("Spin", gameObject);
            currentTravelTime = Mathf.Clamp(currentTravelTime + Time.deltaTime, 0f, travelTime);
            Vector3 currentAngle;
            float travelled = currentTravelTime / travelTime;
            if (angle > 180f) {
                Vector3 midAngle = -Vector3.Slerp(startPoint - circleCentre, endTarget.position - circleCentre, 0.5f);
                if (travelled <= 0.5f) {
                    currentAngle = Vector3.Slerp(startPoint - circleCentre, midAngle, travelled * 2);
                } else {
                    currentAngle = Vector3.Slerp(midAngle, endTarget.position - circleCentre, travelled * 2f - 1f);
                }
            } else {
                currentAngle = Vector3.Slerp(
                    startPoint - circleCentre,
                    endTarget.position - circleCentre,
                    travelled
                );
            }
            Vector3 nextPos = circleCentre + (currentAngle.normalized * radius);
            currentDirection = nextPos - transform.position;
            transform.position = nextPos;
            transform.RotateAround(planeNormal, rotateSpeed * Time.deltaTime);
        }
    }

    public Vector3 GetMovingDirection() {
        return currentDirection;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawCube(transform.position + currentDirection.normalized*3, Vector3.one);
    }

    private void OnCollisionEnter(Collision collision) {
        Switch s = collision.gameObject.GetComponent<Switch>();
        if (s) {
            AkSoundEngine.PostEvent("StickHit", gameObject);
            Debug.Log("Hit a switch");
            s.CheckLeverSwitch(currentDirection);
        }
        if (currentTravelTime < travelTime) {
            currentTravelTime = travelTime;
        }
    }
}
