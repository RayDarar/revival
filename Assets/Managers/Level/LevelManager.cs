using System;
using UnityEngine;

public class LevelManager : GenericManager<LevelManager> {
  public LevelDefinition[] levels;

  public void GenerateLevel(int index) {
    if (index > levels.Length || index < 0) throw new Exception("Level index out of range");

    LevelDefinition level = levels[index];

    
  }
}
