using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Koko_Seikail_Base_Game.Src.InternalCode.Generators.RiverDistribution;
public class PoissonDiscSampleFirstTest {

    public List<Vector2> gen(int width, int height) {
        var r = 20; // radius pixels
        var k = 30; // number of tries
        Vector2[] grid = new Vector2[width * height];
        var w = r / Math.Sqrt(2);
        List<Vector2> activeList = new();

        int cols = (int)Math.Floor(width / w);
        int rows = (int)Math.Floor(height / w);

        for (var index = 0; index < cols*rows; index++)
            grid[index] = new Vector2(-1);
        
        var rand = new Random();
        var rWidth = rand.Next(width);
        var rheight = rand.Next(height);
        var i = (int)Math.Floor(rWidth / w);
        var j = (int)Math.Floor(rheight / w);
        var vec = new Vector2(rWidth, rheight);
        grid[i + j * cols] = vec;
        activeList.Add(vec);

        for (int h = 0; h < 30; h++) {

           // while (activeList.Count > 0) {
                var randIndex = rand.Next(activeList.Count);
                var pos = activeList[randIndex];
                var found = false;

                for (int n = 0; n < k; n++) {
                    var a = (2 * Math.PI) * rand.NextDouble();
                    var offsetX = (float)(r * Math.Cos(a));
                    var offsetY = (float)(r * Math.Sin(a));
                    var offsetVec = new Vector2(offsetX, offsetY);
                    var magnitute = rand.Next(r, 2 * r);
                    offsetVec = Vector2.Normalize(offsetVec) * magnitute;
                    var sample = offsetVec + pos;
                    if (sample.X < 0 || sample.Y < 0 || sample.X >= width || sample.Y >= height)
                        continue;
                    var col = (int)Math.Floor(sample.X / w);
                    var row = (int)Math.Floor(sample.Y / w);

                    var v = grid[col + row * cols];

                    if (v.X != -1 && v.Y != -1) {
                        continue;
                    }

                    if (col < 0 || row < 0 || col >= cols || row >= rows)
                        continue;

                    var ok = true;
                    for (var i1 = -1; i1 <= 1; i1++) {
                        for (var j1 = -1; j1 <= 1; j1++) {
                            var index = (col + i1) + (row + j1) * cols;
                            if (index < 0 || index >= cols * rows)
                                continue;
                            var neighbor = grid[index];
                            if (neighbor.X == -1)
                                continue;
                            var d = Vector2.Distance(sample, neighbor);
                            if (d < r)
                                ok = false;
                        }
                    }

                    if (ok) {
                        found = true;
                        activeList.Add(sample);
                        grid[(col * cols) + row] = sample;
                        break;
                    }
                }

                if (!found) {
                    activeList.RemoveAt(randIndex);
                }
            //}
        }
        return activeList;
    }

}
