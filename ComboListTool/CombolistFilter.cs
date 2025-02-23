using System.Collections.Concurrent;

class CombolistFilter
{
    //(Lọc combolist theo keyword)
    public static void FilterByKeyword(string inputFile, string outputFile, string keywordFile)
    {
        if (!File.Exists(inputFile))
        {
            Console.WriteLine("❌ File combolist không tồn tại!");
            return;
        }

        if (!File.Exists(keywordFile))
        {
            Console.WriteLine("❌ File keyword không tồn tại!");
            return;
        }

        var keywords = File.ReadLines(keywordFile)
                           .Select(k => k.Trim())
                           .Where(k => !string.IsNullOrWhiteSpace(k))
                           .ToHashSet(); // Dùng HashSet để tìm nhanh

        if (keywords.Count == 0)
        {
            Console.WriteLine("❌ File keyword trống!");
            return;
        }

        var filteredLines = new ConcurrentBag<string>();

        Parallel.ForEach(File.ReadLines(inputFile), line =>
        {
            var parts = line.Split(':');
            if (parts.Length >= 3 && parts[1].Contains("@"))
            {
                foreach (var keyword in keywords)
                {
                    if (parts[1].Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    {
                        filteredLines.Add($"{parts[1]}:{parts[2]}");
                        break; // Chỉ cần tìm thấy 1 keyword là đủ
                    }
                }
            }
        });

        File.WriteAllLines(outputFile, filteredLines);
        Console.WriteLine($"✅ Đã lọc combolist theo keyword. Kết quả lưu tại: {outputFile}");
    }
}
