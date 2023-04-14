using System.Collections.Generic;
using GameOverUINamespace;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace UndertaleGameOverModNamespace
{
    public class UndertaleGameOverMod : ModSystem
    {
        public GameOverUI gameoverUI;
        public UserInterface gameoverInterface;

        private int prevMusic = 0;
        public override void Load()
        {
            MusicLoader.AddMusic(Mod, "Sounds/Death");
            MusicLoader.AddMusic(Mod, "Sounds/Silence");
            MusicLoader.AddMusic(Mod, "Sounds/DogSong");
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
                Terraria.Main.musicFade[MusicLoader.GetMusicSlot(Mod, "Sounds/Silence")] = 1f;
                Terraria.Main.musicFade[MusicLoader.GetMusicSlot(Mod, "Sounds/Death")] = 1f;
                Terraria.Main.musicFade[MusicLoader.GetMusicSlot(Mod, "Sounds/DogSong")] = 1f;

                if (Main.curMusic != prevMusic)
                {
                    prevMusic = Main.curMusic;
                    Mod.Logger.Debug(Main.curMusic);
                    Mod.Logger.Debug(Terraria.Main.musicFade[Main.curMusic]);
                    Mod.Logger.Debug("next");

                }

                gameoverInterface?.Update(gameTime);
            }

        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            layers.Add(new LegacyGameInterfaceLayer("", DrawSomethingUI, InterfaceScaleType.UI));
        }

        public override void PreSaveAndQuit()
        {
            GameOverUI.CleanUp();
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