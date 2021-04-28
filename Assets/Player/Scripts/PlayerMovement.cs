using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public float moveSpeed = 5f;

  public new Rigidbody2D rigidbody;
  public Animator animator;

  private Vector2 movement;
  public void Update()
  {
    movement.x = Input.GetAxisRaw("Horizontal");
    movement.y = Input.GetAxisRaw("Vertical");

    animator.SetFloat("Horizontal", movement.x);
    animator.SetFloat("Speed", movement.sqrMagnitude);
    if (movement.x != 0) {
      animator.SetBool("IsRight", movement.x >= 0);
    }
  }

  public void FixedUpdate()
  {
    rigidbody.MovePosition(rigidbody.position + movement * moveSpeed * Time.fixedDeltaTime);
  }
}
