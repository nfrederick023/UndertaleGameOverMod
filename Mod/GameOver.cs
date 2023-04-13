
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

    public class GameOverUI : UIState
    {
        public static bool isVisible;
        public static bool isMusicPlaying;
        public static bool isRespawning;
        private static UIPanel gameOverScreen;
        private static UIPanel gameOverTextOverlay;
        private static UIImage text;
        private static UIImage heart;
        private static UIImage gameOverText;
        private static UIImage brokenHeart;

        private static Dust[] dusts = new Dust[6];

        private static int currentTextFrame = 0;

        private static int maxTextFrames = 13;
        private static int currentFrame = 0;

        private static float centerX = ((Main.screenWidth) / 2) + 10;
        private static float centerY = ((Main.screenHeight) / 2);

        private static Random rand = new Random();

        private static float timeElapsedInGameTicks = 0;

        private static float soundVolume;
        private static float ambientVolume;
        private static float musicVolume;


        public static bool preplay = true;

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

            isVisible = false;

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

            gameOverText = new UIImage(ModContent.Request<Texture2D>($"{nameof(UndertaleDeath)}/Img/GameOver"));
            gameOverText.Left.Set(centerX - (gameOverWidth * 0.35f) + 10, 0);
            gameOverText.Top.Set(centerY + (gameOverHeight / 2) - 300, 0);
            gameOverText.Width.Set(gameOverWidth, 0);
            gameOverText.Height.Set(gameOverHeight, 0);

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

            gameOverScreen.Append(gameOverText);
            gameOverScreen.Append(gameOverTextOverlay);
            gameOverScreen.Append(text);

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
            timeElapsedInGameTicks++;

            if (isRespawning)
            {
                float fadeDuration = 120;
                currentTextFrame = 0;
                text.SetImage(getTextFrame());
                isMusicPlaying = false;

                if (gameOverTextOverlay.BackgroundColor != Color.Black * 1)
                {
                    gameOverTextOverlay.BackgroundColor = Color.Black * (timeElapsedInGameTicks / fadeDuration);
                }
                else
                {
                    CleanUp();
                }
            }
            else
            {
                if (timeElapsedInGameTicks == 1)
                {
                    ambientVolume = Main.ambientVolume;
                    soundVolume = Main.soundVolume;
                    musicVolume = Main.musicVolume;
                    Main.ambientVolume = 0f;
                    Main.musicVolume = 0;

                    gameOverScreen.Append(heart);
                }

                if (timeElapsedInGameTicks == 60)
                {
                    heart.Remove();
                    gameOverScreen.Append(brokenHeart);
                    Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.SoundStyle($"{nameof(UndertaleDeath)}/Sounds/Break1"));
                }

                if (timeElapsedInGameTicks == 135)
                {
                    brokenHeart.Remove();

                    for (int i = 0; i < dusts.Length - 1; i++)
                    {
                        dusts[i].dust.Left.Set(centerX + 75, 0);
                        dusts[i].dust.Top.Set(centerY + 75, 0);
                        gameOverScreen.Append(dusts[i].dust);
                    }

                    Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.SoundStyle($"{nameof(UndertaleDeath)}/Sounds/Break2"));
                }


                if (timeElapsedInGameTicks == 220)
                {
                    Main.musicVolume = musicVolume;
                    isMusicPlaying = true;
                }

                if (timeElapsedInGameTicks >= 220)
                {
                    int fadeDuration = 60;
                    float percentage = (1 - ((timeElapsedInGameTicks - 225) / fadeDuration));
                    gameOverTextOverlay.BackgroundColor = Color.Black * percentage;
                }

                if (timeElapsedInGameTicks >= 360)
                {
                    // every 6 ticks if all the text frames haven't been shown, display the next one
                    if ((timeElapsedInGameTicks - 360) % 6 == 0 && currentTextFrame < maxTextFrames)
                    {
                        currentTextFrame++;
                        Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.SoundStyle($"{nameof(UndertaleDeath)}/Sounds/Asgore"));
                        text.SetImage(getTextFrame());
                    }
                }

                // every 8 ticks increment the heart shard frame
                if ((timeElapsedInGameTicks) % 8 == 0)
                {
                    currentFrame++;
                    if (currentFrame >= 3)
                    {
                        currentFrame = 0;
                    }
                    for (int i = 0; i < dusts.Length - 1; i++)
                    {
                        dusts[i].dust.SetImage(getDustFrame());
                    }
                }

                float intialSpeed = 4.3f;
                float gravity = 1.3f;
                float timeInSec = timeElapsedInGameTicks / 60;

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

        public static void CleanUp()
        {
            if (isVisible)
            {
                Main.musicVolume = musicVolume;
                Main.ambientVolume = ambientVolume;
                Main.soundVolume = soundVolume;
            }
            for (int i = 0; i < dusts.Length - 1; i++)
            {
                dusts[i].dust.Remove();
                dusts[i].angle = -1;
            }

            isRespawning = false;
            isVisible = false;
            isMusicPlaying = false;
            timeElapsedInGameTicks = 0;
            currentFrame = 0;
            currentTextFrame = 0;
            text.Remove();
            text.SetImage(getTextFrame());
            gameOverScreen.Append(text);
            gameOverTextOverlay.BackgroundColor = Color.Black * 1;
        }

        public static void EndGameOver()
        {
            isRespawning = true;
            timeElapsedInGameTicks = 0;
        }

        public static void ActivateGameOver()
        {
            isVisible = true;
            timeElapsedInGameTicks = 0;
        }
    }
}