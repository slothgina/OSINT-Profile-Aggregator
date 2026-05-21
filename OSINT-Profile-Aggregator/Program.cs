using System;
using System.IO;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        var data = LoadProfiles("profiles.json");

        bool running = true;

        while (running)
        {   
            SafeClear();
            Console.WriteLine("=== OSINT Profile Aggregator ===");
            Console.WriteLine("1. View All Profiles");
            Console.WriteLine("2. Search Profiles");
            Console.WriteLine("3. Exit");
            Console.WriteLine("4. View Risk Scores");
            Console.Write("\nChoose an option: ");

            string choice = Console.ReadLine()!;

            switch (choice)
            {
                case "1":
                    ViewAllProfiles(data);
                    break;

                case "2":
                    SearchProfiles(data);
                    break;

                case "3":
                    running = false;
                    break;

                case "4":
                    ViewRiskScores(data);
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    Pause();
                    break;
            }
        }
    }

    static void ViewAllProfiles(ProfileCollection data)
    {
        SafeClear();
        Console.WriteLine("=== All Profiles ===\n");

        foreach (var p in data.Profiles)
        {
            Console.WriteLine($"Username: {p.Username}");
            Console.WriteLine($"Platform: {p.Platform}");
            Console.WriteLine($"Followers: {p.Followers}");
            Console.WriteLine($"Last Active: {p.LastActive}");
            Console.WriteLine($"Bio: {p.Bio}");
            Console.WriteLine("-----------------------------");
        }

        Pause();
    }

    static void SearchProfiles(ProfileCollection data)
    {
        SafeClear();
        Console.Write("Enter username to search: ");
        string query = Console.ReadLine()!.ToLower();

        var results = data.Profiles.FindAll(p =>
            p.Username.ToLower().Contains(query));

        Console.WriteLine("\n=== Search Results ===\n");

        if (results.Count == 0)
        {
            Console.WriteLine("No profiles found.");
        }
        else
        {
            foreach (var p in results)
            {
                Console.WriteLine($"{p.Username} — {p.Platform} — {p.Followers} followers");
            }
        }

        Pause();
    }
    static int CalculateRisk(Profile p)
    {
        int score = 0;

        if (p.Followers > 5000) score += 3;
        else if (p.Followers > 1000) score += 2;
        else score += 1;

        var days = (DateTime.UtcNow - p.LastActive).TotalDays;
        if (days < 7) score += 3;
        else if (days < 30) score += 2;
        else score += 1;

        if (p.PostsPerWeek > 10) score += 3;
        else if (p.PostsPerWeek > 3) score += 2;
        else score += 1;

        if (p.RecentChanges.Count > 3) score += 3;
        else if (p.RecentChanges.Count > 0) score += 2;

        return score;
    }
        static string RiskLevel(int score)
        {
            if (score <= 4) return "Low";
            if (score <= 8) return "Medium";
            return "High";
        }
    
        static void ViewRiskScores(ProfileCollection data)
        {
            Console.Clear();
            Console.WriteLine("=== Risk Scores ===\n");

            foreach (var p in data.Profiles)
            {
                int score = CalculateRisk(p);
                string level = RiskLevel(score);

                Console.WriteLine($"{p.Username} — {level} Risk ({score})");
            }

            Pause();
        }
static void SafeClear()
{
    try
    {
        Console.Clear();

    }
    catch
    {
        // VS Code debugger can't clear the console — ignore
    }
}


            static void Pause()
            {
             Console.WriteLine("\nPress ENTER to return to the menu...");
             Console.ReadLine();
            }

    public static ProfileCollection LoadProfiles(string filePath)
    {
        string json = File.ReadAllText(filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<ProfileCollection>(json, options)!;
    }
}

