using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetToFollow;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void Start()
    {
        if (targetToFollow != null)
        {
            offset = transform.position - targetToFollow.position;
        }
    }

    void LateUpdate()
    {
        if (targetToFollow == null) return;
        Vector3 desiredPosition = targetToFollow.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}
