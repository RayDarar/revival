using UnityEngine;

public class PlayerData {
  public float health { get; set; }
  public float maxHealth { get; set; }
  public int maxPotions { get; set; }
  public int potions { get; set; }

  public int coins { get; set; }

  public float moveSpeed { get; set; }

  public float baseAttackDamage { get; set; }
  public float heavyAttackMultiplier { get; set; }

  public GameObject attackArtifact { get; set; }
  public GameObject defenseArtifact { get; set; }
  public GameObject speedArtifact { get; set; }
  public GameObject magicArtifact { get; set; }
}