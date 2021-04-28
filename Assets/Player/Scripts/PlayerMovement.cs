using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public float moveSpeed = 5f;

  public new Rigidbody2D rigidbody;

  private Vector2 movement;
  public void Update()
  {
    movement.x = Input.GetAxisRaw("Horizontal");
    movement.y = Input.GetAxisRaw("Vertical");
  }

  public void FixedUpdate()
  {
    rigidbody.MovePosition(rigidbody.position + movement * moveSpeed * Time.fixedDeltaTime);
  }
}
