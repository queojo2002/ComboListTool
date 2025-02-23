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

        // Đọc từ keywordFile vào HashSet để tìm nhanh
        HashSet<string> keywords = new(File.ReadLines(keywordFile)
                                           .Select(k => k.Trim())
                                           .Where(k => !string.IsNullOrWhiteSpace(k)),
                                       StringComparer.OrdinalIgnoreCase);

        if (keywords.Count == 0)
        {
            Console.WriteLine("❌ File keyword trống!");
            return;
        }

        try
        {
            using StreamReader reader = new(inputFile);
            using StreamWriter writer = new(outputFile);

            string? line;
            int totalLines = 0, matchedLines = 0;

            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(':');
                if (parts.Length >= 3 && parts[1].Contains("@"))
                {
                    if (keywords.Any(keyword => parts[1].Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                    {
                        writer.WriteLine($"{parts[1]}:{parts[2]}");
                        matchedLines++;
                    }
                }

                totalLines++;
                if (totalLines % 1_000_000 == 0) // Báo tiến trình mỗi 1 triệu dòng
                {
                    Console.WriteLine($"📌 Đã xử lý {totalLines:N0} dòng, lọc được {matchedLines:N0} dòng...");
                }
            }

            Console.WriteLine($"✅ Hoàn thành! Tổng dòng: {totalLines:N0}, Đã lọc: {matchedLines:N0}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Lỗi khi lọc combolist: {ex.Message}");
        }
    }
}