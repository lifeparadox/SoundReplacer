using HarmonyLib;
using SoundReplacer.Configuration;
using UnityEngine;

namespace SoundReplacer.Patches
{
    public class MenuMusicPatch
    {
        private static AudioClip? _originalMenuMusicClip;

        private static AudioClip? _lastMenuMusicClip;
        private static string? _lastMusicSelected;

        [HarmonyPatch(typeof(SongPreviewPlayer))]
        [HarmonyPatch("Start", MethodType.Normal)]
        public class SongPreviewPlayerPatch
        {
            public static void Prefix(ref AudioClip ____defaultAudioClip)
            {
                if (_originalMenuMusicClip == null) {
                    _originalMenuMusicClip = ____defaultAudioClip;
                }

                if (PluginConfig.Instance.MenuMusic == "None") {
                    ____defaultAudioClip = SoundLoader.GetEmptyClip();
                }
                else if (PluginConfig.Instance.MenuMusic == "Default") {
                    ____defaultAudioClip = _originalMenuMusicClip;
                }
                else {
                    if (_lastMusicSelected == PluginConfig.Instance.MenuMusic && _lastMenuMusicClip != null) {
                        ____defaultAudioClip = _lastMenuMusicClip;
                    }
                    else {
                        _lastMusicSelected = PluginConfig.Instance.MenuMusic;
                        _lastMenuMusicClip = SoundLoader.LoadAudioClip(_lastMusicSelected);
                        ____defaultAudioClip = _lastMenuMusicClip;
                    }
                }
            }
        }
    }
}
