using GameOverUINamespace;
using Terraria;
using Terraria.ModLoader;

namespace SilenceNamespaceNamespace
{

    public class Silence : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Silence");
        public override bool IsSceneEffectActive(Player player)
        {
            return GameOverUI.isVisible;
        }
    }

}