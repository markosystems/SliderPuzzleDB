using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

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

    public static implicit operator Pattern(int[] Arr)=> new(Arr);
}

var Patterns = JsonSerializer.Deserialize<List<int[]>>(File.ReadAllText("pattern.json"));
List<Pattern> AllP = [.. Patterns];

var sov = AllP.ConvertAll(entry => entry.ToString()).ToArray();

string txt = String.Join("\n", sov);
File.WriteAllText("grids.txt", txt);