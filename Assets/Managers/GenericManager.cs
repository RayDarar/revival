using UnityEngine;

public class GenericManager<T> : MonoBehaviour where T : Component {
  public static T Instance { get; private set; }

  public void Awake() {
    if (Instance == null) {
      Instance = this as T;
      DontDestroyOnLoad(Instance);
      SetupAwake();
    }
    else Destroy(gameObject);
  }

  public virtual void SetupAwake() { }
}