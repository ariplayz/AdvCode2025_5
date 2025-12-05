using System;
using System.Collections.Generic;
using System.Linq;

class CafeteriaPuzzle
{
    static void Main()
    {
        string input = System.IO.File.ReadAllText("input.txt");

        int part1Count = SolvePart1(input);
        Console.WriteLine($"Part 1 - Fresh ingredient IDs from list: {part1Count}");
        
        long part2Count = SolvePart2(input);
        Console.WriteLine($"Part 2 - Total fresh ingredient IDs in ranges: {part2Count}");
    }

    static int SolvePart1(string input)
    {
        var (ranges, ingredientIds) = ParseInput(input);
        
        int freshCount = 0;
        
        foreach (var id in ingredientIds)
        {
            bool isFresh = ranges.Any(range => id >= range.min && id <= range.max);
            if (isFresh)
            {
                freshCount++;
            }
        }
        
        return freshCount;
    }

    static long SolvePart2(string input)
    {
        var (ranges, _) = ParseInput(input);
        
        var mergedRanges = MergeRanges(ranges);

        long totalCount = 0;
        foreach (var range in mergedRanges)
        {
            totalCount += range.max - range.min + 1;
        }
        
        return totalCount;
    }

    static (List<(long min, long max)> ranges, List<long> ingredientIds) ParseInput(string input)
    {
        var lines = input.Split('\n')
                         .Select(l => l.Trim())
                         .ToList();
        
        var ranges = new List<(long min, long max)>();
        var ingredientIds = new List<long>();
        
        bool parsingRanges = true;
        
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                parsingRanges = false;
                continue;
            }
            
            if (parsingRanges)
            {
                int hyphenIndex = line.IndexOf('-');
                if (hyphenIndex > 0)
                {
                    long min = long.Parse(line.Substring(0, hyphenIndex));
                    long max = long.Parse(line.Substring(hyphenIndex + 1));
                    ranges.Add((min, max));
                }
            }
            else
            {
                if (long.TryParse(line, out long id))
                {
                    ingredientIds.Add(id);
                }
            }
        }
        
        return (ranges, ingredientIds);
    }

    static List<(long min, long max)> MergeRanges(List<(long min, long max)> ranges)
    {
        if (ranges.Count == 0) return new List<(long min, long max)>();
        
        var sorted = ranges.OrderBy(r => r.min).ToList();
        
        var merged = new List<(long min, long max)>();
        var current = sorted[0];
        
        for (int i = 1; i < sorted.Count; i++)
        {
            var next = sorted[i];
            
            if (next.min <= current.max + 1)
            {
                current = (current.min, Math.Max(current.max, next.max));
            }
            else
            {
                merged.Add(current);
                current = next;
            }
        }
        
        merged.Add(current);
        
        return merged;
    }
}