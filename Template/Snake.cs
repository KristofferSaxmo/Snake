using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Snake
{

    public class Snake : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Vector2 headPos = new Vector2(160, 280);
        Vector2 backgroundPos = new Vector2(0, 0);
        Vector2 ApplePos = new Vector2(480, 280);
        List<Vector2> bodyPos = new List<Vector2>();
        List<Vector2> recordPos = new List<Vector2>();
        Texture2D headTex, backgroundTex, AppleTex, bodyTex;
        int newDirection = 0, oldDirection, scoreInt = 0, highScoreInt = 0, lastScoreInt = 0, cooldownInt = 0;
        SpriteFont scoreFont, titleFont, startFont;
        FruitApple Apple = new FruitApple();

        bool spela = false;

        public Snake()
        {

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 680;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            base.Initialize();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            headTex = new Texture2D(GraphicsDevice, 1, 1);
            headTex.SetData(new[] { Color.White });
            bodyTex = new Texture2D(GraphicsDevice, 1, 1);
            bodyTex.SetData(new[] { Color.White });
            AppleTex = new Texture2D(GraphicsDevice, 1, 1);
            AppleTex.SetData(new[] { Color.White });
            backgroundTex = Content.Load<Texture2D>("Snake_BG");
            scoreFont = Content.Load<SpriteFont>("Score");
            titleFont = Content.Load<SpriteFont>("Title");
            startFont = Content.Load<SpriteFont>("Start");
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            spriteBatch.Dispose();
            headTex.Dispose();
            backgroundTex.Dispose();
            AppleTex.Dispose();
            bodyTex.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            var keyboardState = Keyboard.GetState();


            if (keyboardState.IsKeyDown(Keys.W) && oldDirection != 3 && cooldownInt == 0)
                newDirection = 1;

            else if (keyboardState.IsKeyDown(Keys.A) && oldDirection != 4 && cooldownInt == 0)
                newDirection = 2;

            else if (keyboardState.IsKeyDown(Keys.S) && oldDirection != 1 && cooldownInt == 0)
                newDirection = 3;

            else if (keyboardState.IsKeyDown(Keys.D) && oldDirection != 2 && cooldownInt == 0)
                newDirection = 4;

            else if (cooldownInt != 0)
                cooldownInt--;


            if (newDirection != 0)
                spela = true;


            if (spela == true)
            {

                if (headPos.X % 40 == 0 && headPos.Y % 40 == 0)
                {

                    if (newDirection == 1)
                        headPos.Y -= 5;

                    else if (newDirection == 2)
                        headPos.X -= 5;

                    else if (newDirection == 3)
                        headPos.Y += 5;

                    else if (newDirection == 4)
                        headPos.X += 5;

                    oldDirection = newDirection;

                }

                else
                {

                    if (oldDirection == 1)
                        headPos.Y -= 5;

                    else if (oldDirection == 2)
                        headPos.X -= 5;

                    else if (oldDirection == 3)
                        headPos.Y += 5;

                    else if (oldDirection == 4)
                        headPos.X += 5;

                }

                recordPos.Add(headPos);

                if (recordPos.Count > bodyPos.Count + 8)
                    recordPos.RemoveAt(0);

                for (int i = 0; i < bodyPos.Count; i++)
                {
                    bodyPos[i] = recordPos[i];
                    if (bodyPos[i] == headPos)
                        Reset();
                }

                Apple.ChangeBodyLength();

                if (headPos.X > 640 || headPos.X < 0 || headPos.Y > 560 || headPos.Y < 0)
                    Reset();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.White);

            base.Draw(gameTime);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTex, backgroundPos, Color.White);
            foreach (Vector2 tailPos in bodyPos)
            {
                Rectangle rec = new Rectangle();
                rec.Location = tailPos.ToPoint();
                rec.Size = new Point(40, 40);
                spriteBatch.Draw(bodyTex, rec, Color.MediumPurple);
            }
            spriteBatch.Draw(AppleTex, new Rectangle((int)ApplePos.X, (int)ApplePos.Y, 40, 40), Color.Red);
            spriteBatch.Draw(headTex, new Rectangle((int)headPos.X, (int)headPos.Y, 40, 40), Color.Purple);

            if (spela == true)
                spriteBatch.DrawString(scoreFont, "Score: " + scoreInt.ToString(), new Vector2(10, 10), Color.Black);

            else
            {
                spriteBatch.DrawString(titleFont, "Snake", new Vector2(215, 160), Color.Black);
                spriteBatch.DrawString(scoreFont, "Last Score: " + lastScoreInt.ToString(), new Vector2(245, 242), Color.Black);
                spriteBatch.DrawString(scoreFont, "High Score: " + highScoreInt.ToString(), new Vector2(242, 282), Color.Black);
                spriteBatch.DrawString(startFont, "Press W, A, S or D to start", new Vector2(198, 325), Color.Black);
            }

            spriteBatch.End();

        }
        private void Reset()
        {
            if (scoreInt > highScoreInt)
                highScoreInt = scoreInt;

            lastScoreInt = scoreInt;
            scoreInt = 0;
            bodyPos.RemoveRange(0, bodyPos.Count);
            recordPos.RemoveRange(0, recordPos.Count);
            newDirection = 0;
            oldDirection = 0;
            headPos = new Vector2(160, 280);
            ApplePos = new Vector2(480, 280);
            cooldownInt = 30;
            spela = false;
        }
    }
}