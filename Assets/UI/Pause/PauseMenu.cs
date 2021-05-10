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

    GameManager.Instance.isPaused = true;
  }

  public void UnpauseGame() {
    group.alpha = 0;
    group.blocksRaycasts = false;
    group.interactable = false;

    GameManager.Instance.isPaused = false;
  }

  public void ToMainMenu() {
    UnpauseGame();
    LevelManager.Instance.LoadMainMenu();
  }
}
