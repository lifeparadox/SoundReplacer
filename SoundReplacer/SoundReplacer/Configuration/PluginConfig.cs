using IPA.Config.Stores;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace SoundReplacer.Configuration
{
    public class PluginConfig
    {
        public static PluginConfig Instance { get; internal set; } = null!;

        public virtual string GoodHitSound { get; set; } = "Default";
        public virtual string BadHitSound { get; set; } = "Default";

        public virtual string MenuMusic { get; set; } = "Default";
        public virtual string ClickSound { get; set; } = "Default";

        public virtual string SuccessSound { get; set; } = "Default";
        public virtual string FailSound { get; set; } = "Default";

        /// <summary>
        /// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
        /// </summary>
        public virtual void OnReload()
        {

        }
        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed()
        {
            // Do stuff when the config is changed.
        }

        /// <summary>
        /// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
        /// </summary>
        public virtual void CopyFrom(PluginConfig other)
        {
        }
    }
}
