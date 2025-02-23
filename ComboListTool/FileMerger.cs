class FileMerger
{
    public static void MergeFilesInDirectory(string inputDirectory, string outputFile)
    {
        if (!Directory.Exists(inputDirectory))
        {
            Console.WriteLine("❌ Thư mục không tồn tại!");
            return;
        }

        List<string> allLines = new();

        try
        {
            foreach (string file in Directory.GetFiles(inputDirectory, "*.txt"))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(file, System.Text.Encoding.UTF8, true, 4096))
                    {
                        while (!reader.EndOfStream)
                        {
                            string? line = reader.ReadLine();
                            if (!string.IsNullOrWhiteSpace(line))
                                allLines.Add(line);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Lỗi khi đọc file {file}: {ex.Message}");
                }
            }

            Console.WriteLine($"✅ Đã gộp xong {allLines.Count} dòng, bắt đầu lọc trùng...");

            HashSet<string> uniqueLines = new(allLines);
            File.WriteAllLines(outputFile, uniqueLines);

            Console.WriteLine($"✅ Đã hợp nhất và lọc trùng thành công! Output: {outputFile}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Lỗi trong quá trình xử lý: {ex.Message}");
        }
    }
}