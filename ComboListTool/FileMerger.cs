using System.Collections.Concurrent;

class FileMerger
{
    // (Hợp nhất file lớn & lọc trùng)
    public static void MergeFiles(string inputFile, string outputFile)
    {
        if (!File.Exists(inputFile))
        {
            Console.WriteLine("❌ File không tồn tại!");
            return;
        }

        ConcurrentDictionary<string, byte> uniqueLines = new();

        Parallel.ForEach(File.ReadLines(inputFile), line =>
        {
            uniqueLines.TryAdd(line.Trim(), 0);
        });

        File.WriteAllLines(outputFile, uniqueLines.Keys);
        Console.WriteLine($"✅ Đã hợp nhất và lọc trùng: {outputFile}");
    }
}
