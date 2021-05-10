using System;
using UnityEngine;

public enum RewardType {
  ARTIFACT,
  COINS,
  SHOPKEEPER
}

[Serializable]
public class RewardDefinition {
  public string name;

  public Sprite icon;

  public RewardType type;
}