using UnityEngine;

public class SwingCollisions : MonoBehaviour {
  private PlayerController player;

  private void Start() {
    player = PlayerManager.Instance.GetPlayer();
  }
  private void OnCollisionEnter2D(Collision2D other) {
    if (other.gameObject.layer != 9) return;

    var controller = other.gameObject.GetComponent<GenericEnemyController>();
    PlayerData data = GameManager.Instance.playerData;
    if (player.lastAttackType == 3)
      controller.TakeHit(data.baseAttackDamage * data.heavyAttackMultiplier);
    else controller.TakeHit(data.baseAttackDamage);
  }
}