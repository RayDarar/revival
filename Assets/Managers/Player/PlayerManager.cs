using UnityEngine;

public class PlayerManager : GenericManager<PlayerManager> {
  private PlayerController player;

  public PlayerController GetPlayer() {
    if (player == null) {
      player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    return player;
  }
}
