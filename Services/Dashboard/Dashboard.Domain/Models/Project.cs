
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dashboard.Domain.Models
{
    public class Project 
    {
        public int Id { get; private set; }
        public int ClientId { get;  set; }
        public Client Client { get; set; }
        public string Name { get;  set; }
        public string? Description { get;  set; }
        public string? Contact { get;  set; }
        public string ClientName { get; set; }
        public byte[]? ClientLogo { get; set; }
        public int VacancyCount { get; set; }   
        public ICollection<Vacancy> Vacancies { get; set; }

        public Project()
        {
        }
               
        public Project( int clientId, string name, string description, string contact)
        {
           ClientId = clientId;
           Name = name;
           Description = description;
           Contact = contact;
        }
    }
}
