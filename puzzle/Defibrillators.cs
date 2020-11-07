// https://www.codingame.com/ide/puzzle/defibrillators

using System;
using System.Globalization;

class Defibrillators
{
    const double EarthRadius = 6371;

    static void Main(string[] args)
    {
        var coordinateFormat = new NumberFormatInfo { NumberDecimalSeparator = "," };
        var posLon = double.Parse(Console.ReadLine(), coordinateFormat);
        var posLat = double.Parse(Console.ReadLine(), coordinateFormat);
        var defibrillatorCount = int.Parse(Console.ReadLine());

        string closestName = null;
        double minDistance = double.MaxValue;

        while (defibrillatorCount-- > 0)
        {
            var info = Console.ReadLine().Split(';');
            var lon = double.Parse(info[4], coordinateFormat);
            var lat = double.Parse(info[5], coordinateFormat);

            var x = (lon - posLon) * Math.Cos((lat * posLat) / 2);
            var y = lat - posLat;
            var distance = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) * EarthRadius;

            if (distance < minDistance)
            {
                closestName = info[1];
                minDistance = distance;
            }
        }

        Console.WriteLine(closestName);
    }
}