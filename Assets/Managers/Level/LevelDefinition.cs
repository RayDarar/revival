using System;
using UnityEngine;

[Serializable]
public class LevelDefinition {
  public string name;

  public GameObject[] enemies;

  public int waves;

  public int enemyPerWave;

  public int stage;
}