using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : GenericManager<LevelManager> {
  public LevelDefinition[] levels;
  public RewardDefinition[] rewards;

  private int enemiesCount = 0;
  private LevelDefinition currentLevel;
  private TextMeshProUGUI stageText;
  private TextMeshProUGUI levelText;

  public void StartRandomLevel() {
    int stage = GameManager.Instance.stage;

    var list = new List<LevelDefinition>();

    foreach (var level in levels)
      if (level.stage == stage) list.Add(level);

    int index = Random.Range(0, list.Count);
    currentLevel = list[index];

    StartCoroutine(LoadLevel());
  }

  public IEnumerator LoadLevel(int index = -1) {
    var animator = GameObject.FindGameObjectWithTag("SceneTransition").GetComponent<Animator>();
    animator.enabled = true;
    animator.SetTrigger("Start");
    yield return new WaitForSeconds(1f);

    if (index < 0) {
      if (currentLevel.levelType == LevelType.ARENA)
        AudioManager.Instance.Play("arena-background");
      else if (currentLevel.levelType == LevelType.BOSS)
        AudioManager.Instance.Play("boss-background");
      else if (currentLevel.levelType == LevelType.SHOPKEEPER)
        AudioManager.Instance.Play("shopkeeper-background");
    }

    if (index >= 0)
      SceneManager.LoadScene(index);
    else {
      SceneManager.LoadScene(currentLevel.name);
    }

    yield return new WaitForSeconds(0.5f);

    if (index < 0) {
      yield return PopulateLevel();

      stageText = GameObject.FindGameObjectWithTag("GameStage").GetComponent<TextMeshProUGUI>();
      levelText = GameObject.FindGameObjectWithTag("GameLevel").GetComponent<TextMeshProUGUI>();

      stageText.text = $"Stage: {GameManager.Instance.stage}";
      levelText.text = $"Level: {GameManager.Instance.wave}";
    }
  }

  public IEnumerator PopulateLevel() {
    yield return new WaitForSeconds(0.5f);
    GameObject[] enemies = currentLevel.enemies.OrderBy(e => System.Guid.NewGuid()).Take(currentLevel.enemyPerWave).ToArray();

    foreach (var enemy in enemies) {
      StartCoroutine(SpawnEnemy(enemy));
      yield return new WaitForSeconds(0.3f);
    }

    enemiesCount = enemies.Length;
    GameManager.Instance.wave++;
  }

  public IEnumerator SpawnEnemy(GameObject enemy) {
    var obj = Instantiate(enemy, GetRandomLocation(), enemy.transform.rotation);
    var controller = obj.GetComponent<GenericEnemyController>();
    var light = obj.GetComponentInChildren<Light2D>();
    controller.enabled = false;
    yield return new WaitForSeconds(1.5f);
    controller.enabled = true;
    light.enabled = false;
    obj.transform.rotation = enemy.transform.rotation;
  }

  public Vector3 GetRandomLocation() {
    NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

    // Pick the first indice of a random triangle in the nav mesh
    int t = Random.Range(0, navMeshData.indices.Length - 3);

    // Select a random point on it
    Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
    Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

    return point;
  }

  public void GetReward() {
    int reward = GameManager.Instance.selectedReward;

    if (reward == 0) {
      PlayerManager.Instance.GetPlayer().AddCoins(GameManager.Instance.stage * 10 * GameManager.Instance.level);
      ShowNextRewards();
    }
    else ShowArtifacts();
  }

  ArtifactDefinition[] artifactOptions;
  public void ShowArtifacts() {
    ArtifactType type = ArtifactType.ATTACK;
    switch (GameManager.Instance.selectedReward) {
      case 1: type = ArtifactType.ATTACK; break;
      case 2: type = ArtifactType.DEFENSE; break;
      case 3: type = ArtifactType.SPEED; break;
      case 4: type = ArtifactType.MAGIC; break;
    }

    ArtifactDefinition[] options =
      GameManager.Instance.artifacts
      .Where(a => a.condition <= GameManager.Instance.stage * 10 + GameManager.Instance.level && a.type == type)
      .OrderBy(a => System.Guid.NewGuid())
      .Take(3)
      .ToArray();
    GameObject[] slots = GameObject.FindGameObjectsWithTag("ArtifactSlot").OrderBy(s => s.GetComponent<IndexedItem>().index).ToArray();

    for (int i = 0; i < 3; i++) {
      var slotIcon = slots[i].GetComponentInChildren<Image>();
      var slotText = slots[i].GetComponentInChildren<TextMeshProUGUI>();

      if (i > options.Length - 1) {
        slotText.text = "";
        slotIcon.sprite = null;
        slotIcon.color = new Color(255, 255, 255, 0);
        continue;
      }
      var option = options[i];

      slotText.text = option.name;
      slotIcon.sprite = option.sprite;
      slotIcon.color = new Color(255, 255, 255, 1);
    }

    var group = GameObject.FindGameObjectWithTag("ArtifactContainer").GetComponent<CanvasGroup>();
    group.alpha = 1;
    group.interactable = true;
    group.blocksRaycasts = true;

    artifactOptions = options;
  }

  public void SelectArtifact(int index) {
    if (index > artifactOptions.Length - 1) return;
    ArtifactDefinition artifact = artifactOptions[index];

    switch (GameManager.Instance.selectedReward) {
      case 1: GameManager.Instance.playerData.attackArtifact = artifact.script; break;
      case 2: GameManager.Instance.playerData.defenseArtifact = artifact.script; break;
      case 3: GameManager.Instance.playerData.speedArtifact = artifact.script; break;
      case 4: GameManager.Instance.playerData.magicArtifact = artifact.script; break;
    }

    var group = GameObject.FindGameObjectWithTag("ArtifactContainer").GetComponent<CanvasGroup>();
    group.alpha = 0;
    group.interactable = false;
    group.blocksRaycasts = false;

    ShowNextRewards();
  }

  public void GoNext() {
    GameManager.Instance.level++;
    GameManager.Instance.wave = 0;

    if (GameManager.Instance.level == 11) {
      GameManager.Instance.stage++;
      GameManager.Instance.level = 1;
    }

    StartRandomLevel();
  }

  private RewardDefinition[] rewardOptions;
  public void ShowNextRewards() {
    RewardDefinition[] options = rewards.OrderBy(r => System.Guid.NewGuid()).Take(3).ToArray();
    GameObject[] slots = GameObject.FindGameObjectsWithTag("RewardSlot").OrderBy(s => s.GetComponent<IndexedItem>().index).ToArray();

    for (int i = 0; i < 3; i++) {
      var option = options[i];
      var slotIcon = slots[i].GetComponentInChildren<Image>();
      var slotText = slots[i].GetComponentInChildren<TextMeshProUGUI>();

      slotText.text = option.name;

      if (option.type == RewardType.ARTIFACT) {
        slotIcon.sprite = option.icon;
        slotIcon.color = new Color(255, 255, 255, 1);
      }
      else {
        slotIcon.sprite = null;
        slotIcon.color = new Color(255, 255, 255, 0);
      }
    }

    var group = GameObject.FindGameObjectWithTag("RewardContainer").GetComponent<CanvasGroup>();
    group.alpha = 1;
    group.interactable = true;
    group.blocksRaycasts = true;

    rewardOptions = options;
  }

  public void SelectNextReward(int index) {
    RewardDefinition reward = rewardOptions[index];

    switch (reward.type) {
      case RewardType.ARTIFACT: {
          switch (reward.name) {
            case "Attack Artifact": GameManager.Instance.selectedReward = 1; break;
            case "Defense Artifact": GameManager.Instance.selectedReward = 2; break;
            case "Speed Artifact": GameManager.Instance.selectedReward = 3; break;
            case "Magic Artifact": GameManager.Instance.selectedReward = 4; break;
          }
          break;
        }
      case RewardType.COINS: GameManager.Instance.selectedReward = 0; break;
      case RewardType.SHOPKEEPER: GameManager.Instance.selectedReward = 6; break;
    }

    var group = GameObject.FindGameObjectWithTag("RewardContainer").GetComponent<CanvasGroup>();
    group.alpha = 0;
    group.interactable = false;
    group.blocksRaycasts = false;

    GoNext();
  }

  public void DecreaseEnemyCount() {
    enemiesCount--;

    if (enemiesCount == 0 && GameManager.Instance.wave != currentLevel.waves) {
      StartCoroutine(PopulateLevel());
      return;
    }
    else if (enemiesCount == 0 && GameManager.Instance.wave == currentLevel.waves) {
      GetReward();
    }
  }

  public void LoadMainMenu() {
    StartCoroutine(LoadLevel(0));
  }

  public void KillWave() {
    var enemies = GameObject.FindGameObjectsWithTag("Enemies");

    foreach (var enemy in enemies) {
      enemy.GetComponent<GenericEnemyController>().TakeHit(100000f);
    }
  }
}
