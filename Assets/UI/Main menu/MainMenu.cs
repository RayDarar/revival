using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour {
  public void Start() {
    StartCoroutine(PlayBackground());
  }

  IEnumerator PlayBackground() {
    yield return new WaitForSeconds(0.5f);

    AudioManager.Instance.Play("menu-background");
  }
}
