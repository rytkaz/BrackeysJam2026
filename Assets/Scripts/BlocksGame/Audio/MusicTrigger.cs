using MessagePipe;
using UnityEngine;

namespace BlocksGame
{
    public class MusicTrigger : MonoBehaviour
    {
        [SerializeField] private AudioClip musicClip;

        private void Start()
        {
            GlobalMessagePipe.GetPublisher<MPlayAudio>().Publish(new MPlayAudio { Clip = musicClip, Type = AudioType.Music });
        }
    }
}
