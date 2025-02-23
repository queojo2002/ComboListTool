using System.Collections.Concurrent;

class CombolistSeparator
{
    //(Tách EMAIL:PASS & USER:PASS)
    public static void Separate(string inputFile, string emailOutputFile, string userOutputFile)
    {
        if (!File.Exists(inputFile))
        {
            Console.WriteLine("❌ File không tồn tại!");
            return;
        }

        var emailPassList = new ConcurrentBag<string>();
        var userPassList = new ConcurrentBag<string>();

        Parallel.ForEach(File.ReadLines(inputFile), line =>
        {
            var parts = line.Split(':');
            if (parts.Length == 2)
            {
                if (parts[0].Contains("@"))
                    emailPassList.Add(line);
                else
                    userPassList.Add(line);
            }
        });

        File.WriteAllLines(emailOutputFile, emailPassList);
        File.WriteAllLines(userOutputFile, userPassList);
        Console.WriteLine($"✅ Đã phân loại email:pass và user:pass.");
    }
}
