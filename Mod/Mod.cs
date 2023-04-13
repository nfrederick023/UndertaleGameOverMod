using System.Collections.Generic;
using GameOverNamespace;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace TerrariaUITutorial
{
    public class TerrariaUITutorial : ModSystem
    {
        public GameOverUI gameoverUI;
        public UserInterface gameoverInterface;
        public override void Load()
        {
            MusicLoader.AddMusic(Mod, "Sounds/Death");

            // this makes sure that the UI doesn't get opened on the server
            // the server can't see UI, can it? it's just a command prompt
            if (!Main.dedServ)
            {
                gameoverUI = new GameOverUI();
                gameoverUI.Initialize();
                gameoverInterface = new UserInterface();
                gameoverInterface.SetState(gameoverUI);

            }
        }

        public override void PostUpdateTime()
        {
            base.PostUpdateTime();
        }

        public override void UpdateUI(GameTime gameTime)
        {

            // it will only draw if the player is not on the main menu
            if (!Main.gameMenu && GameOverUI.isVisible && !Main.gamePaused)
            {
                gameoverInterface?.Update(gameTime);
            }

            if (!Main.gameMenu && !Main.gamePaused)
            {
                //Terraria.Main.musicFade[Main.curMusic] = 0.5f;
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            layers.Add(new LegacyGameInterfaceLayer("Cool Mod: Something UI", DrawSomethingUI, InterfaceScaleType.UI));
        }

        private bool DrawSomethingUI()
        {
            // it will only draw if the player is not on the main menu
            if (!Main.gameMenu && GameOverUI.isVisible && !Main.gamePaused)
            {
                gameoverInterface.Draw(Main.spriteBatch, new GameTime());
            }
            return true;
        }
    }
}