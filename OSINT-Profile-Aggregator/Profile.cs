using System;
using System.Collections.Generic;

public class Profile
{
    public string Username { get; set; } = "";
    public string Platform { get; set; } = "";
    public int Followers { get; set; }
    public DateTime LastActive { get; set; }
    public string Bio { get; set; } = "";
    public string Created { get; set; } = "";
    public int PostsPerWeek { get; set; }
    public List<string> RecentChanges { get; set; } = new List<string>();
    public List<string> RiskNotes { get; set; } = new List<string>();
}
