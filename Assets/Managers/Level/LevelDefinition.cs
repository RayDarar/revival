using System;
using UnityEngine;

public enum LevelType {
  ARENA,
  BOSS,
  SHOPKEEPER
}

[Serializable]
public class LevelDefinition {
  public string name;

  public GameObject[] enemies;

  public int waves;

  public int enemyPerWave;

  public int stage;

  public LevelType levelType;
}