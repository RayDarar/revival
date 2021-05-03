using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  #region Movement
  public float moveSpeed = 2.5f;

  public new Rigidbody2D rigidbody;
  public Animator animator;

  private Vector2 movement;
  private bool isRight = true;
  #endregion

  #region Rolling
  private readonly float rollingSpeed = 5f;
  private readonly float rollingTime = 500f;
  private readonly float rollingDelay = 400f;

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
  public GameObject swing;

  private readonly float swingSpeed = 25f;
  private readonly float swingTime = 0.5f;

  private readonly float lightAttackDelay = 100f;
  private readonly float lightAttackTime = 300f;

  private readonly float heavyAttackDelay = 300f;
  private readonly float heavyAttackTime = 500f;

  private bool isAttacking = false;
  private int attackType = 0; // 1 light-1, 2 light-2, 3 heavy
  private bool isAttackRight = true;
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
      attackTime = lightAttackTime;
    }
    else if (heavyAttack)
    {
      attackType = 3;
      attackTime = heavyAttackTime;
    }


    if (lightAttack || heavyAttack)
    {
      Rect bounds = new Rect(0, 0, Screen.width / 2, Screen.height);
      isAttackRight = !bounds.Contains(Input.mousePosition);

      GameObject projectile = Instantiate(swing, transform.position, GetMouseAngle());
      Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
      Vector2 direction = projectile.transform.forward * swingSpeed;
      Debug.Log(direction);
      rb.AddForce(direction, ForceMode2D.Impulse);
      Destroy(projectile, swingTime);

      attackStart = DateTime.Now.AddMilliseconds(attackTime);
      isAttacking = true;
    }
  }


  private Quaternion GetMouseAngle()
  {
    // Code from: https://answers.unity.com/questions/395375/2d-mouse-angle-in-360-degrees.html
    Vector3 mouse_pos = Input.mousePosition;
    mouse_pos.z = 5.23f; //The distance between the camera and object
    Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
    mouse_pos.x = mouse_pos.x - object_pos.x;
    mouse_pos.y = mouse_pos.y - object_pos.y;
    float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
    return Quaternion.Euler(new Vector3(0, 0, angle));
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

    // Pre param conditions
    if (movement.x != 0)
      isRight = movement.x >= 0;
    if (attackType != 0)
      isRight = isAttackRight;

    // Setting animator parameters
    animator.SetFloat("Horizontal", movement.x);
    animator.SetFloat("Speed", movement.sqrMagnitude);
    animator.SetFloat("IsRight", isRight ? 1 : -1);
    animator.SetFloat("Attack", attackType);
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
