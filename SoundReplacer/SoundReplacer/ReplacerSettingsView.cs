using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using SoundReplacer.Configuration;
using System.Collections.Generic;


namespace SoundReplacer
{
    internal class ReplacerSettingsView : BSMLResourceViewController
    {
        public override string ResourceName => string.Join(".", this.GetType().Namespace, this.GetType().Name);

        [UIValue("good-hitsound-list")]
        public List<object> SettingsGoodHitSoundList = new List<object>(SoundLoader.GlobalSoundList);

        [UIValue("good-hitsound")]
        protected string SettingCurrentGoodHitSound
        {
            get => PluginConfig.Instance.GoodHitSound;
            set => PluginConfig.Instance.GoodHitSound = value;
        }

        [UIValue("bad-hitsound-list")]
        public List<object> SettingsBadHitSoundList = new List<object>(SoundLoader.GlobalSoundList);

        [UIValue("bad-hitsound")]
        protected string SettingCurrentBadHitSound
        {
            get => PluginConfig.Instance.BadHitSound;
            set => PluginConfig.Instance.BadHitSound = value;
        }

        [UIValue("menu-music-list")]
        public List<object> SettingsMenuMusicList = new List<object>(SoundLoader.GlobalSoundList);

        [UIValue("menu-music")]
        protected string SettingCurrentMenuMusic
        {
            get => PluginConfig.Instance.MenuMusic;
            set
            {
                PluginConfig.Instance.MenuMusic = value;
                Helper.RefreshMenuMusic();
            }
        }

        [UIValue("click-sound-list")]
        public List<object> SettingsClickSoundList = new List<object>(SoundLoader.GlobalSoundList);

        [UIValue("click-sound")]
        protected string SettingCurrentClickSound
        {
            get => PluginConfig.Instance.ClickSound;
            set
            {
                PluginConfig.Instance.ClickSound = value;
                Helper.RefreshClickSounds();
            }
        }

        [UIValue("success-sound-list")]
        public List<object> SettingsSuccessSoundList = new List<object>(SoundLoader.GlobalSoundList);

        [UIValue("success-sound")]
        protected string SettingCurrentSuccessSound
        {
            get => PluginConfig.Instance.SuccessSound;
            set => PluginConfig.Instance.SuccessSound = value;
        }

        [UIValue("fail-sound-list")]
        public List<object> SettingsSuccessFailList = new List<object>(SoundLoader.GlobalSoundList);

        [UIValue("fail-sound")]
        protected string SettingCurrentFailSound
        {
            get => PluginConfig.Instance.FailSound;
            set => PluginConfig.Instance.FailSound = value;
        }
    }
}
