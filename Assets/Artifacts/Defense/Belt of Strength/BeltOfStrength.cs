using UnityEngine;

public class BeltOfStrength : MonoBehaviour {
  public float maxHealth = 10f;
  private void Start() {
    GameManager.Instance.playerData.maxHealth = GameManager.Instance.playerData.maxHealth + maxHealth;
  }
}
