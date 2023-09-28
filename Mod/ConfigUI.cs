using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ConfigOptionsNamespace
{
    public class ConfigOptions : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [LabelKey("Shorter Gameover Screen (eg for Calamity)")]
        [TooltipKey("Supports short respawn times.")]
        [DefaultValue(false)]
        public bool isShorterRespawn;

    }
}