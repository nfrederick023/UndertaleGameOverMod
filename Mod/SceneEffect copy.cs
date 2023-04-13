using GameOverNamespace;
using Terraria;
using Terraria.ModLoader;

namespace SceneEffectNamespace
{

    public class MyScene2 : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Silence");
        public override bool IsSceneEffectActive(Player player)
        {
            return GameOverUI.isVisible;
        }
    }

}