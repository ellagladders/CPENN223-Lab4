// Lab 4
// Student name: Ella Gladders
// Student number: 78754249

using System;
using System.Collections.Generic;

Console.WriteLine("CPEN223 Lab 4");

//Testing: Write test cases that exercise all four methods you are to implement.
//TODO
// Dictionary<string, int> counts = GenomeAnalyzer.CountKMers("AAAA", 2);
// Console.WriteLine($"Expected: AA -> 3, Actual: AA -> {counts["AA"]} ({counts.Count} entries)");
//
// bool differ = GenomeAnalyzer.SamplesDiffer("AAAA", "TTTT", 2, 3);
// Console.WriteLine($"Expected: True, Actual: {differ}");

// CountKMers
Dictionary<string, int> counts = GenomeAnalyzer.CountKMers("AAAA", 2);
Console.WriteLine($"Expected: AA -> 3, Actual: AA -> {counts["AA"]}");

// CompareProfiles
Dictionary<string, int> changes = GenomeAnalyzer.CompareProfiles("AAAA", "TTTT", 2);
Console.WriteLine($"Expected: AA -> -3, TT -> 3, Actual: AA -> {changes["AA"]}, TT -> {changes["TT"]}");

// MostChangedKMers
List<string> mostChanged = GenomeAnalyzer.MostChangedKMers("AAAA", "TTTT", 2);
Console.WriteLine($"Expected: 2 entries containing AA and TT, Actual: {mostChanged.Count} entries, AA: {mostChanged.Contains("AA")}, TT: {mostChanged.Contains("TT")}");

// SamplesDiffer
bool differ = GenomeAnalyzer.SamplesDiffer("AAAA", "TTTT", 2, 3);
Console.WriteLine($"Expected: True, Actual: {differ}");

//end Testing code

//Do not change the program skeleton: keep the class name, method names,
//parameters, and return types exactly as given.
//Do not use LINQ, and do not use Console inside the GenomeAnalyzer methods.

public static class GenomeAnalyzer
{
    public static Dictionary<string, int> CountKMers(string sequence, int k)
    {
        if (sequence == null)
        {
            throw new ArgumentNullException();
        }

        if (k <= 0 || k > sequence.Length)
        {
            throw new ArgumentException();
        }

        foreach (char nucleotide in sequence)
        {
            if (nucleotide != 'A' && nucleotide != 'C' && nucleotide != 'G' && nucleotide != 'T')
            {
                throw new ArgumentException();
            }
        }
        // store each kmer as a key and its occureence as a value in a dictionary
        Dictionary<string, int> CountKMers = new Dictionary<string, int>();

        // move one position at a time to include overlap, stop at the last starting index where k chars still fit
        for (int i = 0; i <= sequence.Length - k; i++)
        {
            string kmers = sequence.Substring(i, k); 
                if (CountKMers.ContainsKey(kmers))
            {
                // increase count for kmers already seen
                CountKMers[kmers]++; 
            }
            else
            {
                // start count at 1 for a new kmer
                CountKMers[kmers] = 1;
            }
        }
        return CountKMers;
    }

    public static Dictionary<string, int> CompareProfiles(string reference, string sample, int k)
    {
        // store the counts of kmers in both reference and sample sequences
        Dictionary<string, int> referenceCounts = CountKMers(reference, k); 
        Dictionary<string, int> sampleCounts = CountKMers(sample, k);
        Dictionary<string, int> differences = new Dictionary<string,int>();

        // handle kmers that are present in the reference sequence
        foreach (var entry in referenceCounts)
        {
            string kmer = entry.Key;
            int referenceCount = entry.Value; 
            // use the sample count if present, otherwise default to 0
            int sampleCount = sampleCounts.ContainsKey(kmer) ? sampleCounts[kmer] : 0;
            int difference = sampleCount - referenceCount;

            // store only kmers whose count changed 
            if (difference !=0)
            {
                differences[kmer] = difference;
            }
        }

        // handle kmers present in sample
        foreach (var entry in sampleCounts)
        {
            if (!referenceCounts.ContainsKey(entry.Key))
            {
                differences[entry.Key] = entry.Value; 
            }
        }
        return differences;
    }

    public static List<string> MostChangedKMers(string reference, string sample, int k)
    {
        Dictionary<string, int> differences = CompareProfiles(reference, sample, k);
        List<string> mostChangedKMers = new List<string>();
        int maxDifference = 0;

        foreach (var entry in differences)
        {
            // compare side of the change
            int difference = Math.Abs(entry.Value);
            if (difference > maxDifference)
            {
                // record the new largest change
                maxDifference = difference;
                // replace previous with this kmer
                mostChangedKMers.Clear();
                mostChangedKMers.Add(entry.Key);
            }
            else if (difference == maxDifference)
            {
                // include all kemrs tied for largest change
                mostChangedKMers.Add(entry.Key);
            }
        }
        // if there are no diffs, the list will be empty
        return mostChangedKMers;
    }

    public static bool SamplesDiffer(string reference, string sample, int k, int threshold)
    {
        // checks for invalid input
        if (threshold <= 0)
        {
            throw new ArgumentException();
        }

        Dictionary<string, int> differences = CompareProfiles(reference, sample, k);

        foreach (var entry in differences)
        {
            // return immediately when a change meets or exceeds threshold
            if (Math.Abs(entry.Value) >= threshold)
            {
                return true;
            }
        }
        // no kmer met the threshold
        return false;
    }
}
