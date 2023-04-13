using System.Threading.Tasks;
using GameOverNamespace;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace UndertaleDeathPlayer
{
    public class MyPlayer : ModPlayer
    {
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            GameOverUI.ActivateGameOver();
        }

        public override void Initialize()
        {
            GameOverUI.CleanUp();
        }

        public override void OnRespawn(Player player)
        {
            GameOverUI.EndGameOver();
        }
    }
}