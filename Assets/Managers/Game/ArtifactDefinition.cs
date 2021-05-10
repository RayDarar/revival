using System;
using UnityEngine;

public enum ArtifactType {
  ATTACK,
  DEFENSE,
  SPEED,
  MAGIC
}

[Serializable]
public class ArtifactDefinition {
  public string name;

  public string description;

  public ArtifactType type;

  public Sprite sprite;

  public int condition;

  public GameObject script;
}