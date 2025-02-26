namespace ComboListTool
{
    public class FileProcessor
    {
        public static async Task ProcessFiles(string inputDirectory, string outputDirectory, string mergedOutputFile, string keywordsFile, int taskCount)
        {
            Directory.CreateDirectory(outputDirectory);

            try
            {
                HashSet<string> keywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (File.Exists(keywordsFile))
                {
                    var keywordLines = File.ReadLines(keywordsFile).Where(line => !string.IsNullOrWhiteSpace(line));
                    keywords = new HashSet<string>(keywordLines, StringComparer.OrdinalIgnoreCase);
                }

                var files = Directory.EnumerateFiles(inputDirectory, "*.txt").ToArray();
                var fileGroups = files.Select((file, index) => new { file, group = index % taskCount })
                                      .GroupBy(x => x.group)
                                      .Select(g => g.Select(x => x.file).ToList())
                                      .ToList();

                var tasks = fileGroups.Select((groupFiles, index) =>
                    Task.Run(() => ProcessFileGroup(groupFiles, outputDirectory, keywords, index))).ToList();

                await Task.WhenAll(tasks);
                await MergeFiles(outputDirectory, mergedOutputFile);

                Console.WriteLine($"Xử lý toàn bộ file hoàn tất và lưu kết quả vào: {mergedOutputFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
            }
        }

        private static void ProcessFileGroup(List<string> files, string outputDirectory, HashSet<string> keywords, int groupIndex)
        {
            var results = new List<string>();

            foreach (var file in files)
            {
                try
                {
                    foreach (var line in File.ReadLines(file))
                    {
                        var parts = line.Replace("://", "").Split(':');
                        if (parts.Length >= 3)
                        {
                            string link = parts[0].Trim();
                            string user = parts[1].Trim();
                            string pass = parts[2].Trim();

                            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass))
                            {
                                string userPass = $"{user}:{pass}";

                                if (keywords.Count == 0 || keywords.Any(keyword => link.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                                {
                                    results.Add(userPass);
                                }
                            }
                        }
                    }

                    Console.WriteLine($"Đã xử lý file: {file}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi xử lý file {file}: {ex.Message}");
                }
            }

            string tempFile = Path.Combine(outputDirectory, $"TempResult_Group{groupIndex}_{Guid.NewGuid()}.txt");
            try
            {
                File.WriteAllLines(tempFile, results.Distinct(StringComparer.OrdinalIgnoreCase));
                Console.WriteLine($"Kết quả nhóm {groupIndex} đã được ghi vào: {tempFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi ghi file {tempFile}: {ex.Message}");
            }
        }

        private static async Task MergeFiles(string outputDirectory, string mergedOutputFile)
        {
            try
            {
                var seenLines = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                using (var writer = new StreamWriter(mergedOutputFile, append: false))
                {
                    foreach (var file in Directory.EnumerateFiles(outputDirectory, "TempResult_Group*.txt"))
                    {
                        try
                        {
                            using (var reader = new StreamReader(file))
                            {
                                string line;
                                while ((line = await reader.ReadLineAsync()) != null)
                                {
                                    if (seenLines.Add(line))
                                    {
                                        await writer.WriteLineAsync(line);
                                    }
                                }
                            }
                            File.Delete(file);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Lỗi xử lý file trung gian {file}: {ex.Message}");
                        }
                    }
                }

                Console.WriteLine($"Kết quả đã được hợp nhất vào file: {mergedOutputFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi hợp nhất file: {ex.Message}");
            }
        }
    }
}
