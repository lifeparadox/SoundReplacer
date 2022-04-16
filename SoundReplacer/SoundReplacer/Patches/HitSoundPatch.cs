using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace SoundReplacer.Patches
{
    public class HitSoundPatch
    {
        private static List<AudioClip> _originalBadSounds;
        private static List<AudioClip> _originalGoodLongSounds;
        private static List<AudioClip> _originalGoodShortSounds;

        private static readonly AudioClip[] _lastBadAudioClips = new AudioClip[s_badCutArrayLength];
        private static string _lastBadSelected;

        private static readonly AudioClip[] _lastGoodAudioClips = new AudioClip[s_goodCutArrayLength];
        private static string _lastGoodSelected;

        private const int s_badCutArrayLength = 10;
        private const int s_goodCutArrayLength = 10;

        [HarmonyPatch(typeof(NoteCutSoundEffect))]
        [HarmonyPatch("Awake", MethodType.Normal)]
        public class BadCutSoundPatch
        {
            public static void Prefix(ref AudioClip[] ____badCutSoundEffectAudioClips)
            {
                if (_originalBadSounds == null)
                {
                    _originalBadSounds = new List<AudioClip>();
                    _originalBadSounds.AddRange(____badCutSoundEffectAudioClips);
                }

                if (Plugin.CurrentConfig.BadHitSound == "None")
                {
                    ____badCutSoundEffectAudioClips = new AudioClip[] { SoundLoader.GetEmptyClip() };
                } else if (Plugin.CurrentConfig.BadHitSound == "Default")
                {
                    ____badCutSoundEffectAudioClips = _originalBadSounds.ToArray();
                }
                else if (_lastBadSelected != Plugin.CurrentConfig.BadHitSound)
                {
                    _lastBadSelected = Plugin.CurrentConfig.BadHitSound;
                    for (int i = 0; i < s_badCutArrayLength; i++)
                    {
                        if (_lastBadAudioClips[i] != null) {
                            GameObject.Destroy(_lastBadAudioClips[i]);
                            _lastBadAudioClips[i] = null;
                        }
                        _lastBadAudioClips[i] = SoundLoader.LoadAudioClip(_lastBadSelected);
                    }
                    ____badCutSoundEffectAudioClips = _lastBadAudioClips;
                }
                else
                {
                    ____badCutSoundEffectAudioClips = _lastBadAudioClips;
                }
            }
        }

        [HarmonyPatch(typeof(NoteCutSoundEffectManager))]
        [HarmonyPatch("Start", MethodType.Normal)]
        public class HitSoundsPatch
        {
            public static void Prefix(ref AudioClip[] ____longCutEffectsAudioClips, ref AudioClip[] ____shortCutEffectsAudioClips)
            {
                if (_originalGoodLongSounds == null)
                {
                    _originalGoodLongSounds = new List<AudioClip>();
                    _originalGoodLongSounds.AddRange(____longCutEffectsAudioClips);
                }

                if (_originalGoodShortSounds == null)
                {
                    _originalGoodShortSounds = new List<AudioClip>();
                    _originalGoodShortSounds.AddRange(____shortCutEffectsAudioClips);
                }

                if (Plugin.CurrentConfig.GoodHitSound == "None")
                {
                    ____longCutEffectsAudioClips = new AudioClip[] { SoundLoader.GetEmptyClip() };
                    ____shortCutEffectsAudioClips = new AudioClip[] { SoundLoader.GetEmptyClip() };
                }
                else if (Plugin.CurrentConfig.GoodHitSound == "Default")
                {
                    ____shortCutEffectsAudioClips = _originalGoodShortSounds.ToArray();
                    ____longCutEffectsAudioClips = _originalGoodLongSounds.ToArray();
                }
                else if (_lastGoodSelected != Plugin.CurrentConfig.GoodHitSound)
                {
                    _lastGoodSelected = Plugin.CurrentConfig.GoodHitSound;
                    for (int i = 0; i < s_goodCutArrayLength; i++)
                    {
                        if (_lastGoodAudioClips[i] != null)
                        {
                            GameObject.Destroy(_lastGoodAudioClips[i]);
                            _lastGoodAudioClips[i] = null;
                        }
                        _lastGoodAudioClips[i] = SoundLoader.LoadAudioClip(_lastGoodSelected); ;
                    }
                    ____shortCutEffectsAudioClips = _lastGoodAudioClips;
                    ____longCutEffectsAudioClips = _lastGoodAudioClips;
                }
                else
                {
                    ____shortCutEffectsAudioClips = _lastGoodAudioClips;
                    ____longCutEffectsAudioClips = _lastGoodAudioClips;
                }
            }
        }
    }
}
