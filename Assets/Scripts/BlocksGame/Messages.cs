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

    public struct MSetVolume
    {
        public VolumeType Type;
        public float Percentage;
    }

    public struct MToggleGameUI
    {
        public UI.GameUIScreenType ScreenType;
    }

    public struct MGamePauseStateChanged
    {
        public bool IsPaused;
    }
}
