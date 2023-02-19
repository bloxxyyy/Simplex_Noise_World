using SimplexNoise;
using System;

namespace Koko_Seikail_Base_Game.Src.InternalCode.Generators.Heightmap;
public class SimplexNoiseHeightMap : HeightMapper
{

    private float _Scale = .0077f;
    private int width = 500, height = 500;

    public float[,] GenerateHeightMap(int octaves, float persistance)
    {
        Random random = new Random();
        int seed = random.Next();
        Noise.Seed = seed;
        var luminance = new float[height, width];

        for (var i = 0; i < height; ++i)
        {
            for (var j = 0; j < width; ++j)
            {
                luminance[i, j] = sumOcatave(octaves, i, j, persistance);
            }
        }

        return luminance;
    }

    float sumOcatave(int num_iterations, int x, int y, float persistence)
    {
        var maxAmp = 0f;
        var amp = 1f;
        var freq = _Scale;
        var noise = 0f;

        for (var i = 0; i < num_iterations; ++i)
        {
            noise += Noise.CalcPixel2D(x, y, freq) * amp;
            maxAmp += amp;
            amp *= persistence;
            freq *= 2;
        }

        noise /= maxAmp;
        return noise;
    }
}
