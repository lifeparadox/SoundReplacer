using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace SoundReplacer.Patches
{
    public class ClickSoundPatch
    {
        private static List<AudioClip> _originalClickClips;

        private static AudioClip[] _lastClickClips;
        private static string _lastClickSelected;

        [HarmonyPatch(typeof(BasicUIAudioManager))]
        [HarmonyPatch("Start", MethodType.Normal)]
        public class BasicUIAudioManagerPatch
        {
            public static void Prefix(ref AudioClip[] ____clickSounds)
            {
                if (_originalClickClips == null) {
                    _originalClickClips = new List<AudioClip>();
                    _originalClickClips.AddRange(____clickSounds);
                }

                if (PluginConfig.Instance.ClickSound == "None") {
                    ____clickSounds = new AudioClip[] { SoundLoader.GetEmptyClip() };
                }
                else if (PluginConfig.Instance.ClickSound == "Default") {
                    ____clickSounds = _originalClickClips.ToArray();
                }
                else {
                    if (_lastClickSelected == PluginConfig.Instance.ClickSound) {
                        ____clickSounds = _lastClickClips;
                    }
                    else {
                        _lastClickSelected = PluginConfig.Instance.ClickSound;
                        _lastClickClips = new AudioClip[] { SoundLoader.LoadAudioClip(_lastClickSelected) };
                        ____clickSounds = _lastClickClips;
                    }
                }
            }
        }
    }
}
