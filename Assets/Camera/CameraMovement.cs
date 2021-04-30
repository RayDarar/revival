using UnityEngine;

public class CameraMovement : MonoBehaviour
{
  public Transform target;

  public float cameraSpeed = 0.125f;
  public Vector3 offset;

  public void LateUpdate()
  {
    transform.position = Vector3.Lerp(transform.position, target.position + offset, cameraSpeed);
  }
}
