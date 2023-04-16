using GameOverUINamespace;
using Terraria;
using Terraria.ModLoader;

namespace DogSongNamespace
{
    public class DogSong : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BossHigh;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/DogSong");
        public override bool IsSceneEffectActive(Player player)
        {
            return GameOverUI.isMusicPlaying && GameOverUI.currentTextName == "dunkedon";
        }
    }

}