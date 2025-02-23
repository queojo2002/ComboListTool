using System.Collections.Concurrent;
using System.Text.RegularExpressions;

class CombolistModifier
{
    //(Biến đổi combolist - thêm @! & xử lý số)
    public static void ProcessCombolist(string inputFile, string outputFile)
    {
        if (!File.Exists(inputFile))
        {
            Console.WriteLine("❌ File không tồn tại!");
            return;
        }

        var modifiedLines = new ConcurrentBag<string>();

        Parallel.ForEach(File.ReadLines(inputFile), line =>
        {
            var parts = line.Split(':');
            if (parts.Length == 2)
            {
                string email = parts[0];
                string password = parts[1];

                if (password.Length > 0)
                    password = char.ToUpper(password[0]) + password.Substring(1);

                modifiedLines.Add($"{email}:{password}");
                modifiedLines.Add($"{email}:{password}!");
                modifiedLines.Add($"{email}:{password}@");
            }
            else if (Regex.IsMatch(line, @"^\d+$")) // Nếu toàn số, thêm 'a'
            {
                modifiedLines.Add(line + "a");
            }
            else
            {
                modifiedLines.Add(line);
            }
        });

        File.WriteAllLines(outputFile, modifiedLines);
        Console.WriteLine($"✅ Đã xử lý combolist: {outputFile}");
    }
}
