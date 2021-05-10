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
    }
    else ShowArtifacts();
  }

  public void ShowArtifacts() {

  }

  public void ShowNextRewards() {
    RewardDefinition[] options = rewards.OrderBy(r => System.Guid.NewGuid()).Take(3).ToArray();
    GameObject[] slots = GameObject.FindGameObjectsWithTag("RewardSlot");

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
