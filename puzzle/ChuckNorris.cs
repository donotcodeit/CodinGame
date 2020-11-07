// https://www.codingame.com/ide/puzzle/chuck-norris

using System;
using System.Linq;
using System.Text.RegularExpressions;

class ChuckNorris
{
    static Regex SectionRegex = new Regex("(0+)|(1+)", RegexOptions.Compiled);

    static void Main(string[] args)
    {
        var message = Console.ReadLine();
        var binary = string.Join("", message.Select(c => Convert.ToString(c, 2).PadLeft(7, '0')));
        var encoded = SectionRegex.Replace(binary, m =>
            $"{(m.Groups[1].Success ? "00" : "0")} " +
            $"{new string('0', m.Length)} ").TrimEnd();

        Console.WriteLine(encoded);
    }
}