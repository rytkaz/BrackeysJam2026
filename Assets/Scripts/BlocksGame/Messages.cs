using UnityEngine;

namespace BlocksGame
{
    public struct MGameplayTick
    {
        
    }

    public struct MGameplayPieceFinished
    {
        
    }

    public struct MPlayAudio
    {
        public AudioType Type;
        public AudioClip Clip;
    }

    public enum AudioType
    {
        Sfx,
        Music
    }
}
