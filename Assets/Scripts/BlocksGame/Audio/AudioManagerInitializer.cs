#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace BlocksGame
{
    public static class AudioManagerInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {   
            var audioManagerPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/AudioManager.prefab", typeof(GameObject));
            PrefabUtility.InstantiatePrefab(audioManagerPrefab);
        }
    }
}
#endif
