using System;
using UnityEngine;

public class AudioManager : GenericManager<AudioManager> {
  public Sound[] sounds;

  public override void SetupAwake() {
    foreach (Sound sound in sounds) {
      AudioSource s = gameObject.AddComponent<AudioSource>();

      s.loop = sound.loop;
      s.clip = sound.clip;
      s.volume = sound.volume;
      s.pitch = sound.pitch;
      sound.source = s;
    }
  }

  public void Play(string name) {
    Sound sound = Array.Find(sounds, s => s.name == name);

    if (sound == null) {
      Debug.LogWarning("Sound " + name + " is not found");
      return;
    }
    if (!sound.source.isPlaying)
      sound.source.Play();
  }

  public void Stop(string name) {
    Sound sound = Array.Find(sounds, s => s.name == name);

    if (sound == null) {
      Debug.LogWarning("Sound " + name + " is not found");
      return;
    }
    if (sound.source.isPlaying)
      sound.source.Stop();
  }
}
