using UnityEngine;

public class BladesOfAttack : MonoBehaviour {
  public float baseAttackDamage = 1f;
  private void Start() {
    GameManager.Instance.playerData.baseAttackDamage = GameManager.Instance.playerData.baseAttackDamage + baseAttackDamage;
  }
}
