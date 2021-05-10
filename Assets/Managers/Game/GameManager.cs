using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class GameManager : GenericManager<GameManager> {
  [HideInInspector]
  public bool isPaused = false;

  [HideInInspector]
  public int stage = 0;

  [HideInInspector]
  public int level = 0;

  [HideInInspector]
  public int wave = 0;

  [HideInInspector]
  public int selectedReward = 0; // 0 Coins, 1 Attack, 2 Defense, 3 Speed, 4 Magic, 6 Shopkeeper

  [HideInInspector]
  public PlayerData playerData = new PlayerData();

  public ArtifactDefinition[] artifacts;

  public void GameOver() {
    var lights = GameObject.FindObjectsOfType<Light2D>();
    foreach (var light in lights) {
      if (!light.gameObject.CompareTag("PlayerLight"))
        light.gameObject.SetActive(false);
    }
  }

  public void NewGame() {
    stage = 1;
    level = 1;
    selectedReward = 3;

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

    // Setting artifacts
    if (playerData.attackArtifact != null) {
      SetupArtifact(playerData.attackArtifact, 0, controller);
    }
    if (playerData.defenseArtifact != null) {
      SetupArtifact(playerData.defenseArtifact, 1, controller);
    }
    if (playerData.speedArtifact != null) {
      SetupArtifact(playerData.speedArtifact, 2, controller);
    }
    if (playerData.magicArtifact != null) {
      SetupArtifact(playerData.magicArtifact, 3, controller);
    }
  }

  private void SetupArtifact(ArtifactDefinition artifact, int index, PlayerController player) {
    var slot = GameObject.FindGameObjectsWithTag("PlayerArtifact").FirstOrDefault(a => a.GetComponent<IndexedItem>().index == index);

    var image = slot.GetComponentsInChildren<Image>().FirstOrDefault(c => c.gameObject.CompareTag("PlayerArtifactImage"));
    image.color = new Color(255, 255, 255, 1);
    image.sprite = artifact.sprite;

    GameObject instance = Instantiate(artifact.script, new Vector3(0, 0, 0), player.transform.rotation);
    instance.transform.parent = player.transform;
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