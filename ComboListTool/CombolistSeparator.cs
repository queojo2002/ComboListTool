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

        try
        {
            using StreamReader reader = new(inputFile);
            using StreamWriter emailWriter = new(emailOutputFile);
            using StreamWriter userWriter = new(userOutputFile);

            string? line;
            int totalLines = 0, emailCount = 0, userCount = 0;

            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(':');
                if (parts.Length == 2)
                {
                    if (parts[0].Contains("@"))
                    {
                        emailWriter.WriteLine(line);
                        emailCount++;
                    }
                    else
                    {
                        userWriter.WriteLine(line);
                        userCount++;
                    }
                }

                totalLines++;
                if (totalLines % 1_000_000 == 0) // Cứ mỗi 1 triệu dòng, báo tiến trình
                {
                    Console.WriteLine($"📌 Đã xử lý {totalLines:N0} dòng...");
                }
            }

            Console.WriteLine($"✅ Hoàn thành! Tổng: {totalLines:N0} dòng, Email: {emailCount:N0}, User: {userCount:N0}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Lỗi khi xử lý file: {ex.Message}");
        }
    }
}