using UnityEngine;

public class CameraMovement : MonoBehaviour {
  [HideInInspector]
  public Transform target;

  public float cameraSpeed = 0.125f;
  public Vector3 offset;

  public void Start() {
    target = PlayerManager.Instance.player.transform;
  }

  public void LateUpdate() {
    transform.position = Vector3.Lerp(transform.position, target.position + offset, cameraSpeed);
  }
}
