using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class LevelDefinition {
  public string name;

  public GameObject[] enemies;

  public SceneAsset scene;

  public int waves;
}