using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

DateTime Start = DateTime.Now;
if(Args.Count == 0){
    Console.WriteLine("Please enter a number");
    return;
}
int MAX = int.Parse(Args[0]);
if(!IsPerfectSquare(MAX)){
    Console.WriteLine($"{MAX} is not a perfect Square.\nPlease use a perfect square!");
    return;
}
struct Pattern
{
    public int[] StartingPattern { get; set; } = new int[0];
    public Pattern(int[] patt){
        StartingPattern = patt;
    } 
    public string ToString(int max)
    {
        var NO = Math.Sqrt(max);
        string pattern = string.Empty;
        for (int i = 0; i < StartingPattern.Length; i++)
        {
            pattern += (StartingPattern[i] + 1).ToString() != $"{max}" ? (StartingPattern[i] + 1).ToString() : "[]";
            if (i < StartingPattern.Length - 1)
            {
                pattern += ", ";
            }
            if(i%NO == NO-1)
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
    public PuzzleTile(int index, int max){
        OriginalIndex = index;
        CurrentIndex = index;
        IsEmpty = index == max-1;
    }
}

Random rand = new();
List<PuzzleTile> tiles = new();
for (int i = 0; i < MAX; i++)
{
    tiles.Add(new(i, MAX));
}
private bool IsAdjacent(int idx1, int idx2)
{
    var GRID = (int)Math.Sqrt(MAX);
    int r1 = idx1 / GRID, c1 = idx1 % GRID;
    int r2 = idx2 / GRID, c2 = idx2 % GRID;
    return Math.Abs(r1 - r2) + Math.Abs(c1 - c2) == 1;
}
int[] spa = new int[MAX];
for (int i = 0; i < MAX; i++)
{
    spa[i] = i;
}
Pattern Solved = new(spa);
Console.WriteLine("Let's get it");
var CSP = (Factorial(MAX)/2)-1;
while (Solveable.Count< CSP)
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
    Console.WriteLine($"{Solveable.Count}/{CSP} Patterns Found\n{P.ToString(MAX)}");
}
var dirNAME = $"{MAX}x{MAX}";
Directory.CreateDirectory(dirNAME);
var sov = Solveable.ConvertAll(entry => entry.ToString(MAX)).ToArray();
JsonSerializerOptions options = new JsonSerializerOptions
{
    WriteIndented = true
};
string txt = String.Join("\n", sov);
File.WriteAllText(Path.Combine(dirNAME,"grids.txt"), txt);
var S = Solveable.ConvertAll(entry => entry.StartingPattern).ToArray();
string Sjson = JsonSerializer.Serialize(S, options);
File.WriteAllText(Path.Combine(dirNAME,"pattern.json"), Sjson);
var End = DateTime.Now - Start;
Console.WriteLine(End);

int Factorial(int number){
    int factorial = number;
    for (int i = number-1; i > 0; i--)
    {
        factorial *= i;
    }
    return factorial;
}

bool IsPerfectSquare(long number)
{
    if (number < 0) return false;

    long root = (long)Math.Sqrt(number);
    return root * root == number || (root + 1) * (root + 1) == number;
}