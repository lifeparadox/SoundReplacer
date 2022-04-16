using HarmonyLib;
using System;
using System.Collections.Generic;
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

        private const int s_badCutArrayLength = 4;
        private const int s_goodCutArrayLength = 10;

        [HarmonyPatch(typeof(NoteCutSoundEffect))]

        public class BadCutSoundPatch
        {
            [HarmonyPatch(nameof(NoteCutSoundEffect.Awake))]
            [HarmonyPrefix]
            public static void AwakePrefix(ref AudioClip[] ____badCutSoundEffectAudioClips)
            {
                if (_originalBadSounds == null) {
                    _originalBadSounds = new List<AudioClip>();
                    _originalBadSounds.AddRange(____badCutSoundEffectAudioClips);
                }

                if (PluginConfig.Instance.BadHitSound == "None") {
                    ____badCutSoundEffectAudioClips = new AudioClip[] { SoundLoader.GetEmptyClip() };
                }
                else if (PluginConfig.Instance.BadHitSound == "Default") {
                    ____badCutSoundEffectAudioClips = _originalBadSounds.ToArray();
                }
                else if (_lastBadSelected != PluginConfig.Instance.BadHitSound) {
                    _lastBadSelected = PluginConfig.Instance.BadHitSound;
                    for (var i = 0; i < s_badCutArrayLength; i++) {
                        if (_lastBadAudioClips[i] != null) {
                            GameObject.Destroy(_lastBadAudioClips[i]);
                            _lastBadAudioClips[i] = null;
                        }
                        _lastBadAudioClips[i] = SoundLoader.LoadAudioClip(_lastBadSelected);
                    }
                    ____badCutSoundEffectAudioClips = _lastBadAudioClips;
                }
                else {
                    ____badCutSoundEffectAudioClips = _lastBadAudioClips;
                }
            }
            /// <summary>
            /// バッドカット音が想定より長い場合再生されない不具合用のパッチ
            /// </summary>
            /// <param name="____endDSPtime"></param>
            /// <param name="noteCutInfo"></param>
            [HarmonyPatch(nameof(NoteCutSoundEffect.NoteWasCut), new Type[] { typeof(NoteController), typeof(NoteCutInfo) }, new ArgumentType[] { ArgumentType.Normal, ArgumentType.Ref })]
            [HarmonyPostfix]
            public static void NotewasCutPostfix(ref double ____endDSPtime, NoteCutInfo noteCutInfo)
            {
                if (!noteCutInfo.allIsOK && -0.5 < AudioSettings.dspTime - ____endDSPtime) {
                    ____endDSPtime = ____endDSPtime + 0.5;
                }
            }

            /// <summary>
            /// 勝手に音程変えないでくれ
            /// </summary>
            /// <param name="____pitch"></param>
            /// <param name="____aheadTime"></param>
            /// <param name="____audioSource"></param>
            /// <param name="aheadTime"></param>
            [HarmonyPatch(nameof(NoteCutSoundEffect.Init))]
            [HarmonyPostfix]
            public static void InitPostfix(ref float ____pitch, ref float ____aheadTime, ref AudioSource ____audioSource, float aheadTime)
            {
                ____pitch = 1;
                ____aheadTime = aheadTime / ____pitch;
                ____audioSource.pitch = ____pitch;
            }
#if false
            [HarmonyPatch(nameof(NoteCutSoundEffect.NoteWasCut))]
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> NoteWasCutTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                var maches = new CodeMatcher(instructions);
                maches
                    .Start()
                    .MatchForward(false,
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(NoteCutSoundEffect), "_audioSource")),
                    new CodeMatch(i => i.opcode == OpCodes.Ldc_I4_S && int.Parse($"{i.operand}") == 16),
                    new CodeMatch(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(AudioSource), nameof(AudioSource.priority))))
                    .ThrowIfInvalid("0")
                    .Advance(2)
                    .Set(OpCodes.Ldc_I4_S, 0);
                return maches.InstructionEnumeration();
            }
#endif
        }

        [HarmonyPatch(typeof(NoteCutSoundEffectManager))]
        [HarmonyPatch("Start", MethodType.Normal)]
        public class HitSoundsPatch
        {
            public static void Prefix(ref AudioClip[] ____longCutEffectsAudioClips, ref AudioClip[] ____shortCutEffectsAudioClips)
            {
                if (_originalGoodLongSounds == null) {
                    _originalGoodLongSounds = new List<AudioClip>();
                    _originalGoodLongSounds.AddRange(____longCutEffectsAudioClips);
                }

                if (_originalGoodShortSounds == null) {
                    _originalGoodShortSounds = new List<AudioClip>();
                    _originalGoodShortSounds.AddRange(____shortCutEffectsAudioClips);
                }

                if (PluginConfig.Instance.GoodHitSound == "None") {
                    ____longCutEffectsAudioClips = new AudioClip[] { SoundLoader.GetEmptyClip() };
                    ____shortCutEffectsAudioClips = new AudioClip[] { SoundLoader.GetEmptyClip() };
                }
                else if (PluginConfig.Instance.GoodHitSound == "Default") {
                    ____shortCutEffectsAudioClips = _originalGoodShortSounds.ToArray();
                    ____longCutEffectsAudioClips = _originalGoodLongSounds.ToArray();
                }
                else if (_lastGoodSelected != PluginConfig.Instance.GoodHitSound) {
                    _lastGoodSelected = PluginConfig.Instance.GoodHitSound;
                    for (var i = 0; i < s_goodCutArrayLength; i++) {
                        if (_lastGoodAudioClips[i] != null) {
                            GameObject.Destroy(_lastGoodAudioClips[i]);
                            _lastGoodAudioClips[i] = null;
                        }
                        _lastGoodAudioClips[i] = SoundLoader.LoadAudioClip(_lastGoodSelected); ;
                    }
                    ____shortCutEffectsAudioClips = _lastGoodAudioClips;
                    ____longCutEffectsAudioClips = _lastGoodAudioClips;
                }
                else {
                    ____shortCutEffectsAudioClips = _lastGoodAudioClips;
                    ____longCutEffectsAudioClips = _lastGoodAudioClips;
                }
            }
        }
    }
}
