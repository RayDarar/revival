using UnityEngine;

public class ArtifactWindow : MonoBehaviour {
  public void Select1() {
    LevelManager.Instance.SelectArtifact(0);
  }

  public void Select2() {
    LevelManager.Instance.SelectArtifact(1);
  }

  public void Select3() {
    LevelManager.Instance.SelectArtifact(2);
  }
}