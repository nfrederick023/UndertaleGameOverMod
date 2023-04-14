using GameOverUINamespace;
using Terraria;
using Terraria.ModLoader;

namespace DeathNamespace
{

    public class Death : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Death");
        public override bool IsSceneEffectActive(Player player)
        {
            return GameOverUI.isMusicPlaying && GameOverUI.currentTextName != "dunkedon";
        }
    }

}