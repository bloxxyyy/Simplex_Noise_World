using System;

namespace Koko_Seikail_Base_Game.Src.InternalCode;
public static class MathK {
    public static float InverseLerp(float minValue, float maxValue, float value, float min, float max) {
        var t = (value - minValue) / (maxValue - minValue);
        return min + (t * (max - min));
    }
}
