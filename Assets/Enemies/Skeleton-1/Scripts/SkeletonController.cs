﻿using System;

public class SkeletonController : GenericEnemyController {
  #region Attacking
  private bool isAttacking = false;
  private float attackTime = 750f;
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

    animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
    animator.SetBool("IsAttacking", isAttacking);
  }

  public override void SetupEnemy(GenericEnemyBuilder builder) {
    builder.SetLookRadius(6f).SetAttackRadius(2f).SetHealth(100f);
  }
}
