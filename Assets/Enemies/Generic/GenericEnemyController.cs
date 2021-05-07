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
  public float health;

  [HideInInspector]
  public bool isRight;

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

  public void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, lookRadius);
  }

  public abstract void SetupEnemy(GenericEnemyBuilder builder);

  public void MoveTowardsPlayerRadius() {
    Vector2 diff = transform.position - player.transform.position;

    if (diff.sqrMagnitude < lookRadius * lookRadius) {
      agent.SetDestination(player.transform.position);
    }
  }

  public void UpdateIsRight() {
    if (agent.velocity.x != 0 && Math.Abs(agent.velocity.x) > 0.5f)
      isRight = agent.velocity.x >= 0;
  }
}