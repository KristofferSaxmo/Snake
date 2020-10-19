using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Snake
{
    abstract class FruitBase
    {
        public Texture2D FruitTex { get; protected set; }
        public Vector2 FruitPos { get; protected set; }
        public int ScoreInt { get; protected set; }

        Random random = new Random();

        public abstract void ChangeBodyLength(Texture2D FruitTex, Vector2 FruitPos, ref int Score, Random random, Vector2 headPos, List<Vector2> bodyPos);
    }
}