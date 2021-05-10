using UnityEngine;

public class BootsOfSpeed : MonoBehaviour {
  public float moveSpeed = 0.5f;
  private void Start() {
    GameManager.Instance.playerData.moveSpeed = GameManager.Instance.playerData.moveSpeed + moveSpeed;
  }
}
