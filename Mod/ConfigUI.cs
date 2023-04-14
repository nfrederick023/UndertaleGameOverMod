using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ConfigOptionsNamespace
{
    public class ConfigOptions : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Shorter GameOver Screen (eg for Calamity, Shorter Respawn Timer, etc mods)")]
        [Tooltip("Disables the Game Over theme, \"Game Over\" text, and Asgore's message to allow for a shorter respawn time.")]
        [DefaultValue(false)]
        public bool isShorterRespawn;

    }
}