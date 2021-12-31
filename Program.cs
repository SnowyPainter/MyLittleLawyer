using Harbor.DB;
using Microsoft.Data.Sqlite;

namespace MyLittleLawyer
{
    public class Config
    {
        public int Eng = 0;
        public bool Showlog = true;
    }
    public class Settings
    {
        public Dictionary<string, List<string>> KeyValues = new Dictionary<string, List<string>>();
        public Settings(string[] lines)
        {
            foreach (var l in lines)
            {
                var splited = l.Split('=');
                var list = new List<string>();
                foreach (var e in splited[1].Substring(1, splited[1].Length - 2).Split(','))
                    list.Add(e.Trim());
                KeyValues[splited[0]] = list;
            }
        }
    }
    public static class SettingsExtension
    {
        public static string? GetChecked(this List<string> values)
        {
            foreach (var item in values)
            {
                if (item[0] == 'v')
                    return item.Substring(1);
            }
            return null;
        }
    }
    public class Accident
    {
        public string? Situation { get; set; } = "";
        public string? Negative { get; set; } = "";
        public string? UnconsiousReact { get; set; } = "";
        public string? Positive { get; set; } = "";
        public string? WhatToDoNext { get; set; } = "";
        public Accident() { }
        public Accident(string situation, string negative, string reaction, string positive, string whattodo)
        {
            Situation = situation;
            Negative = negative;
            UnconsiousReact = reaction;
            Positive = positive;
            WhatToDoNext = whattodo;
        }
    }
    public class Evidence
    {
        public string? Thinks { get; set; } = "";
        public string? WhatToDoNext { get; set; } = "";
        public Evidence() { }
        public Evidence(string wtdn, string thinks)
        {
            WhatToDoNext = wtdn;
            Thinks = thinks;
        }
    }
    public class Program
    {
        /*
         유발 상황, 기소인의 생각, 반응, 변호사의 생각, 실행 계획

        1. 자동사고 인식
        2. 검사와 판사, 배심원의 부정적인 생각
        3. 나의 반응
        4. 변호사의 긍정적인 생각과 실행 가능함을 입증하는 증거
        5. 긍정적 생각에 따른 새로운 실행 계획
         */
        private static int welcomes = 10;
        private static string[] messages =
        {
            "개인 변호사에게 자문하세요.",
            "Consult Your Personal Lawyer, Here.",
            "다른 이들보다 더 많은 시간을 할애한다고 좌절하지마세요.",
            "Don't be anxiety yourself whether you allocate your time more than others.",
            "자신에게 너무 가혹한 결론을 내리지 마세요.",
            "Do not be too harsh on yourself.",
            "다른 사람에게 책임을 전가하지 마세요.",
            "Do not pass your responsibility to other.",
            "자신의 감정과 실제로 해야할 일을 꼭 분리하세요!",
            "Must Seperate your emotion and really what to do.",
            "settings.txt가 없습니다.",
            "There's no settings.txt",
            "https://github.com/SnowyPainter/MyLittleLawyer 에 접속하여 설명을 읽어주세요.",
            "Go to https://github.com/SnowyPainter/MyLittleLawyer and read description",
            "핵심 요소인 Folder Key가 없습니다.",
            "There's no Folder key as important factor in settings.txt",
            "Folder에 해당하는 파일이 없습니다. 따라서 생성하겠습니다.",
            "There's no file written at Folder key. Therefore It'll be created."
        };
        private static DB db = new DB();
        public static void Main(string[] opt)
        {
            var config = configure(opt);
            printWelcomeMsgs(config);
            if (!File.Exists("settings.txt"))
            {
                Console.WriteLine(messages[10 + config.Eng]);
                Console.WriteLine(messages[12 + config.Eng]);
                return;
            }
            var settings = new Settings(File.ReadAllLines("settings.txt"));
            if (!settings.KeyValues.ContainsKey("Folder"))
            {
                Console.WriteLine(messages[14 + config.Eng]);
                Console.WriteLine(messages[12 + config.Eng]);
                return;
            }

            var folder = settings.KeyValues["Folder"].GetChecked();
            if (folder == null)
            {
                Console.WriteLine(messages[14 + config.Eng]);
                return;
            }
            else if (!File.Exists(folder))
            {
                File.Create(folder);
                Console.WriteLine(messages[16 + config.Eng]);
                return;
            }
            if (settings.KeyValues.ContainsKey("Showlog"))
            {
                var showlog = settings.KeyValues["Showlog"].GetChecked();
                if (showlog != null && showlog == "False")
                    config.Showlog = false;
            }

            DB.ConnectionString = $@"Data Source={folder};";
            initDB();

            for (; ; )
            {
                Console.WriteLine("1.\tAdd Accident");
                Console.WriteLine("2.\tAdd Evidence");
                Console.WriteLine("3.\tShow Accidents");
                Console.WriteLine("4.\tShow Evidences(Negatives)");
                Console.WriteLine("Exit.\tExit");
                var input = Console.ReadLine();
                if (input == "1")
                {
                    Accident acc = new Accident();
                    Evidence evi = new Evidence();
                    Console.Write("Situation\t: ");
                    acc.Situation = Console.ReadLine();
                    evi.Thinks = acc.Situation;
                    Console.Write("Negative\t: ");
                    acc.Negative = Console.ReadLine();
                    Console.Write("Reaction\t: ");
                    acc.UnconsiousReact = Console.ReadLine();
                    Console.Write("Positive\t: ");
                    acc.Positive = Console.ReadLine();
                    Console.Write("What to do next: ");
                    acc.WhatToDoNext = Console.ReadLine();
                    evi.WhatToDoNext = acc.WhatToDoNext;
                    addAccident(acc);
                    addEvidence(evi);
                }
                else if (input == "2")
                {
                    Evidence evi = new Evidence();
                    Console.Write("Situation\t: ");
                    evi.Thinks = Console.ReadLine();
                    Console.Write("What to do next: ");
                    evi.WhatToDoNext = Console.ReadLine();
                    addEvidence(evi);
                }
                else if (input == "3")
                {
                    int i = 1;
                    foreach (var acc in getAccidents())
                    {
                        Console.WriteLine("======================");
                        Console.WriteLine($"{i}. About {acc.UnconsiousReact}");
                        Console.WriteLine("----------------------");
                        Console.WriteLine($"{acc.Situation}");
                        Console.WriteLine($"{acc.Positive}");
                        Console.WriteLine($"{acc.Negative}");
                        Console.WriteLine($"{acc.WhatToDoNext}");
                        i++;
                    }
                }
                else if (input == "4")
                {
                    int i = 1;
                    foreach (var evi in getEvidences())
                    {
                        Console.WriteLine("======================");
                        Console.WriteLine($"{i}. About {evi.Thinks}");
                        Console.WriteLine("----------------------");
                        Console.WriteLine($"{evi.WhatToDoNext}");
                        i++;
                    }
                }
                else if (input == "Exit")
                {
                    break;
                }
            }
            return;
        }
        private static void addAccident(Accident acc)
        {
            using (var conn = db.Connection)
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = "INSERT INTO Accident(Situation, Negative, UnconsiousReact, Positive, WhatToDoNext) VALUES(@s, @n, @u, @p, @w)";
                command.Parameters.AddWithValue("@s", acc.Situation);
                command.Parameters.AddWithValue("@n", acc.Negative);
                command.Parameters.AddWithValue("@u", acc.UnconsiousReact);
                command.Parameters.AddWithValue("@p", acc.Positive);
                command.Parameters.AddWithValue("@w", acc.WhatToDoNext);
                command.Prepare();
                command.ExecuteNonQuery();
            }
        }
        private static void addEvidence(Evidence evi)
        {
            using (var conn = db.Connection)
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = "INSERT INTO Evidence(Thinks, WhatToDoNext) VALUES(@t, @w)";
                command.Parameters.AddWithValue("@t", evi.Thinks);
                command.Parameters.AddWithValue("@w", evi.WhatToDoNext);
                command.Prepare();
                command.ExecuteNonQuery();
            }
        }
        private static List<Accident> getAccidents()
        {
            List<Accident> list = new List<Accident>();
            using (var conn = db.Connection)
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Accident";
                using SqliteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    list.Add(new Accident(rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetString(5)));
            }
            return list;
        }
        private static List<Evidence> getEvidences()
        {
            List<Evidence> list = new List<Evidence>();
            using (var conn = db.Connection)
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Evidence";
                using SqliteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    list.Add(new Evidence(rdr.GetString(1), rdr.GetString(2)));
            }
            return list;
        }
        private static void initDB()
        {
            using (var conn = db.Connection)
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = "CREATE TABLE IF NOT EXISTS Accident " +
                    "(id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "Situation TEXT NOT NULL," +
                    "Negative TEXT NOT NULL," +
                    "UnconsiousReact TEXT NOT NULL," +
                    "Positive TEXT NOT NULL," +
                    "WhatToDoNext TEXT NOT NULL);";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE IF NOT EXISTS Evidence " +
                    "(id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "WhatToDoNext TEXT NOT NULL," +
                    "Thinks TEXT NOT NULL);";
                command.ExecuteNonQuery();
            }
        }
        private static Config configure(string[] opt)
        {
            var config = new Config();
            if (opt.Contains("eng"))
                config.Eng = 1;
            else
                config.Eng = 0;
            return config;
        }
        private static void printWelcomeMsgs(Config config)
        {
            if (config == null || config.Showlog == false)
                return;
            //WelcomeMessage
            for (int i = 0; i < welcomes; i += 2)
            {
                var msg = messages[i + config.Eng];
                Console.WriteLine(msg);
            }
        }
    }
}