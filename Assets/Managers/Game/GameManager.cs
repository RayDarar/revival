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
    LevelManager.Instance.StartRandomLevel();
  }
}