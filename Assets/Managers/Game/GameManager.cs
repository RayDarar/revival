using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GameManager : GenericManager<GameManager> {
  [HideInInspector]
  public int stage = 0;

  [HideInInspector]
  public int level = 0;

  [HideInInspector]
  public int wave = 0;

  [HideInInspector]
  public int selectedReward = 0; // 0 Coins, 1 Attack, 2 Defense, 3 Speed, 4 Magic, 6 Shopkeeper

  [HideInInspector]
  public PlayerData playerData;

  public void GameOver() {
    Debug.Log("Game Over");

    var lights = GameObject.FindObjectsOfType<Light2D>();
    foreach (var light in lights) {
      if (!light.gameObject.CompareTag("PlayerLight"))
        light.gameObject.SetActive(false);
    }
  }

  public void NewGame() {
    stage = 1;
    level = 1;
    selectedReward = 0;

    playerData = new PlayerData();
    playerData.baseAttackDamage = 5f;
    playerData.heavyAttackMultiplier = 1.5f;
    playerData.moveSpeed = 2f;
    playerData.maxHealth = 100f;
    playerData.health = playerData.maxHealth;
    playerData.coins = 0;
    LevelManager.Instance.StartRandomLevel();
  }

  public void SetupPlayer(PlayerController controller) {
    controller.baseAttackDamage = playerData.baseAttackDamage;
    controller.heavyAttackMultiplier = playerData.heavyAttackMultiplier;
    controller.moveSpeed = playerData.moveSpeed;
    controller.maxHealth = playerData.maxHealth;
    controller.health = playerData.health;
    controller.coins = playerData.coins;
  }

  public void UpdatePlayerData(PlayerController controller) {
    playerData.baseAttackDamage = controller.baseAttackDamage;
    playerData.heavyAttackMultiplier = controller.heavyAttackMultiplier;
    playerData.moveSpeed = controller.moveSpeed;
    playerData.maxHealth = controller.maxHealth;
    playerData.health = controller.health;
    playerData.coins = controller.coins;
  }
}