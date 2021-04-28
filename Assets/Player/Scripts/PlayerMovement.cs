using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
  #region Movement
  public float moveSpeed = 5f;

  public new Rigidbody2D rigidbody;
  public Animator animator;

  private Vector2 movement;
  private bool isRight = true;
  #endregion

  #region Rolling
  public float rollingSpeed = 10f;
  public float rollingTime = 500f;
  public float rollingDelay = 800f;

  private bool isRolling = false;
  private DateTime rollingStart;
  private DateTime rollingEnd;

  private void HandleRolling()
  {
    bool spacePressed = Input.GetKeyDown(KeyCode.Space);
    bool rollingAvailable = (DateTime.Now - rollingEnd).TotalMilliseconds > rollingDelay;
    if (!isRolling && spacePressed && rollingAvailable)
    {
      isRolling = true;
      if (movement.x == 0)
        movement.x = isRight ? 1 : -1;
      rollingStart = DateTime.Now.AddMilliseconds(rollingTime);
    }
  }
  private void PerformRolling()
  {
    double delta = (DateTime.Now - rollingStart).TotalMilliseconds;
    if (delta > 0)
    {
      isRolling = false;
      rollingEnd = DateTime.Now;
      return;
    }
    float speed = rollingSpeed * Time.fixedDeltaTime;
    if (Math.Abs(movement.x) == Math.Abs(movement.y))
      speed /= 1.33f;

    rigidbody.MovePosition(rigidbody.position + movement * speed);
  }
  #endregion

  #region Attacking
  public float lightAttackDelay = 100f;
  public float heavyAttackDelay = 300f;

  private bool isAttacking = false;
  private int attackType = 0; // 1 light-1, 2 light-2, 3 heavy
  private DateTime attackStart;
  private DateTime attackEnd;

  private void HandleAttacking()
  {
    if (isAttacking)
    {
      double deltaStart = (DateTime.Now - attackStart).TotalMilliseconds;
      if (deltaStart > 0)
      {
        attackType = 0;
        isAttacking = false;
        attackEnd = DateTime.Now;
      }
      return;
    }
    double delta = (DateTime.Now - attackEnd).TotalMilliseconds;

    bool lightAttack = Input.GetMouseButtonDown(0) && delta > lightAttackDelay;
    bool heavyAttack = Input.GetMouseButtonDown(1) && delta > heavyAttackDelay;

    float attackTime = 0f;
    if (lightAttack)
    {
      attackType = 1;
      attackTime = 100f;
    }
    else if (heavyAttack)
    {
      attackType = 3;
      attackTime = 250f;
    }


    if (lightAttack || heavyAttack)
    {
      attackStart = DateTime.Now.AddMilliseconds(attackTime);
      isAttacking = true;
    }
  }
  #endregion

  public void Update()
  {
    HandleRolling();

    animator.SetBool("IsRolling", isRolling);
    if (isRolling) return;
    HandleAttacking();
    // Registering direction, movement and speed
    movement.x = Input.GetAxisRaw("Horizontal");
    movement.y = Input.GetAxisRaw("Vertical");
    if (movement.x != 0)
      isRight = movement.x >= 0;
    animator.SetFloat("Horizontal", movement.x);
    animator.SetFloat("Speed", movement.sqrMagnitude);
    animator.SetBool("IsRight", isRight);
    animator.SetInteger("Attack", attackType);
    animator.SetBool("IsRolling", false);
  }

  public void FixedUpdate()
  {
    if (isRolling)
    {
      PerformRolling();
      return;
    }
    float speed = moveSpeed * Time.fixedDeltaTime;
    if (isAttacking)
      speed /= 1.5f;
    rigidbody.MovePosition(rigidbody.position + movement * speed);
  }
}
