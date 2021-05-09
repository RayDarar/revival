using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour {
  public void Start() {
    StartCoroutine(PlayBackground());
  }

  IEnumerator PlayBackground() {
    yield return new WaitForSeconds(0.2f);

    AudioManager.Instance.Play("menu-background");
  }

  public void PlayGame() {
    GameManager.Instance.NewGame();
  }

  public void GoToOptions() {
    Debug.Log("Options!");
  }

  public void ExitGame() {
    Application.Quit(0);
  }

  public void OnDestroy() {
    AudioManager.Instance.Stop("menu-background");
  }
}
