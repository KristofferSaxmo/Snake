using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Snake
{
    class FruitApple : FruitBase
    {
        public FruitApple()
        {

        }
        public override void ChangeBodyLength(Texture2D FruitTex, Vector2 FruitPos, ref int Score, Random random, Vector2 headPos, List<Vector2> bodyPos)
        {
            if (headPos == FruitPos)
            {
                FruitPos.X = random.Next(17) * 40;
                FruitPos.Y = random.Next(15) * 40;
                ScoreInt++;

                if (bodyPos.Count == 0)
                    bodyPos.Add(headPos);

                else
                {
                    for (int x = 0; x < 8; x++)
                        bodyPos.Add(headPos);
                }
            }
        }
    }
}