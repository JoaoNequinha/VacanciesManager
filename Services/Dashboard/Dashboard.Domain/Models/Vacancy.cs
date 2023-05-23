namespace Dashboard.Domain.Models;

public class Vacancy
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Skill { get; private set; }
    public string? Location { get; private set; }
    public string Target_start_date { get; private set; }
    public int Vacancy_count { get; private set; }
    public string Is_open { get; private set; }
    public string Project_name { get; private set; }
    public string Client_name { get; private set; }
    public int ProjectId { get; set; }
    public Project Project { get; private set; }

    public Vacancy() { }

    public Vacancy(
        string name,
        string skill,
        string? location,
        string target_start_date,
        int vacancy_count,
        string is_open,
        string project_name,
        string client_name
        )
    {
        Name = name;
        Skill = skill;
        Location = location;
        Target_start_date = target_start_date;
        Vacancy_count = vacancy_count;
        Is_open = is_open;
        Project_name = project_name;
        Client_name = client_name;
    }
}