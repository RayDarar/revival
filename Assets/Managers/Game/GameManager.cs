using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GameManager : GenericManager<GameManager> {
  public GameEntity[] enemies;
  public GameObject player;

  public void GameOver() {
    Debug.Log("Game Over");

    var lights = GameObject.FindObjectsOfType<Light2D>();
    foreach (var light in lights) {
      if (!light.gameObject.CompareTag("PlayerLight"))
        light.gameObject.SetActive(false);
    }
  }

  public void NewGame() {
    int index = Random.Range(0, LevelManager.Instance.levels.Length);

    LevelManager.Instance.GenerateLevel(index);
  }
}