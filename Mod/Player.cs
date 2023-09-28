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
            if (Player == Main.LocalPlayer)
                GameOverUI.ActivateGameOver();
        }
        public override void OnRespawn()
        {
            if (GameOverUI.isVisible && Player == Main.LocalPlayer)
                GameOverUI.EndGameOver();
        }

        public override bool CanUseItem(Item item)
        {
            // prevent player from using items if they've respawned before the game over screen is finished (eg Calamity short respawn time)
            if (GameOverUI.isVisible && Player == Main.LocalPlayer)
            {
                return false;
            }
            return true;
        }

        public override void PreUpdateMovement()
        {
            // prevent player from moving if they've respawned before the game over screen is finished (eg Calamity short respawn time)
            if (GameOverUI.isVisible && Player == Main.LocalPlayer)
            {
                Player.velocity.X = 0;
                Player.velocity.Y = 0;
            }
        }

        public override bool FreeDodge(Player.HurtInfo info)/* tModPorter Override ImmuneTo, FreeDodge or ConsumableDodge instead to prevent taking damage */
        {
            // prevent the player from being damaged if they've respawned before the game over screen is finished (eg Calamity short respawn time)
            if (GameOverUI.isVisible && Player == Main.LocalPlayer)
            {
                return true;
            }
            return false;
        }
    }
}