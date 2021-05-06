using UnityEngine;

public class RotateEntity {
  public static void rotate(GameObject target, bool isRight) {
    target.transform.localScale = new Vector3(isRight ? 1 : -1, 1, 1);
  }
}