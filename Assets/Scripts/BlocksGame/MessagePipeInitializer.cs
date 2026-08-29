using System;
using MessagePipe;
using R3;
using UnityEngine;

namespace BlocksGame
{
    public static class MessagePipeInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            GlobalMessagePipe.SetProvider(new BuiltinContainerBuilder()
                .AddMessagePipe()
                .AddMessageBroker<MGameplayTick>()
                .AddMessageBroker<MGameplayPieceFinished>()
                .AddMessageBroker<MPlayAudio>()
                .BuildServiceProvider());
        }
    }
}
