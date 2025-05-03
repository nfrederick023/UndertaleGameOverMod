using System.ComponentModel;
using Terraria.ModLoader.Config;
using Microsoft.Xna.Framework;

namespace ConfigOptionsNamespace
{
    public class ConfigOptions : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Shorter Gameover Screen (eg for Calamity)")]
        [Tooltip("Supports short respawn times.")]
        [DefaultValue(false)]
        public bool isShorterRespawn;

        [Label("Dunked On! Easter Egg")]
        [Tooltip("Enables/Disables the Dunked On! Easter Egg.")]
        [DefaultValue(true)]
        public bool isDunkedOn;

        [Label("Heart Color")]
        [Tooltip("Changes the color of the heart.")]
        [DefaultValue("255, 0, 0, 255")] 
        public Color heartColor;

    }
}