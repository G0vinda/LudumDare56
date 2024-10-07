using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundID {
   Jump = 0,
   HitGround = 1,
   StabbedBySpike = 2,
   Win = 3,
   Teleport = 4,
   Grab = 5,
   Throw = 6,
   Dash = 7,
}

public class AudioPlayer : MonoBehaviour
{
   public static AudioPlayer instance;
   
   [SerializeField] AudioSource audioSource;
   [SerializeField] AudioSource longClipsAudioSource;
   
   [SerializeField] AudioClip[] audioClips;

   private void Awake()
   {
      if(instance == null)
      {
         instance = this;
      }
      else
      {
         Destroy(this);
      }
   }

   public void PlayAudio(SoundID soundID)
   {
      if(soundID == SoundID.Teleport || soundID == SoundID.Win)
         PlayLongAudioClip(soundID);
      
      Debug.Log("Playing audio clip " + soundID);
      audioSource.clip = audioClips[(int)soundID];
      audioSource.Play();
   }
   
   private void PlayLongAudioClip(SoundID soundID)
   {
      longClipsAudioSource.clip = audioClips[(int)soundID];
      longClipsAudioSource.Play();
   }
}
