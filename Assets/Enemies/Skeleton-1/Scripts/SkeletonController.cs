using UnityEngine;
using UnityEngine.AI;

public class SkeletonController : GenericEnemyController {
  public override void Update() {
    base.Update();
    animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
  }

  public override void SetupEnemy(GenericEnemyBuilder builder) {
    builder.SetLookRadius(6f).SetAttackRadius(1.5f).SetHealth(100f);
  }
}
