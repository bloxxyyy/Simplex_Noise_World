using Koko_Seikail_Base_Game.Src.GameCode.Scene_Map_Code;
using Koko_Seikail_Base_Game.Src.InternalCode.Generators.Heightmap;
using Koko_Seikail_Base_Game.Src.InternalCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using Koko_Seikail_Base_Game.Src.InternalCode.Generators.RiverDistribution;
using System.Collections.Generic;
using DelaunatorSharp;
using MonoGame.Extended;

namespace Koko_Seikail_Base_Game;
public class Game1 : Game {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    Texture2D texture;
    int width = 500;
    int height = 500;

    public Game1() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        var properties = new PropertiesManager("game"); // testing
        var mapManager = new MapManager(properties);     // testing
    }

    PoissonDiscSample pds;
    Delaunator delaunator;

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        pds = new(700, 360, 14);

        var list = new List<IPoint>();
        for (int i = 0; i < pds.Grid.Length; i++) {
            if (pds.Grid[i].X > 1 || pds.Grid[i].Y > 1)
                list.Add(new DelaunatorSharp.Point(pds.Grid[i].X, pds.Grid[i].Y));
        }
        
        delaunator = new Delaunator(list.ToArray());
    }
    

    private int octaves = 7;
    private float persistance = 0.6f;

    /*
    private void CreateTestTexture() {
        var gradient = testGradient();

        texture = new Texture2D(GraphicsDevice, height, width);
        Color[] colors = new Color[height * width];
        var hmObject = new SimplexNoiseHeightMap();
        var heightmap = hmObject.GenerateHeightMap(octaves, persistance);
        
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float value = heightmap[x, y];
                float c = MathK.InverseLerp(0, 255, value, 0, 1);
                float g = MathK.InverseLerp(0, 2, gradient[y, x], 0, 1);
                c = c - g;
                c = MathK.InverseLerp(0, 0.8f, c, 0, 1);
                
                if (c < 0.1f) {
                    colors[y * width + x] = Color.DarkBlue;
                } else if (c < 0.2f) {
                    colors[y * width + x] = Color.LightBlue;
                } else if (c < 0.25f) {
                    colors[y * width + x] = Color.Yellow;
                } else if (c < 0.6f) {
                    colors[y * width + x] = Color.Green;
                } else if (c < 0.75f) {
                    colors[y * width + x] = Color.Brown;
                } else if (c < 0.9f) {
                    colors[y * width + x] = Color.Gray;
                } else {
                    colors[y * width + x] = Color.WhiteSmoke;
                }
            }
        }

        texture.SetData(colors);
    }
    */



    /*

    private float[,] testGradient() {
        int centerX = width / 2;
        int centerY = height / 2;
        int maxDistance = Math.Max(centerX, centerY);
        float colorStart = 0f;
        float colorEnd = 2f;
        float power = 2f; // Adjust this value to control the gradient shape

        var colorValues = new float[height, width];

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float distanceToCenter = MathF.Sqrt(MathF.Pow(x - centerX, 2) + MathF.Pow(y - centerY, 2));
                float normalizedDistance = distanceToCenter / maxDistance;
                normalizedDistance = MathF.Pow(normalizedDistance, power);
                colorValues[y, x] = colorStart + normalizedDistance * (colorEnd - colorStart);
            }
        }

        return colorValues;
    }

    */





    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (Keyboard.GetState().IsKeyDown(Keys.Space)) {
            Debug.WriteLine("Space pressed");
            pds = new(700, 360, 14);
            var list = new List<IPoint>();
            for (int i = 0; i < pds.Grid.Length; i++) {
                if (pds.Grid[i].X > 1 || pds.Grid[i].Y > 1)
                    list.Add(new DelaunatorSharp.Point(pds.Grid[i].X, pds.Grid[i].Y));
            }

            delaunator = new Delaunator(list.ToArray());
            //CreateTestTexture();
        }

            base.Update(gameTime);
    }

    private Texture2D garbage() {
        texture = new Texture2D(GraphicsDevice, 5, 5);
        Color[] colors = new Color[5 * 5];
        for (int y = 0; y < 5; y++) {
            for (int x = 0; x < 5; x++) {
                colors[y * 5 + x] = Color.Red;
            }
        }
        texture.SetData(colors);
        return texture;
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        base.Draw(gameTime);

        _spriteBatch.Begin();

        delaunator.ForEachTriangleEdge(edge =>
        {
            var vec1 = new Vector2((float)edge.P.X, (float)edge.P.Y);
            var vec2 = new Vector2((float)edge.Q.X, (float)edge.Q.Y);

            _spriteBatch.DrawLine(vec1, vec2, Color.White, 1);

        });

        /*
        foreach (var item in pds.Grid) {
            if (item.X > 1 && item.Y > 1) {
                texture = garbage();
                _spriteBatch.Draw(texture, new Vector2(item.X, item.Y), Color.Red);
            }
        }
        */

        _spriteBatch.End();
    }
}
