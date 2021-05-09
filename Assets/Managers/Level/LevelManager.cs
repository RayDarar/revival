using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : GenericManager<LevelManager> {
  public LevelDefinition[] levels;

  public void GenerateLevel(int index) {
    if (index > levels.Length || index < 0) throw new Exception("Level index out of range");

    LevelDefinition level = levels[index];

    StartCoroutine(LoadLevel(level));
  }

  IEnumerator LoadLevel(LevelDefinition level) {
    var animator = GameObject.FindGameObjectWithTag("SceneTransition").GetComponent<Animator>();
    animator.SetTrigger("Start");

    yield return new WaitForSeconds(3f);

    SceneManager.LoadScene(level.scene.name);
  }
}
