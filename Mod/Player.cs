using GameOverUINamespace;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace UndertaleDeathPlayerNamespace
{
    public class UndertaleDeathPlayer : ModPlayer
    {
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            GameOverUI.ActivateGameOver();
        }
        public override void OnRespawn(Player player)
        {
            GameOverUI.EndGameOver();
        }

        public override bool CanUseItem(Item item)
        {
            // prevent player from using items if they've respawned before the game over screen is finished (eg Calamity short respawn time)
            if (GameOverUI.isVisible)
            {
                return false;
            }
            return true;
        }

        public override void PreUpdateMovement()
        {
            // prevent player from moving if they've respawned before the game over screen is finished (eg Calamity short respawn time)
            if (GameOverUI.isVisible)
            {
                Player.velocity.X = 0;
                Player.velocity.Y = 0;
            }
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
        {
            // prevent the player from being damaged if they've respawned before the game over screen is finished (eg Calamity short respawn time)
            if (GameOverUI.isVisible)
            {
                return false;
            }
            return true;
        }
    }
}