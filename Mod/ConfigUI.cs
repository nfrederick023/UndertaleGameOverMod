using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ConfigOptionsNamespace
{
    public class ConfigOptions : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Shorter Gameover Screen (eg for Calamity)")]
        [Tooltip("Limits the GAMEOVER screen to the SOUL shatter to support a short respawn time.")]
        [DefaultValue(false)]
        public bool isShorterRespawn;

    }
}