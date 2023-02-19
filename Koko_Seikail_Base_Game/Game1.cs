using Koko_Seikail_Base_Game.Src.GameCode.Scene_Map_Code;
using Koko_Seikail_Base_Game.Src.InternalCode.Generators.Heightmap;
using Koko_Seikail_Base_Game.Src.InternalCode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

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

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        CreateTestTexture();
    }


    private int octaves = 7;
    private float persistance = 0.6f;

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

                if (c < 0.1f) {
                    colors[y * width + x] = Color.DarkBlue;
                } else if (c < 0.2f) {
                    colors[y * width + x] = Color.LightBlue;
                } else if (c < 0.25f) {
                    colors[y * width + x] = Color.Yellow;
                } else if (c < 0.5f) {
                    colors[y * width + x] = Color.Green;
                } else if (c < 0.7f) {
                    colors[y * width + x] = Color.Brown;
                } else {
                    colors[y * width + x] = Color.Gray;
                }
            }
        }

        texture.SetData(colors);
    }






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







    protected override void Update(GameTime gameTime) {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (Keyboard.GetState().IsKeyDown(Keys.Q)) {
            octaves += 1;
            Debug.WriteLine("Octaves: " + octaves);
            CreateTestTexture();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.A)) {
            octaves -= 1;
            Debug.WriteLine("Octaves: " + octaves);
            CreateTestTexture();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.W)) {
            persistance += 0.1f;
            Debug.WriteLine("Persistance: " + persistance);
            CreateTestTexture();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.S)) {
            persistance -= 0.1f;
            Debug.WriteLine("Persistance: " + persistance);
            CreateTestTexture();
        }



        
        if (Keyboard.GetState().IsKeyDown(Keys.Space)) {
            Debug.WriteLine("Space pressed");

            CreateTestTexture();
        }

            base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        base.Draw(gameTime);

        _spriteBatch.Begin();
        _spriteBatch.Draw(texture, new Vector2(0, 0), Color.White);
        _spriteBatch.End();
    }
}
