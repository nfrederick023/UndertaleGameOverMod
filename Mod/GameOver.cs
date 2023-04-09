
using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace GameOverNamespace
{

    public class Dust
    {
        public UIImage dust;
        public float angle;

        public Dust(UIImage newDust, float newAngle)
        {
            dust = newDust;
            angle = newAngle;
        }
    }

    public class GameOverText
    {
        public UIImage gameOverText;
        public bool isVisible;

        public GameOverText(UIImage newnewGameOverTextt, bool newIsVisible)
        {
            gameOverText = newnewGameOverTextt;
            isVisible = newIsVisible;
        }
    }

    public class GameOverUI : UIState
    {
        public static bool textIsVisible;
        public static bool isVisible;
        public static bool isRestarting;
        private static UIPanel gameOverScreen;
        private static UIPanel gameOverTextOverlay;
        private static UIImage text;
        private static UIImage heart;
        private static GameOverText gameOverText;
        private static UIImage brokenHeart;

        private static Dust[] dusts = new Dust[6];

        private static int currentTextFrame = 0;
        private static int currentFrame = 0;

        private static float centerX = ((Main.screenWidth) / 2) + 10;
        private static float centerY = ((Main.screenHeight) / 2);

        private static int frameTimer = 0;
        private static int frameTime = 120;

        private static int frameTimerText = 0;
        private static int frameTimeText = 90;

        private static float endTimer = 0;
        private static float fadeTimer = 0;
        private static float time = 0;

        private static float soundVolume;
        private static float ambientVolume;
        private static float musicVolume;



        public override void OnInitialize()
        {
            float heartSize = 50;
            float dustSize = 10;
            float numberOfDust = 6;
            float brokenHeartWidth = 63;
            float gameOverHeight = 212;
            float gameOverWidth = 500;
            float textWidth = 375;
            float textHeight = 39;
            textIsVisible = false;
            isVisible = false;
            isRestarting = false;
            gameOverScreen = new UIPanel();
            gameOverScreen.Left.Set(-20, 0);
            gameOverScreen.Top.Set(-20, 0);
            gameOverScreen.BackgroundColor = Color.Black;
            gameOverScreen.MinWidth.Set(Main.screenWidth + 500, 0);
            gameOverScreen.MinHeight.Set(Main.screenHeight + 500, 0);


            brokenHeart = new UIImage(ModContent.Request<Texture2D>($"{nameof(UndertaleDeath)}/Img/HeartBroken"));
            brokenHeart.Left.Set(centerX + (heartSize) - 3, 0);
            brokenHeart.Top.Set(centerY + (heartSize), 0);
            brokenHeart.Width.Set(brokenHeartWidth, 0);
            brokenHeart.Height.Set(heartSize, 0);

            heart = new UIImage(ModContent.Request<Texture2D>($"{nameof(UndertaleDeath)}/Img/Heart"));
            heart.Left.Set(centerX + (heartSize), 0);
            heart.Top.Set(centerY + (heartSize), 0);
            heart.Width.Set(heartSize, 0);
            heart.Height.Set(heartSize, 0);

            UIImage gameOverTextImage = new UIImage(ModContent.Request<Texture2D>($"{nameof(UndertaleDeath)}/Img/GameOver"));
            gameOverTextImage.Left.Set(centerX - (gameOverWidth * 0.35f) + 10, 0);
            gameOverTextImage.Top.Set(centerY + (gameOverHeight / 2) - 300, 0);
            gameOverTextImage.Width.Set(gameOverWidth, 0);
            gameOverTextImage.Height.Set(gameOverHeight, 0);
            gameOverText = new GameOverText(gameOverTextImage, false);

            gameOverTextOverlay = new UIPanel();
            gameOverTextOverlay.Left.Set(centerX - (gameOverWidth * 0.4f) - 40, 0);
            gameOverTextOverlay.Top.Set(centerY + (gameOverHeight / 2) - 300 - 10, 0);
            gameOverTextOverlay.Width.Set(gameOverWidth + 80, 0);
            gameOverTextOverlay.Height.Set(gameOverHeight + 20, 0);
            gameOverTextOverlay.BackgroundColor = Color.Black;
            gameOverTextOverlay.BorderColor = Color.Black * 0.0f;

            text = new UIImage(getTextFrame());
            text.Left.Set(centerX - (textWidth * 0.30f) + 10, 0);
            text.Top.Set(centerY + (gameOverHeight / 2) + 200, 0);
            text.Width.Set(textWidth, 0);
            text.Height.Set(textHeight, 0);


            for (int i = 0; i < 13; i++)
            {
                currentTextFrame++;
                getTextFrame();
            }

            currentTextFrame = 0;

            for (int i = 0; i < numberOfDust; i++)
            {
                UIImage dust = new UIImage(getDustFrame());
                dust.ScaleToFit = true;
                dust.AllowResizingDimensions = true;
                dust.MinWidth.Set(dustSize, 0);
                dust.MinHeight.Set(dustSize, 0);
                dust.MaxWidth.Set(dustSize, 0);
                dust.MaxHeight.Set(dustSize, 0);
                dusts[i] = new Dust(dust, -1);

            }

            Append(gameOverScreen); //appends the panel to the UIState
        }

        public static Asset<Texture2D> getDustFrame()
        {
            return ModContent.Request<Texture2D>($"{nameof(UndertaleDeath)}/Img/Dust{currentFrame}");
        }

        public static Asset<Texture2D> getTextFrame()
        {
            return ModContent.Request<Texture2D>($"{nameof(UndertaleDeath)}/Img/dontlosehope/txt{currentTextFrame}");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Random rand = new Random();

            if (isRestarting)
            {
                float fadeOutTime = 2f;
                currentTextFrame = 0;
                text.SetImage(getTextFrame());

                if ((endTimer / 60) <= fadeOutTime)
                {
                    float percentage = (endTimer / 60) / fadeOutTime;
                    float fadeSoundVolume = (1 - (percentage));
                    gameOverTextOverlay.BackgroundColor = Color.Black * (percentage * 1.5f);
                    if (fadeSoundVolume <= soundVolume)
                    {
                        Main.soundVolume = (fadeSoundVolume);
                    }
                    endTimer++;

                }
                else
                {
                    isVisible = false;
                    CleanUp();
                }
            }

            if (isVisible)
            {

                time++;
                float fadeInTimeSeconds = 0.5f;
                float intialSpeed = 4.3f;
                float gravity = 1.3f;
                float timeInSec = (time / 60);

                // increment the frame timer
                frameTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

                // if it's time to change frames, move to the next frame
                if (frameTimer >= frameTime)
                {
                    currentFrame++;
                    for (int i = 0; i < dusts.Length - 1; i++)
                    {
                        dusts[i].dust.SetImage(getDustFrame());
                    }
                    if (currentFrame >= 3)
                    {
                        currentFrame = 0;
                    }
                    frameTimer = 0;
                }

                if (textIsVisible)
                {
                    frameTimerText += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                if (textIsVisible && frameTimerText >= frameTimeText && currentTextFrame < 13 && !isRestarting)
                {
                    currentTextFrame++;
                    Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.SoundStyle($"{nameof(UndertaleDeath)}/Sounds/Asgore"));

                    text.SetImage(getTextFrame());
                    frameTimerText = 0;
                }

                if (gameOverText.isVisible && (fadeTimer / 60) <= fadeInTimeSeconds)
                {
                    float percentage = (fadeTimer / 60) / fadeInTimeSeconds;
                    gameOverTextOverlay.BackgroundColor = Color.Black * (1 - percentage);
                    fadeTimer++;
                }

                for (int i = 0; i < dusts.Length - 1; i++)
                {
                    float angle = rand.Next(0, 360);
                    if (i < (dusts.Length / 2))
                    {
                        // atleast half of the dusts should fly up
                        angle = rand.Next(20, 130);
                    }
                    if (dusts[i].angle == -1)
                    {
                        dusts[i].angle = angle;
                    }
                    else
                    {
                        angle = dusts[i].angle;
                    }

                    double angleInRadians = angle * (Math.PI / 180.0);

                    float y1 = intialSpeed * (float)Math.Sin(angleInRadians) * timeInSec;
                    float y2 = gravity * (float)Math.Pow(timeInSec, 2f);
                    float x = intialSpeed * (float)Math.Cos(angleInRadians) * timeInSec;
                    float posX = dusts[i].dust.Left.Pixels;
                    float posY = dusts[i].dust.Top.Pixels;
                    dusts[i].dust.Left.Set((x) + posX, 0);
                    dusts[i].dust.Top.Set(((y1 - y2) * -1) + posY, 0);
                }
            }
            Recalculate();
        }

        public static void EndGameOver()
        {
            endTimer = 0;
            isRestarting = true;
        }

        public static void CleanUp()
        {
            gameOverText.isVisible = false;
            gameOverText.gameOverText.Remove();
            gameOverTextOverlay.Remove();
            text.Remove();
            text.SetImage(getTextFrame());
            Terraria.Audio.SoundEngine.StopTrackedSounds();


            for (int i = 0; i < dusts.Length - 1; i++)
            {
                dusts[i].dust.Remove();
                dusts[i].angle = -1;
            }
            time = 0;
            fadeTimer = 0;
            currentTextFrame = 0;
            Main.musicVolume = musicVolume;
            Main.ambientVolume = ambientVolume;
            Main.soundVolume = soundVolume;
        }

        public static void ActivateGameOver()
        {
            isRestarting = false;
            textIsVisible = false;
            musicVolume = Main.musicVolume;
            ambientVolume = Main.ambientVolume;
            soundVolume = Main.soundVolume;

            centerX = ((Main.screenWidth) / 2) + 10;
            centerY = ((Main.screenHeight) / 2);

            Main.musicVolume = 0f;
            Main.ambientVolume = 0f;

            gameOverScreen.Append(heart);

            isVisible = true;
            Task.Delay(1000).ContinueWith(_ =>
            {
                heart.Remove();
                gameOverScreen.Append(brokenHeart);
                Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.SoundStyle($"{nameof(UndertaleDeath)}/Sounds/Break1"));
            });

            Task.Delay(2250).ContinueWith(_ =>
             {
                 brokenHeart.Remove();
                 gameOverScreen.Append(gameOverText.gameOverText);
                 gameOverScreen.Append(gameOverTextOverlay);
                 gameOverScreen.Append(text);
                 for (int i = 0; i < dusts.Length - 1; i++)
                 {
                     dusts[i].dust.Left.Set(centerX + 75, 0);
                     dusts[i].dust.Top.Set(centerY + 75, 0);
                     gameOverScreen.Append(dusts[i].dust);
                 }

                 Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.SoundStyle($"{nameof(UndertaleDeath)}/Sounds/Break2"));
             });

            Task.Delay(3750).ContinueWith(_ =>
            {
                gameOverText.isVisible = true;
                Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.SoundStyle($"{nameof(UndertaleDeath)}/Sounds/Death"));
            });

            Task.Delay(4000).ContinueWith(_ =>
            {
                textIsVisible = true;
            });
        }
    }
}