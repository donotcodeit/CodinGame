using System;
using System.Collections.Generic;
using System.IO;

class MimeTypes
{
    const string UnknownMimeType = "UNKNOWN";

    static void Main(string[] args)
    {
        var mapCount = int.Parse(Console.ReadLine());
        var queryCount = int.Parse(Console.ReadLine());

        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        while (mapCount-- > 0)
        {
            var entry = Console.ReadLine().Split(' ');
            map.Add($".{entry[0]}", entry[1]);
        }

        while (queryCount-- > 0)
        {
            var query = Console.ReadLine();
            var ext = Path.GetExtension(query);
            Console.WriteLine(map.TryGetValue(ext, out var mimeType) ? mimeType : UnknownMimeType);
        }
    }
}