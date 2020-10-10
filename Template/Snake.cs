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
        Vector2 bgPos = new Vector2(0, 0);
        Vector2 strawberryPos = new Vector2(480, 280);
        List<Vector2> tailPos = new List<Vector2>();
        List<Vector2> recordPos = new List<Vector2>();
        Texture2D snakeHead, background, strawberry, tail;
        int newDirection = 0, oldDirection, scoreInt = 0, highScoreInt = 0, lastScoreInt = 0, cooldown = 0;
        SpriteFont score, title, start;
        Random random = new Random();
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
            snakeHead = new Texture2D(GraphicsDevice, 1, 1);
            snakeHead.SetData(new[] { Color.White });
            tail = new Texture2D(GraphicsDevice, 1, 1);
            tail.SetData(new[] { Color.White });
            strawberry = new Texture2D(GraphicsDevice, 1, 1);
            strawberry.SetData(new[] { Color.White });
            background = Content.Load<Texture2D>("Snake_BG");
            score = Content.Load<SpriteFont>("Score");
            title = Content.Load<SpriteFont>("Title");
            start = Content.Load<SpriteFont>("Start");
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            spriteBatch.Dispose();
            snakeHead.Dispose();
            background.Dispose();
            strawberry.Dispose();
            tail.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();



            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.W) && oldDirection != 3 && cooldown == 0)
            {
                newDirection = 1;
            }

            else if (keyboardState.IsKeyDown(Keys.A) && oldDirection != 4 && cooldown == 0)
            {
                newDirection = 2;
            }

            else if (keyboardState.IsKeyDown(Keys.S) && oldDirection != 1 && cooldown == 0)
            {
                newDirection = 3;
            }

            else if (keyboardState.IsKeyDown(Keys.D) && oldDirection != 2 && cooldown == 0)
            {
                newDirection = 4;
            }
            else if (cooldown != 0)
            {
                cooldown--;
            }

            if (newDirection != 0)
            {
                spela = true;
            }

            if (spela == true)
            {

                if (headPos.X % 40 == 0 && headPos.Y % 40 == 0)
                {

                    if (newDirection == 1)
                    {
                        headPos.Y -= 5;
                    }

                    else if (newDirection == 2)
                    {
                        headPos.X -= 5;
                    }

                    else if (newDirection == 3)
                    {
                        headPos.Y += 5;
                    }

                    else if (newDirection == 4)
                    {
                        headPos.X += 5;
                    }
                    oldDirection = newDirection;

                }

                else
                {

                    if (oldDirection == 1)
                    {
                        headPos.Y -= 5;
                    }

                    else if (oldDirection == 2)
                    {
                        headPos.X -= 5;
                    }

                    else if (oldDirection == 3)
                    {
                        headPos.Y += 5;
                    }

                    else if (oldDirection == 4)
                    {
                        headPos.X += 5;
                    }

                }

                recordPos.Add(headPos);

                if (recordPos.Count > tailPos.Count + 8)
                {
                    recordPos.RemoveAt(0);
                }

                for (int i = 0; i < tailPos.Count; i++)
                {
                    tailPos[i] = recordPos[i];
                    if (tailPos[i] == headPos)
                    {
                        Reset();
                    }
                }

                if (headPos == strawberryPos)
                {
                    strawberryPos.X = random.Next(17) * 40;
                    strawberryPos.Y = random.Next(15) * 40;
                    scoreInt++;

                    if (tailPos.Count == 0)
                    {
                        tailPos.Add(headPos);
                    }
                    else
                    {
                        for (int x = 0; x < 8; x++)
                        {
                            tailPos.Add(headPos);
                        }
                    }
                }



                if (headPos.X > 640 || headPos.X < 0 || headPos.Y > 560 || headPos.Y < 0)
                {
                    Reset();
                }
            }

            else
            {
                
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.White);

            base.Draw(gameTime);

            spriteBatch.Begin();

            spriteBatch.Draw(background, bgPos, Color.White);
            foreach (Vector2 tailPos in tailPos)
            {
                Rectangle rec = new Rectangle();
                rec.Location = tailPos.ToPoint();
                rec.Size = new Point(40, 40);
                spriteBatch.Draw(tail, rec, Color.MediumPurple);
            }
            spriteBatch.Draw(strawberry, new Rectangle((int)strawberryPos.X, (int)strawberryPos.Y, 40, 40), Color.Red);
            spriteBatch.Draw(snakeHead, new Rectangle((int)headPos.X, (int)headPos.Y, 40, 40), Color.Purple);

            if (spela == true)
            {
                spriteBatch.DrawString(score, "Score: " + scoreInt.ToString(), new Vector2(10, 10), Color.Black);
            }

            else
            {
                spriteBatch.DrawString(title, "Snake", new Vector2(215, 160), Color.Black);
                spriteBatch.DrawString(score, "Last Score: " + lastScoreInt.ToString(), new Vector2(245, 242), Color.Black);
                spriteBatch.DrawString(score, "High Score: " + highScoreInt.ToString(), new Vector2(242, 282), Color.Black);
                spriteBatch.DrawString(start, "Press W, A, S or D to start", new Vector2(198, 325), Color.Black);
            }

            spriteBatch.End();

        }
        private void Reset()
        {
            if (scoreInt > highScoreInt)
            {
                highScoreInt = scoreInt;
            }
            lastScoreInt = scoreInt;
            scoreInt = 0;
            tailPos.RemoveRange(0, tailPos.Count);
            recordPos.RemoveRange(0, recordPos.Count);
            newDirection = 0;
            oldDirection = 0;
            headPos = new Vector2(160, 280);
            strawberryPos = new Vector2(480, 280);
            cooldown = 30;
            spela = false;
        }
    }
}