using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ConfigOptionsNamespace
{
    public class ConfigOptions : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Shorter Gameover Screen (eg for Calamity)")]
        [Tooltip("Supports short respawn times.")]
        [DefaultValue(false)]
        public bool isShorterRespawn;

    }
}