using System;
using UnityEngine;

public class SkeletonController : GenericEnemyController {
  #region Attacking
  private bool isAttacking = false;
  private bool isPlayerHit = false;
  private float attackTime = 750f;
  private float attackHitTime = -200f;
  private float attackDelay = 1000f;
  private DateTime attackEnd;

  private float baseAttackDamage = 5f;

  private void PerformAttack() {
    double attackDelayDelta = (DateTime.Now - attackEnd).TotalMilliseconds;
    if (!isAttacking && diff.sqrMagnitude < attackRadius * attackRadius && attackDelayDelta >= attackDelay) {
      isAttacking = true;
      attackEnd = DateTime.Now.AddMilliseconds(attackTime);
    }

    double attackDelta = (DateTime.Now - attackEnd).TotalMilliseconds;
    if (isAttacking && attackDelta > 0) {
      isAttacking = false;
      isPlayerHit = false;
    }
    else if (isAttacking && !isPlayerHit && attackDelta > attackHitTime) {
      isPlayerHit = true;
      UpdatePlayerDifference();

      if (diff.sqrMagnitude < attackRadius * attackRadius) {
        player.TakeHit(baseAttackDamage);
      }
    }
  }
  #endregion

  public override void Update() {
    base.Update();

    PerformAttack();

    animator.SetBool("IsAttacking", isAttacking);
  }

  public override void SetupEnemy(GenericEnemyBuilder builder) {
    builder.SetLookRadius(6f).SetAttackRadius(1.8f).SetHealth(100f);
  }
}
