using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using SoundReplacer.Configuration;
using System;
using System.Reflection;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;

namespace SoundReplacer
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        private Harmony? _harmony;
        internal static Plugin Instance { get; private set; } = null!;
        internal static IPALogger Log { get; private set; } = null!;
        internal static PluginConfig CurrentConfig { get; private set; } = null!;

        [Init]
        public void Init(Config config, IPALogger logger)
        {
            Instance = this;
            Log = logger;
            PluginConfig.Instance = config.Generated<PluginConfig>();
            SoundLoader.GetSoundLists();
        }

        [OnStart]
        public void OnApplicationStart()
        {
            new GameObject("SoundReplacerController").AddComponent<SoundReplacerController>();
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            /**/
        }

        [OnEnable]
        public void OnEnable()
        {
            try {
                this._harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception e) {
                Log.Error(e);
            }
        }

        [OnDisable]
        public void OnDisable()
        {
            try {
                this._harmony?.UnpatchSelf();
            }
            catch (Exception e) {
                Log.Error(e);
            }
        }
    }
}
