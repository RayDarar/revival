using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public Sound[] sounds;

  public static AudioManager instance;

  public void Awake()
  {
    if (instance == null)
      instance = this;
    else
    {
      Destroy(gameObject);
      return;
    }

    DontDestroyOnLoad(gameObject);

    foreach (Sound sound in sounds)
    {
      AudioSource s = gameObject.AddComponent<AudioSource>();

      s.loop = sound.loop;
      s.clip = sound.clip;
      s.volume = sound.volume;
      s.pitch = sound.pitch;
      sound.source = s;
    }
  }

  public void Play(string name)
  {
    Sound sound = Array.Find(sounds, s => s.name == name);
    if (sound != null && !sound.source.isPlaying)
      sound.source.Play();
  }

  public void Stop(string name)
  {
    Sound sound = Array.Find(sounds, s => s.name == name);
    if (sound != null && sound.source.isPlaying)
      sound.source.Stop();
  }
}
