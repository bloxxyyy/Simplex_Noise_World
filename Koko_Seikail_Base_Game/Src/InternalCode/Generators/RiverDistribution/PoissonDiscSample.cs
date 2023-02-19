using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Koko_Seikail_Base_Game.Src.InternalCode.Generators.RiverDistribution;
public class PoissonDiscSample {

    private int _DiameterOfACircle = 7;
    private int _Tries = 30;
    private int _MapWidth;
    private int _MapHeight;
    private Queue<Vector2> _ActivePoints = new();

    private int _Columns = 0;
    private int _Rows = 0;
    public Vector2[] Grid { get; private set; }

    private Random _Random = new();

    public PoissonDiscSample(int mapWidth, int mapHeight, int diameterOfACircle = 7, int tries = 30) {
        _MapWidth = mapWidth;
        _MapHeight = mapHeight;
        _DiameterOfACircle = diameterOfACircle;
        _Tries = tries;

        InitializeCheckTable();
        GetStartingPoint();
        Run();
    }

    private void InitializeCheckTable() {
        _Columns = GetGridPosition(_MapWidth);
        _Rows = GetGridPosition(_MapHeight);
        Grid = new Vector2[(_Columns + 1) * (_Rows + 1)];
        for (int i = 0; i < Grid.Length; i++)  Grid[i] = new Vector2(-1, -1);
    }

    private void GetStartingPoint() {
        var randomXPosition = _Random.Next(0, _MapWidth);
        var randomYPosition = _Random.Next(0, _MapHeight);
        var position = new Vector2(randomXPosition, randomYPosition);
        _ActivePoints.Enqueue(position);

        var column = GetGridPosition(randomXPosition);
        var row = GetGridPosition(randomYPosition);
        Grid[column + (row * _Columns)] = position;
    }
    
    private int GetGridPosition(int pos) {
        var halfACircle = _DiameterOfACircle / Math.Sqrt(2);
        return (int)Math.Floor(pos / halfACircle);
    }

    private void Run() {
        while (_ActivePoints.Count > 0) {

            var currentPoint = _ActivePoints.Peek();
            var newPointMade = TryMakeNewPoint(currentPoint);    
            if (!newPointMade) _ActivePoints.Dequeue();
        }
    }

    private bool TryMakeNewPoint(Vector2 currentPoint) {
        var found = false;
        for (int i = 0; i < _Tries; i++) {
            var offset = GetOffsetFromCurrentPoint(currentPoint);
            var sample = GetNextPoint(offset, currentPoint);
            found = CheckIfPointIsValid(sample);
            if (found) {
                _ActivePoints.Enqueue(sample);
                break;
            }
        }
        return found;
    }

    private bool CheckIfPointIsValid(Vector2 p) {
        if (p.X < 0 || p.Y < 0 || p.X >= _MapWidth || p.Y >= _MapHeight) return false;
        var column = GetGridPosition((int)p.X);
        var row = GetGridPosition((int)p.Y);
        
        var startColumn = Math.Max(0, column - 2);
        var endColumn = Math.Min(column + 2, _Columns);
        
        var startRow = Math.Max(0, row - 2);
        var endRow = Math.Min(row + 2, _Rows);
        
        for (int x = startColumn; x <= endColumn; x++) {
            for (int y = startRow; y <= endRow; y++) {
                var checkPoint = Grid[x + (y * _Columns)];
                if (checkPoint.X == -1)  continue;
                var distance = Vector2.Distance(p, checkPoint);
                if (distance < _DiameterOfACircle) return false;
            }
        }
        Grid[column + (row * _Columns)] = p;
        return true;
    }

    private Vector2 GetOffsetFromCurrentPoint(Vector2 point) {
        var value = (2 * Math.PI) * _Random.NextDouble();
        var offsetX = (float)(_DiameterOfACircle * Math.Cos(value));
        var offsetY = (float)(_DiameterOfACircle * Math.Sin(value));
        return new Vector2(offsetX, offsetY);
    }

    private Vector2 GetNextPoint(Vector2 offset, Vector2 currentPoint) {
        var magnitute = _Random.Next(_DiameterOfACircle, 2 * _DiameterOfACircle);
        offset = Vector2.Normalize(offset) * magnitute;
        return offset + currentPoint;
    }
}
