using HarmonyLib;
using SoundReplacer.Configuration;
using System;
using UnityEngine;

namespace SoundReplacer.Patches
{
    public class LevelEndPatch
    {
        private static AudioClip? _lastSuccessClip;
        private static string? _lastSuccessSelected;

        private static AudioClip? _lastFailClip;
        private static string? _lastFailSelected;

        [HarmonyPatch(typeof(ResultsViewController))]
        [HarmonyPatch("DidActivate", MethodType.Normal)]
        public class DidActivatePatch
        {
            public static void Prefix(bool addedToHierarchy, ref SongPreviewPlayer ____songPreviewPlayer, ref LevelCompletionResults ____levelCompletionResults)
            {
                if (!addedToHierarchy) {
                    return;
                }

                if (____levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared) {
                    if (!(PluginConfig.Instance.SuccessSound == "Default" ||
                          PluginConfig.Instance.SuccessSound == "None")) {
                        AudioClip desiredSuccessClip;

                        if (_lastSuccessSelected == PluginConfig.Instance.SuccessSound && _lastSuccessClip != null) {
                            desiredSuccessClip = _lastSuccessClip;
                        }
                        else {
                            _lastSuccessSelected = PluginConfig.Instance.SuccessSound;
                            _lastSuccessClip = SoundLoader.LoadAudioClip(_lastSuccessSelected);
                            desiredSuccessClip = _lastSuccessClip;
                        }

                        ____songPreviewPlayer.CrossfadeTo(desiredSuccessClip, 0f, 0f, Math.Min(desiredSuccessClip.length, 20.0f), null);
                    }
                }

                if (____levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Failed) {
                    if (!(PluginConfig.Instance.FailSound == "Default" ||
                          PluginConfig.Instance.FailSound == "None")) {
                        AudioClip desiredFailClip;

                        if (_lastFailSelected == PluginConfig.Instance.FailSound && _lastFailClip != null) {
                            desiredFailClip = _lastFailClip;
                        }
                        else {
                            _lastFailSelected = PluginConfig.Instance.FailSound;
                            _lastFailClip = SoundLoader.LoadAudioClip(_lastFailSelected);
                            desiredFailClip = _lastFailClip;
                        }

                        ____songPreviewPlayer.CrossfadeTo(desiredFailClip, 0f, 0f, Math.Min(desiredFailClip.length, 20.0f), null);
                    }
                }
            }
        }
    }
}
