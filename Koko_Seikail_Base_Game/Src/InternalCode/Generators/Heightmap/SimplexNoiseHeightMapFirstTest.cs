using SimplexNoise;

namespace Koko_Seikail_Base_Game.Src.InternalCode.Generators.Heightmap;
public class SimplexNoiseHeightMapFirstTest : HeightMapper
{

    private int _Width;
    private int _Height;
    private float _Scale = .007f;

    private int _Octaves = 8;
    private float _Lacunarity = 2, _Persistance = 0.5f;

    public SimplexNoiseHeightMapFirstTest(int width, int height, int octaves, float lacunarity, float persistence)
    {
        _Height = height;
        _Width = width;
        _Octaves = octaves;
        _Lacunarity = lacunarity;
        _Persistance = persistence;
    }


    public float[,] GenerateHeightMap(int octaves, float lacunarity, float persistence)
    {
        var _Heightmap = new float[_Width, _Height];

        float amplitude = 1;
        float frequency = 1;
        float totalAmplitude = 0;

        // Generate each octave of the noise
        for (int octave = 0; octave < octaves; octave++)
        {
            float[,] octaveMap = Noise.Calc2D(_Width, _Height, frequency * _Scale);

            // Add the octave map to the heightmap with the appropriate amplitude
            for (int y = 0; y < _Height; y++)
            {
                for (int x = 0; x < _Width; x++)
                {
                    _Heightmap[x, y] += octaveMap[x, y] * amplitude;
                }
            }

            // Update the amplitude and frequency for the next octave
            totalAmplitude += amplitude;
            amplitude *= persistence;
            frequency *= lacunarity;
        }

        // Normalize the values in the heightmap
        float range = totalAmplitude;
        for (int y = 0; y < _Height; y++)
        {
            for (int x = 0; x < _Width; x++)
            {
                _Heightmap[x, y] /= range;
            }
        }

        return _Heightmap;
    }

    private float[,] Normalize(float[,] hmap)
    {
        // Find the minimum and maximum values in the heightmap
        float min = float.MaxValue;
        float max = float.MinValue;
        for (int y = 0; y < _Height; y++)
        {
            for (int x = 0; x < _Width; x++)
            {
                float value = hmap[x, y];
                if (value < min)
                {
                    min = value;
                }
                if (value > max)
                {
                    max = value;
                }
            }
        }

        // Normalize the values in the heightmap to a range between 0 and 1
        float range = max - min;
        for (int y = 0; y < _Height; y++)
        {
            for (int x = 0; x < _Width; x++)
            {
                hmap[x, y] = (hmap[x, y] - min) / range;
            }
        }

        return hmap;
    }

    public float[,] GenerateHeightMap(int octaves, float persistance)
    {
        var hmap = GenerateHeightMap(_Octaves, _Lacunarity, _Persistance);
        hmap = Normalize(hmap);
        return hmap;
    }
}
