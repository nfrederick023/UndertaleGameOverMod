using GameOverNamespace;
using Terraria;
using Terraria.ModLoader;

namespace SceneEffectNamespace
{

    public class MyScene : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Death");
        public override bool IsSceneEffectActive(Player player)
        {
            return true;
        }

    }
}