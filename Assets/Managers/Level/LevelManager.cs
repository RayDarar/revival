using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : GenericManager<LevelManager> {
  public LevelDefinition[] levels;

  public void StartRandomLevel() {
    int stage = GameManager.Instance.stage;

    var list = new List<LevelDefinition>();

    foreach (var level in levels)
      if (level.stage == stage) list.Add(level);

    int index = Random.Range(0, list.Count);

    StartCoroutine(LoadLevel(list[index]));
  }

  IEnumerator LoadLevel(LevelDefinition level) {
    var animator = GameObject.FindGameObjectWithTag("SceneTransition").GetComponent<Animator>();
    animator.enabled = true;
    animator.SetTrigger("Start");

    yield return new WaitForSeconds(3f);

    SceneManager.LoadScene(level.name);
  }
}
