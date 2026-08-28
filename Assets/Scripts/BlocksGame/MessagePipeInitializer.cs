using System;
using MessagePipe;
using R3;
using UnityEngine;

namespace BlocksGame
{
    public static class MessagePipeInitializer
    {
        [RuntimeInitializeOnLoadMethod]
        public static void Initialize()
        {
            GlobalMessagePipe.SetProvider(new BuiltinContainerBuilder()
                .AddMessagePipe()
                .AddMessageBroker<MGameplayTick>()
                .AddMessageBroker<MGameplayPieceFinished>()
                .BuildServiceProvider());
        }
    }
}
