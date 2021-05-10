using UnityEngine;

public class PauseMenu : MonoBehaviour {
  private CanvasGroup group;

  public void Start() {
    group = GetComponent<CanvasGroup>();
  }

  public void Update() {
    if (!GameManager.Instance.isPaused && Input.GetKeyDown(KeyCode.Escape)) {
      PauseGame();
    }
  }

  public void PauseGame() {
    group.alpha = 1;
    group.blocksRaycasts = true;
    group.interactable = true;
    Time.timeScale = 0f;

    GameManager.Instance.isPaused = true;
    PlayerManager.Instance.GetPlayer().enabled = false;
  }

  public void UnpauseGame() {
    group.alpha = 0;
    group.blocksRaycasts = false;
    group.interactable = false;
    Time.timeScale = 1f;

    GameManager.Instance.isPaused = false;
    PlayerManager.Instance.GetPlayer().enabled = true;
  }

  public void ToMainMenu() {
    UnpauseGame();
    LevelManager.Instance.LoadMainMenu();
  }

  public void KillWave() {
    LevelManager.Instance.KillWave();
  }
}
