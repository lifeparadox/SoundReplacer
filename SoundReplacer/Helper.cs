﻿namespace SoundReplacer
{
    internal class Helper
    {
        public static void RefreshMenuMusic()
        {
            var previews = UnityEngine.Object.FindObjectsOfType<SongPreviewPlayer>();
            foreach (var preview in previews) {
                preview.Start();
                preview.CrossfadeToDefault();
            }
        }

        public static void RefreshClickSounds()
        {
            var audioManagers = UnityEngine.Object.FindObjectsOfType<BasicUIAudioManager>();
            foreach (var audioManager in audioManagers) {
                audioManager.Start();
            }
        }
    }
}
