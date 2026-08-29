using System;
using System.Collections.Generic;
using MessagePipe;
using R3;
using UnityEngine;
using UnityEngine.Audio;

namespace BlocksGame
{
    public enum AudioType
    {
        Sfx,
        Music
    }

    public enum VolumeType
    {
        Sfx,
        Music,
        Master
    }
    
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        public static readonly Dictionary<VolumeType, string> VolumeParameterNames = new Dictionary<VolumeType, string>()
        {
            { VolumeType.Music, "MusicVolume" },
            { VolumeType.Sfx, "SFXVolume" },
            { VolumeType.Master, "MasterVolume" }
        };
        
        private IDisposable disposable;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            var d1= GlobalMessagePipe.GetSubscriber<MSetVolume>().Subscribe(SetVolume);
            var d2 = GlobalMessagePipe.GetSubscriber<MPlayAudio>().Subscribe(OnPlayAudio);
            disposable = Disposable.Combine(d1, d2);
            foreach (var value in VolumeParameterNames)
            {
                if (!PlayerPrefs.HasKey(value.Value))
                {
                    PlayerPrefs.SetFloat(value.Value, 1);
                    continue;
                }
                SetVolume(new MSetVolume() {Type = value.Key, Percentage = PlayerPrefs.GetFloat(value.Value)});
            }
        }

        private void OnPlayAudio(MPlayAudio args)
        {
            switch (args.Type)
            {
                case AudioType.Music:
                {
                    musicSource.clip = args.Clip;
                    musicSource.Play();
                    break;
                }
                case AudioType.Sfx:
                {
                    sfxSource.PlayOneShot(args.Clip);
                    break;
                }
            }
        }

        private void OnDestroy()
        {
            disposable?.Dispose();
        }

        private void SetVolume(MSetVolume args)
        {
            if (!VolumeParameterNames.TryGetValue(args.Type, out var paramName))
            {
                return;
            }
            
            mixer.SetFloat(paramName, ConvertToDecibel(args.Percentage));
            PlayerPrefs.SetFloat(paramName, args.Percentage);
        }
        
        private float ConvertToDecibel(float percentage)
        {
            return Mathf.Log10(Mathf.Max(percentage, 0.0001f))*20f;
        }
    }
}
