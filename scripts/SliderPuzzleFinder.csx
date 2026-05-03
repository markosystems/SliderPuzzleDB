using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

DateTime Start = DateTime.Now;
struct Pattern
{
    public int[] StartingPattern { get; set; } = new int[0];
    public Pattern(int[] patt){
        StartingPattern = patt;
    }
    public override string ToString()
    {
        string pattern = string.Empty;
        for (int i = 0; i < StartingPattern.Length; i++)
        {
            pattern += (StartingPattern[i] + 1).ToString() != "9" ? (StartingPattern[i] + 1).ToString() : "[]";
            if (i < StartingPattern.Length - 1)
            {
                pattern += ", ";
            }
            if(i%3 == 2)
            {
                pattern += "\n";
            }
        }
        return pattern;
    }
    public bool SamePattern(int[] pattern)
    {
        if (pattern.Length != StartingPattern.Length) return false;
        for (int i = 0; i < pattern.Length; i++)
        {
            if (pattern[i] != StartingPattern[i]) return false;
        }
        return true;
    }
}
List<Pattern> Solveable = new();
class PuzzleTile
{
    public int OriginalIndex { get; set; }
    public int CurrentIndex { get; set; }
    public bool IsEmpty { get; set; }
    public PuzzleTile(int index){
        OriginalIndex = index;
        CurrentIndex = index;
        IsEmpty = index == 8;
    }
}
Random rand = new();
List<PuzzleTile> tiles = new();
for (int i = 0; i < 9; i++)
{
    tiles.Add(new(i));
}
private bool IsAdjacent(int idx1, int idx2)
{
    int r1 = idx1 / 3, c1 = idx1 % 3;
    int r2 = idx2 / 3, c2 = idx2 % 3;
    return Math.Abs(r1 - r2) + Math.Abs(c1 - c2) == 1;
}
Pattern Solved = new([0,1,2,3,4,5,6,7,8]);
Console.WriteLine("Let's get it");
while (Solveable.Count< 181439)
{
    var emptyTile = tiles.First(t => t.IsEmpty);
    var validMoves = tiles.Where(t => IsAdjacent(t.CurrentIndex, emptyTile.CurrentIndex)).ToList();
    var move = validMoves[rand.Next(validMoves.Count)];
    var temp = move.CurrentIndex;
    move.CurrentIndex = emptyTile.CurrentIndex;
    emptyTile.CurrentIndex = temp;
    var sorted = tiles.OrderBy(t => t.CurrentIndex).ToList();
    tiles.Clear();
    foreach (var tile in sorted) tiles.Add(tile);
    var patt = tiles.Select(t => t.OriginalIndex).ToArray();
    if(Solved.SamePattern(patt))
    {
        Console.WriteLine("Solved Position");
        continue;
    }
    if(Solveable.Any(F=> F.SamePattern(patt)))
    {
        continue;
    }
    var P = new Pattern(patt);
    Solveable.Add(P);
    Console.Clear();
    Console.WriteLine($"{Solveable.Count}/181,440 Patterns Found\n{P.ToString()}");
}
var sov = Solveable.ConvertAll(entry => entry.ToString()).ToArray();
JsonSerializerOptions options = new JsonSerializerOptions
{
    WriteIndented = true
};
string txt = String.Join("\n", sov);
File.WriteAllText("grids.txt", txt);
var S = Solveable.ConvertAll(entry => entry.StartingPattern).ToArray();
string Sjson = JsonSerializer.Serialize(S, options);
File.WriteAllText("pattern.json", Sjson);
var End = DateTime.Now - Start;
Console.WriteLine(End);