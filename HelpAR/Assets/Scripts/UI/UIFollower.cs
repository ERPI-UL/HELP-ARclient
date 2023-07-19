using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor.Rendering;
using UnityEngine;

public class UIFollower : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private Vector3 targetPos;
    private Vector3 targetOrient;
    public float minDist;
    public float maxDist;
    public float maxAngle;
    public float minAngle;
    public float maxOrientation;
    public float speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = Vector2.zero;
        
        Vector2 shift = new Vector2(targetPos.x, targetPos.z) - new Vector2(cam.transform.position.x, cam.transform.position.z);
        float distance = 0f;
        Vector2 direction = new Vector2(cam.transform.forward.x, cam.transform.forward.z);
        if (!(shift == Vector2.zero))
        {
            distance = Vector2.Distance(shift, Vector2.zero);
            direction = shift.normalized;
        }

        if (distance > maxDist)
        {
            movement -= direction * (distance - maxDist);
        }
        if (distance < minDist)
        {
            movement += direction * (minDist - distance);
        }

        float camAngle = cam.transform.rotation.eulerAngles.y;
        float angleShift = Vector2.SignedAngle(
            new Vector2(shift.x, shift.y),
            new Vector2(cam.transform.forward.x, cam.transform.forward.z)
        );
        
        if (angleShift < minAngle)
        {
            targetPos = new Vector3(
                Mathf.Sin((minAngle + camAngle) * Mathf.Deg2Rad) * distance,
                0,
                Mathf.Cos((minAngle + camAngle) * Mathf.Deg2Rad) * distance
            ) + cam.transform.position;
        }
        if (angleShift > maxAngle)
        {
            targetPos = new Vector3(
                Mathf.Sin((maxAngle + camAngle) * Mathf.Deg2Rad) * distance,
                0,
                Mathf.Cos((maxAngle + camAngle) * Mathf.Deg2Rad) * distance
            ) + cam.transform.position;
        }

        targetPos += new Vector3(movement.x, 0f, movement.y);
        targetPos.y = cam.transform.position.y - 0.15f;
        transform.position += (targetPos - transform.position) * Time.deltaTime * speed;
        if (distance > 2) transform.position = targetPos;

        angleShift = Vector2.Angle(
            new Vector2(transform.forward.x, transform.forward.z),
            new Vector2(shift.x, shift.y)
        );
        
        if (angleShift > maxOrientation)
        {
            targetOrient += (cam.transform.position - targetOrient) * Time.deltaTime * speed;
        }
        if (Mathf.Abs(angleShift) > 90) targetOrient = cam.transform.position;
        transform.LookAt(targetOrient);
        transform.rotation = Quaternion.Euler(0, 180 + transform.rotation.eulerAngles.y, 0);
    }
}
