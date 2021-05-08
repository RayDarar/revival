using UnityEngine;

public class GameManager : GenericManager<GameManager> {
  public GameEntity[] enemies;
  public GameObject player;

  public void GameOver() {
    Debug.Log("Game Over");
  }
}