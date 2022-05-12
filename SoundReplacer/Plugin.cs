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
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }
        private Harmony _harmony;
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
                this._harmony.UnpatchSelf();
            }
            catch (Exception e) {
                Log.Error(e);
            }
        }
    }
}
