using UnityEngine;

public class NextRewardWindow : MonoBehaviour {
  public void Select1() {
    LevelManager.Instance.SelectNextReward(0);
  }

  public void Select2() {
    LevelManager.Instance.SelectNextReward(1);
  }

  public void Select3() {
    LevelManager.Instance.SelectNextReward(2);
  }
}
