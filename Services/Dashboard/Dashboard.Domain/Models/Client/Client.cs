using Dashboard.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Domain.Models
{
    public class Client
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string? AccountManager { get; set; }
        public string? Description { get; set; }
        public byte[]? ClientLogo { get; set; }
        public int ProjectCount { get; set; }
        public int VacancyCount { get; set; }
        public ICollection<Project> Projects { get; set; }

        private Client() { }

        public Client(
            string name,
            string? accountManager,
            string? description,
            byte[]? clientLogo,
            int projectCount,
            int vacancyCount
            )
        {
            Name = name;
            AccountManager = accountManager;
            Description = description;
            ClientLogo = clientLogo;
            ProjectCount = projectCount;
            VacancyCount = vacancyCount;
        }
    }
}
