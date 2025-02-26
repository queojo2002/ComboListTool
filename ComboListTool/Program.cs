using ComboListTool;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("=== TOOL XỬ LÝ COMBOLIST ===");
            Console.WriteLine("1. Hợp nhất file lớn & lọc trùng");
            Console.WriteLine("2. Biến đổi combolist (thêm @! & xử lý số)");
            Console.WriteLine("3. Lọc combolist theo keyword");
            Console.WriteLine("4. Phân loại EMAIL:PASS và USER:PASS");
            Console.WriteLine("5. Xử lý nhiều file với FileProcessor");
            Console.WriteLine("0. Thoát");
            Console.Write("Chọn chức năng: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Nhập đường dẫn thư mục đầu vào: ");
                    string inputFolder = Console.ReadLine();
                    Console.Write("Nhập đường dẫn thư mục đầu ra: ");
                    string outputFolder = Console.ReadLine();
                    Console.Write("Nhập đường dẫn file đầu ra: ");
                    string outputFile = Console.ReadLine();
                    Console.Write("Nhập đường dẫn file keyword (hoặc bỏ trống): ");
                    string keywordsFile = Console.ReadLine();
                    Console.Write("Nhập số lượng task: ");
                    int taskCount = int.TryParse(Console.ReadLine(), out int count) ? count : 4;

                    await FileProcessor.ProcessFiles(inputFolder, outputFolder, outputFile, keywordsFile, taskCount);
                    break;
                case "2":
                    Console.Write("Nhập đường dẫn file đầu vào: ");
                    string inputModify = Console.ReadLine();
                    Console.Write("Nhập đường dẫn file đầu ra: ");
                    string outputModify = Console.ReadLine();
                    CombolistModifier.ProcessCombolist(inputModify, outputModify);
                    break;
                case "3":
                    Console.Write("Nhập đường dẫn file combolist: ");
                    string inputFilter = Console.ReadLine();
                    Console.Write("Nhập đường dẫn file đầu ra: ");
                    string outputFilter = Console.ReadLine();
                    Console.Write("Nhập đường dẫn file keyword: ");
                    string keywordFile = Console.ReadLine();
                    CombolistFilter.FilterByKeyword(inputFilter, outputFilter, keywordFile);
                    break;
                case "4":
                    Console.Write("Nhập đường dẫn file đầu vào: ");
                    string inputSeparate = Console.ReadLine();
                    Console.Write("Nhập đường dẫn file lưu EMAIL:PASS: ");
                    string emailOutput = Console.ReadLine();
                    Console.Write("Nhập đường dẫn file lưu USER:PASS: ");
                    string userOutput = Console.ReadLine();
                    CombolistSeparator.Separate(inputSeparate, emailOutput, userOutput);
                    break;
                case "5":
                    Console.Write("Nhập đường dẫn thư mục đầu vào: ");
                    string inputDir = Console.ReadLine();
                    Console.Write("Nhập đường dẫn thư mục đầu ra: ");
                    string outputDir = Console.ReadLine();
                    Console.Write("Nhập đường dẫn file đầu ra: ");
                    string mergedFile = Console.ReadLine();
                    Console.Write("Nhập đường dẫn file keyword (hoặc bỏ trống): ");
                    string keywordFilePath = Console.ReadLine();
                    Console.Write("Nhập số lượng task: ");
                    int numTasks = int.TryParse(Console.ReadLine(), out int tasks) ? tasks : 4;

                    await FileProcessor.ProcessFiles(inputDir, outputDir, mergedFile, keywordFilePath, numTasks);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ. Nhấn Enter để chọn lại...");
                    Console.ReadLine();
                    break;
            }

            Console.WriteLine("\nNhấn Enter để quay lại menu...");
            Console.ReadLine();
        }
    }
}