using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public float moveSpeed = 5f;
  public float rollingSpeed = 10f;
  public float rollingTime = 500f;
  public float rollingDelay = 800f;

  public new Rigidbody2D rigidbody;
  public Animator animator;

  private Vector2 movement;
  private bool isRolling = false;
  private DateTime rollingStart;
  private DateTime rollingEnd;
  private bool isRight = true;
  public void Update()
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

    animator.SetBool("IsRolling", isRolling);
    if (isRolling) return;
    // Registering direction, movement and speed
    movement.x = Input.GetAxisRaw("Horizontal");
    movement.y = Input.GetAxisRaw("Vertical");
    if (movement.x != 0)
      isRight = movement.x >= 0;
    animator.SetFloat("Horizontal", movement.x);
    animator.SetFloat("Speed", movement.sqrMagnitude);
    animator.SetBool("IsRight", isRight);
    animator.SetBool("IsRolling", false);
  }

  public void FixedUpdate()
  {
    if (isRolling)
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
      return;
    }
    rigidbody.MovePosition(rigidbody.position + movement * moveSpeed * Time.fixedDeltaTime);
  }
}
