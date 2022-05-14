using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using UnityEngine;

namespace SoundReplacer
{
    public class SoundReplacerController : MonoBehaviour
    {
        public static SoundReplacerController? Instance { get; private set; }

        private ReplacerFlowCoordinator? _flowCoordinator;

        private void Awake()
        {
            if (Instance != null) {
                GameObject.DestroyImmediate(this);
                return;
            }
            GameObject.DontDestroyOnLoad(this);
            Instance = this;

            MenuButtons.instance.RegisterButton(new MenuButton("SoundReplacer", "Setup SoundReplacer here!", this.MenuButtonPressed, true));
        }

        private void OnDestroy()
        {
            if (Instance == this) {
                Instance = null;
            }
        }

        private void MenuButtonPressed()
        {
            if (this._flowCoordinator == null) {
                this._flowCoordinator = BeatSaberMarkupLanguage.BeatSaberUI.CreateFlowCoordinator<ReplacerFlowCoordinator>();
            }

            BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinatorOrAskForTutorial(this._flowCoordinator);
        }
    }
}
