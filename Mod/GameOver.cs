
using System;
using ConfigOptionsNamespace;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace GameOverUINamespace
{

    public class Dust
    {
        public UIImage dust;
        public float angle;

        public Dust(UIImage dust, float angle)
        {
            this.dust = dust;
            this.angle = angle;
        }
    }

    public class AsgoreText
    {
        public string name;
        public int frames;

        public AsgoreText(string name, int frames)
        {
            this.name = name;
            this.frames = frames;
        }
    }

    public class GameOverUI : UIState
    {
        public static bool isVisible;
        public static bool isMusicPlaying;
        public static bool isRespawning;

        public static bool hasFinished;
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

        private static Random rand = new Random();

        private static float timeElapsedInGameTicks = 0;

        private static float soundVolume;
        private static float ambientVolume;
        private static float musicVolume;

        public static string currentTextName = "dontlosehope";

        public static bool preplay = true;

        private static AsgoreText[] asgoreTexts = new AsgoreText[6];

        public override void OnInitialize()
        {
            float numberOfDust = 6;
            isVisible = false;

            gameOverScreen = new UIPanel();
            brokenHeart = new UIImage(ModContent.Request<Texture2D>($"{nameof(UndertaleGameOver)}/Img/HeartBroken"));
            heart = new UIImage(ModContent.Request<Texture2D>($"{nameof(UndertaleGameOver)}/Img/Heart"));
            gameOverText = new UIImage(ModContent.Request<Texture2D>($"{nameof(UndertaleGameOver)}/Img/GameOver"));
            gameOverTextOverlay = new UIPanel();
            text = new UIImage(getTextFrame());

            currentTextFrame = 0;

            for (int i = 0; i < numberOfDust; i++)
            {
                UIImage dust = new UIImage(getDustFrame());
                dust.ScaleToFit = true;
                dust.AllowResizingDimensions = true;
                dusts[i] = new Dust(dust, -1);
            }

            gameOverScreen.BackgroundColor = Color.Black;
            gameOverScreen.Left.Set(-5, 0);
            gameOverScreen.Top.Set(-5, 0);
            gameOverScreen.MinWidth.Set(10, 1.0f);
            gameOverScreen.MinHeight.Set(10, 1.0f);

            float heartSize = 50;

            heart.Left.Set(-(heartSize / 2f), 0.5f);
            heart.Top.Set(-(heartSize / 2f), 0.5f);
            heart.Width.Set(heartSize, 0);
            heart.Height.Set(heartSize, 0);

            float brokenHeartWidth = 63;
            float brokenHeartHeight = heartSize;

            brokenHeart.Left.Set(-(brokenHeartWidth / 2f), 0.5f);
            brokenHeart.Top.Set(-(brokenHeartHeight / 2f), 0.5f);
            brokenHeart.Width.Set(brokenHeartWidth, 0);
            brokenHeart.Height.Set(brokenHeartHeight, 0);

            float gameOverTextWidth = 500;
            float gameOverTextHeight = 212;
            float gameOverTextYOffset = -150;


            float dustSize = 10;

            for (int i = 0; i < dusts.Length - 1; i++)
            {
                dusts[i].dust.Left.Set(-(dustSize / 2f), 0.5f);
                dusts[i].dust.Top.Set(-(dustSize / 2f), 0.5f);
                dusts[i].dust.MinWidth.Set(dustSize, 0);
                dusts[i].dust.MinHeight.Set(dustSize, 0);
                dusts[i].dust.MaxWidth.Set(dustSize, 0);
                dusts[i].dust.MaxHeight.Set(dustSize, 0);
            }

            gameOverText.Left.Set(-(gameOverTextWidth / 2f), 0.5f);
            gameOverText.Top.Set(-(gameOverTextHeight / 2f) + gameOverTextYOffset, 0.5f);
            gameOverText.Width.Set(gameOverTextWidth, 0);
            gameOverText.Height.Set(gameOverTextHeight, 0);

            float gameOverTextOverlayWidth = gameOverTextWidth + 20;
            float gameOverTextOverlayHeight = gameOverTextHeight + 20;
            float gameOverTextOverlayYOffset = gameOverTextYOffset;


            gameOverTextOverlay.Left.Set(-(gameOverTextOverlayWidth / 2f), 0.5f);
            gameOverTextOverlay.Top.Set(-(gameOverTextOverlayHeight / 2f) + gameOverTextOverlayYOffset, 0.5f);

            gameOverTextOverlay.Width.Set(gameOverTextOverlayWidth, 0);
            gameOverTextOverlay.Height.Set(gameOverTextOverlayHeight, 0);
            gameOverTextOverlay.BackgroundColor = Color.Black;
            gameOverTextOverlay.BorderColor = Color.Black * 0.0f;

            float textWidth = 375;
            float textHeight = 90;
            float textYOffset = 150;

            text.Left.Set(-(textWidth / 2f), 0.5f);
            text.Top.Set(-(textHeight / 2f) + textYOffset, 0.5f);
            text.Width.Set(textWidth, 0);
            text.Height.Set(textHeight, 0);

            gameOverScreen.Append(gameOverText);
            gameOverScreen.Append(gameOverTextOverlay);
            gameOverScreen.Append(text);

            Append(gameOverScreen); //appends the panel to the UIState

            asgoreTexts[0] = new AsgoreText("dontlosehope", 14);
            asgoreTexts[1] = new AsgoreText("cannotgiveup", 25);
            asgoreTexts[2] = new AsgoreText("ourfate", 22);
            asgoreTexts[3] = new AsgoreText("cantquit", 28);
            asgoreTexts[4] = new AsgoreText("alright", 23);
            asgoreTexts[5] = new AsgoreText("dunkedon", 22);

            // load all the image frames so they don't flash when loaded in game
            for (int i = 0; i < asgoreTexts.Length; i++)
            {
                currentTextName = asgoreTexts[i].name;
                for (int j = 0; j <= asgoreTexts[i].frames; j++)
                {
                    currentTextFrame = j;
                    getTextFrame();
                }
            }

            currentTextFrame = 0;

        }

        public static Asset<Texture2D> getDustFrame()
        {
            return ModContent.Request<Texture2D>($"{nameof(UndertaleGameOver)}/Img/Dust{currentFrame}");
        }

        public static Asset<Texture2D> getTextFrame()
        {
            return ModContent.Request<Texture2D>($"{nameof(UndertaleGameOver)}/Img/{currentTextName}/{currentTextFrame}");
        }

        public static void setRandomText()
        {

            int numberOfTexts = asgoreTexts.Length - 1;
            int selectedText = rand.Next(0, numberOfTexts);

            currentTextName = asgoreTexts[selectedText].name;
            maxTextFrames = asgoreTexts[selectedText].frames;

            //if certain skeletons kill the player, run a 5% chance to get dunked on 
            if (NPC.CountNPCS(35) > 0 || NPC.CountNPCS(127) > 0 || NPC.CountNPCS(21) > 0 || NPC.CountNPCS(77) > 0 || NPC.CountNPCS(292) > 0)
            {
                int rollForDunkedOn = rand.Next(0, 20);
                if (rollForDunkedOn == 0)
                {
                    currentTextName = asgoreTexts[5].name;
                    maxTextFrames = asgoreTexts[5].frames;
                }
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            timeElapsedInGameTicks++;

            if (isRespawning && hasFinished)
            {
                float fadeDuration = 120;
                if (currentTextFrame > 0)
                {
                    currentTextFrame = 0;
                    text.SetImage(getTextFrame());
                    isMusicPlaying = false;
                    timeElapsedInGameTicks = 0;
                }
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
                    Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.SoundStyle($"{nameof(UndertaleGameOver)}/Sounds/Break1"));
                }

                if (timeElapsedInGameTicks == 135)
                {
                    brokenHeart.Remove();

                    for (int i = 0; i < dusts.Length - 1; i++)
                    {
                        gameOverScreen.Append(dusts[i].dust);
                    }

                    Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.SoundStyle($"{nameof(UndertaleGameOver)}/Sounds/Break2"));
                }
                if (timeElapsedInGameTicks > 135)
                {
                    float intialSpeed = 9f;
                    float gravity = 5f;

                    // limits how far dusts fly horizontally
                    float horizontalLimiter = 0.6f;
                    float timeInSec = ((timeElapsedInGameTicks - 135) / 60) + 1;

                    for (int i = 0; i < dusts.Length - 1; i++)
                    {
                        float angle = rand.Next(0, 360);
                        if (i < (dusts.Length / 2))
                        {
                            // atleast half of the dusts should try to fly up because it looks cool
                            angle = rand.Next(40, 140);
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
                        dusts[i].dust.Left.Set((x * horizontalLimiter) + posX, 0.5f);
                        dusts[i].dust.Top.Set(((y1 - y2) * -1) + posY, 0.5f);
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
                }

                if (!ModContent.GetInstance<ConfigOptions>().isShorterRespawn)
                {

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
                        int newFrameEveryTick = 5;
                        // every 6 ticks if all the text frames haven't been shown, display the next one
                        if (timeElapsedInGameTicks % newFrameEveryTick == 0 && currentTextFrame < maxTextFrames)
                        {
                            currentTextFrame++;
                            text.SetImage(getTextFrame());
                        }

                        if (timeElapsedInGameTicks % 5 == 0 && currentTextFrame < maxTextFrames)
                        {
                            if (currentTextName == "dunkedon")
                            {
                                Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.SoundStyle($"{nameof(UndertaleGameOver)}/Sounds/Sans"));
                            }
                            else
                            {
                                Terraria.Audio.SoundEngine.PlaySound(new Terraria.Audio.SoundStyle($"{nameof(UndertaleGameOver)}/Sounds/Asgore"));
                            }
                        }

                        if ((timeElapsedInGameTicks - 360) > ((maxTextFrames + 1) * newFrameEveryTick) + 120)
                        {
                            hasFinished = true;
                        }
                    }
                }
                else
                {
                    if (timeElapsedInGameTicks == 220)
                    {
                        hasFinished = true;
                    }
                }

            }
            Recalculate();
        }

        public static void CleanUp()
        {
            float dustSize = 10;

            if (isVisible)
            {
                Main.musicVolume = musicVolume;
                Main.ambientVolume = ambientVolume;
                Main.soundVolume = soundVolume;
            }
            for (int i = 0; i < dusts.Length - 1; i++)
            {
                dusts[i].dust.Left.Set(-(dustSize / 2f), 0.5f);
                dusts[i].dust.Top.Set(-(dustSize / 2f), 0.5f);
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
            brokenHeart.Remove();
            heart.Remove();
            text.SetImage(getTextFrame());
            gameOverScreen.Append(text);
            gameOverTextOverlay.BackgroundColor = Color.Black * 1;
        }

        public static void EndGameOver()
        {
            if (isVisible)
            {
                isRespawning = true;
            }
        }

        public static void ActivateGameOver()
        {
            setRandomText();
            isVisible = true;
            hasFinished = false;
            timeElapsedInGameTicks = 0;
        }
    }
}