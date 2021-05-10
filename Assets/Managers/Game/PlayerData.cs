public class PlayerData {
  public float health { get; set; }
  public float maxHealth { get; set; }
  public int maxPotions { get; set; }
  public int potions { get; set; }

  public int coins { get; set; }

  public float moveSpeed { get; set; }

  public float baseAttackDamage { get; set; }
  public float heavyAttackMultiplier { get; set; }

  public ArtifactDefinition attackArtifact { get; set; }
  public ArtifactDefinition defenseArtifact { get; set; }
  public ArtifactDefinition speedArtifact { get; set; }
  public ArtifactDefinition magicArtifact { get; set; }
}