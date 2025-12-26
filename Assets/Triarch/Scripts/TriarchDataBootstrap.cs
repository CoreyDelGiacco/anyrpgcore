using System;
using UnityEngine;

namespace Triarch
{
    public sealed class TriarchDataBootstrap : MonoBehaviour
    {
        public static TriarchDataRepository Data { get; private set; }

        [Tooltip("Load Triarch data on Awake.")]
        [SerializeField]
        private bool loadOnAwake = true;

        private void Awake()
        {
            if (!loadOnAwake || Data != null)
            {
                return;
            }

            try
            {
                Data = TriarchDataLoader.LoadFromStreamingAssets();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Triarch data bootstrap failed: {ex.Message}\n{ex}");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit(1);
#endif
                throw;
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}
