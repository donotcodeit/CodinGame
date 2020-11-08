using System;
using System.Collections.Generic;

class ChildsPlay
{
    enum Direction { Up, Right, Down, Left };
    struct Position
    {
        public int X;
        public int Y;
        public Direction Dir;
    }

    const char ObstacleChar = '#';
    const char RobotChar = 'O';

    static void Main(string[] args)
    {
        var sizeInput = Console.ReadLine().Split(' ');
        var w = int.Parse(sizeInput[0]);
        var h = int.Parse(sizeInput[1]);
        var initCount = ulong.Parse(Console.ReadLine());
        var map = ReadObstacleMap(w, h, out var initPos);

        var stops = new Dictionary<Position, (ulong CountToNext, Position Next)>();
        var pos = initPos;
        var count = initCount;

        while (count > 0)
        {
            var prevPos = pos;
            ulong moveCount;
            (pos, moveCount) = MoveToObstacle(w, h, map, pos, count);
            count -= moveCount;

            if (count > 0)
            {
                if (!stops.ContainsKey(prevPos))
                {
                    stops[prevPos] = (moveCount, pos);
                    continue;
                }

                (pos, count) = FastForward(w, h, map, initCount, initPos, prevPos, stops);
            }
        }

        Console.WriteLine($"{pos.X} {pos.Y}");
    }

    static (Position, ulong) MoveToObstacle(int w, int h, bool[] map, Position pos, ulong count)
    {
        Direction RotateDirection(Direction dir) => dir switch
        {
            Direction.Up => Direction.Right,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up
        };

        int GetPosShift(Direction dir) => dir switch
        {
            Direction.Up => -w,
            Direction.Right => 1,
            Direction.Down => w,
            Direction.Left => -1
        };

        var posDir = pos.Dir;
        var posShift = GetPosShift(pos.Dir);
        var posOffset = pos.Y * w + pos.X;
        var nextOffset = posOffset + posShift;
        ulong moveCount = 0;

        while (moveCount < count)
        {
            posOffset = nextOffset;
            nextOffset += posShift;
            moveCount++;

            var hasObstacle = map[nextOffset];
            if (hasObstacle)
            {
                do
                {
                    posDir = RotateDirection(posDir);
                    posShift = GetPosShift(posDir);
                    nextOffset = posOffset + posShift;
                    hasObstacle = map[nextOffset];
                } while (hasObstacle);
                break;
            }
        };

        var nextPos = new Position { X = posOffset % w, Y = posOffset / w, Dir = posDir };
        return (nextPos, moveCount);
    }

    static (Position, ulong) FastForward(
        int w,
        int h,
        bool[] map,
        ulong initCount,
        Position initPos,
        Position loopPos,
        Dictionary<Position, (ulong CountToNext, Position Next)> stops)
    {
        ulong GetDistance(Position from, Position to)
        {
            ulong distance = 0;
            var trace = stops[from];
            do
            {
                distance += trace.CountToNext;
                trace = stops[trace.Next];
            } while (!trace.Next.Equals(to));

            return distance;
        }

        var curPos = stops[loopPos].Next;
        var beforeLoopCount = loopPos.Equals(initPos) ? 0 : GetDistance(initPos, curPos);
        var loopCount = GetDistance(loopPos, curPos);
        var restCount = (initCount - beforeLoopCount) % loopCount;

        curPos = loopPos;
        var trace = stops[curPos];
        while (trace.CountToNext <= restCount)
        {
            restCount -= trace.CountToNext;
            curPos = trace.Next;
            trace = stops[trace.Next];
        }

        return (curPos, restCount);
    }

    static bool[] ReadObstacleMap(int w, int h, out Position pos)
    {
        var map = new bool[w * h];
        pos = new Position { X = 0, Y = 0, Dir = Direction.Up };

        for (var y = 0; y < h; y++)
        {
            var mapLine = Console.ReadLine();
            var offset = w * y;
            foreach (var element in mapLine)
            {
                if (element == RobotChar)
                {
                    pos.X = offset % w;
                    pos.Y = y;
                }
                map[offset++] = element == ObstacleChar;
            }
        }

        return map;
    }
}