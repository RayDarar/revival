using System;
using UnityEngine;
using UnityEngine.AI;

public class GenericEnemyBuilder {
  private readonly GenericEnemyController controller;
  public GenericEnemyBuilder(GenericEnemyController controller) {
    this.controller = controller;
  }

  public GenericEnemyBuilder SetLookRadius(float value) {
    controller.lookRadius = value;
    return this;
  }

  public GenericEnemyBuilder SetHealth(float value) {
    controller.health = value;
    return this;
  }

  public GenericEnemyBuilder SetAttackRadius(float value) {
    controller.attackRadius = value;
    return this;
  }
}

public abstract class GenericEnemyController : MonoBehaviour {
  [HideInInspector]
  public NavMeshAgent agent;

  [HideInInspector]
  public Animator animator;

  [HideInInspector]
  public PlayerController player;

  [HideInInspector]
  public float lookRadius;

  [HideInInspector]
  public float attackRadius;

  [HideInInspector]
  public Vector2 diff;

  [HideInInspector]
  public float health;

  [HideInInspector]
  public bool isRight;

  public abstract void SetupEnemy(GenericEnemyBuilder builder);
  public virtual void Awake() {
    SetupEnemy(new GenericEnemyBuilder(this));
  }

  public virtual void Start() {
    agent = GetComponent<NavMeshAgent>();
    agent.updateRotation = false;
    agent.updateUpAxis = false;

    animator = GetComponent<Animator>();

    player = PlayerManager.Instance.player;
  }

  public virtual void Update() {
    UpdatePlayerDifference();
    MoveTowardsPlayerRadius();
    UpdateIsRight();

    RotateEntity.rotate(gameObject, isRight);
  }

  public void OnDrawGizmosSelected() {
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, lookRadius);

    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, attackRadius);
  }

  public void MoveTowardsPlayerRadius() {
    if (diff.sqrMagnitude < lookRadius * lookRadius) {
      agent.SetDestination(player.transform.position);
    }
  }

  public void UpdateIsRight() {
    if (agent.velocity.x != 0 && Math.Abs(agent.velocity.x) > 0.5f)
      isRight = agent.velocity.x >= 0;
  }

  public void UpdatePlayerDifference() {
    diff = transform.position - player.transform.position;
  }

  public void TakeHit(float damage) {
    health -= damage;
    Debug.Log(this.name + "Hit! Health: " + health);
    if (health <= 0)
      Debug.Log(this.name + "Is Dead!");
  }
}