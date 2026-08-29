using System;
using MessagePipe;
using UnityEngine;
using UnityEngine.Audio;

namespace BlocksGame
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        
        private IDisposable diposable;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            diposable = GlobalMessagePipe.GetSubscriber<MPlayAudio>().Subscribe(OnPlayAudio);
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
            diposable?.Dispose();
        }
    }
}
