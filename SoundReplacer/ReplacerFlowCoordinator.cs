using BeatSaberMarkupLanguage;
using HMUI;

namespace SoundReplacer
{
    internal class ReplacerFlowCoordinator : FlowCoordinator
    {
        private ReplacerSettingsView _settingsView;

        public void Awake()
        {
            if (this._settingsView == null) {
                this._settingsView = BeatSaberUI.CreateViewController<ReplacerSettingsView>();
            }
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (!firstActivation) {
                return;
            }

            this.SetTitle("SoundReplacer");
            this.showBackButton = true;
            this.ProvideInitialViewControllers(this._settingsView);
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            var mainFlow = BeatSaberUI.MainFlowCoordinator;
            mainFlow.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Horizontal);
        }
    }
}