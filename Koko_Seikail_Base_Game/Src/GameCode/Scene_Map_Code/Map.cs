using Koko_Seikail_Base_Game.Src.InternalCode.Generators.Heightmap;

namespace Koko_Seikail_Base_Game.Src.GameCode.Scene_Map_Code;
public class Map {

    //  you can calculate the index of the element at a given row i and column j using the formula: index = i* 5 + j
    /* To get the neighbors of index 3 in a 5 by 5 1-dimensional array, you can calculate the 1D indices of its neighboring elements using the formula:
        left: index - 1
        right: index + 1
        up: index - 5
        down: index + 5
    */
    public int[] Tiles { get; private set; }

    public static Map MapCreator(int mapHeight, int mapWidth, HeightMapper heightMapObject) {
        Map map = new() {
            Tiles = new int[mapHeight * mapWidth]
        };

        var generatedHM = heightMapObject.GenerateHeightMap(7, .6f);

        for (int i = 0; i < mapHeight; i++) {
            for (int j = 0; j < mapWidth; j++) {
                int val = -1;

                if (generatedHM[i, j] < 0.1f)
                    val = 2;
                else if (generatedHM[i, j] < 0.2f)
                    val = 1;
                else if (generatedHM[i, j] < 0.5f)
                    val = 0;
                else if (generatedHM[i, j] < 0.8f)
                    val = 4;
                else
                    val = 3;

                map.Tiles[(i * mapWidth) + j] = val;
            }
        }
        
        return map;
    }
}
