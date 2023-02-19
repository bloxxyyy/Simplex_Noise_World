using Koko_Seikail_Base_Game.Src.InternalCode;
using Koko_Seikail_Base_Game.Src.InternalCode.Generators.Heightmap;
using System.Collections.Generic;

namespace Koko_Seikail_Base_Game.Src.GameCode.Scene_Map_Code;
public class MapManager {

    private readonly Dictionary<int, Map> MAPS = new();

    // for now we just reinit maybe saves later?
    public MapManager(PropertiesManager propertiesManager) {
        var owHeight = propertiesManager.GetProperty<int>("overworld.baseHeight");
        var owWidth = propertiesManager.GetProperty<int>("overworld.baseWidth");

        // TODO fix
        MAPS.Add(0, Map.MapCreator(owHeight, owWidth, new SimplexNoiseHeightMapFirstTest(owWidth, owHeight, 0,0,0)));
    }
}
