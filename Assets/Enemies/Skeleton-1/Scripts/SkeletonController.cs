using UnityEngine;
using UnityEngine.AI;

public class SkeletonController : GenericEnemyController {
  public void Update() {
    MoveTowardsPlayerRadius();
  }

  public override void SetupEnemy(GenericEnemyBuilder builder) {
    builder.SetLookRadius(6f).SetHealth(100f);
  }
}
