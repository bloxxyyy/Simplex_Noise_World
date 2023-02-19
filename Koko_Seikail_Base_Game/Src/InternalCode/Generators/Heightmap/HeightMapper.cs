namespace Koko_Seikail_Base_Game.Src.InternalCode.Generators.Heightmap;
public interface HeightMapper
{
    float[,] GenerateHeightMap(int octaves, float persistance);
}
