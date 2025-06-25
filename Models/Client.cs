// Viple FilesVersion - Client 1.0.0 - Date 25/06/2025
// Application créée par Viple SAS

using System;
using System.Collections.Generic;

namespace VipleManagement.Models
{
    [Serializable]
    public class Client
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ContactPerson { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModified { get; set; }
        public List<Service> SubscribedServices { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }

        public Client()
        {
            Id = Guid.NewGuid().ToString();
            CreationDate = DateTime.Now;
            LastModified = DateTime.Now;
            SubscribedServices = new List<Service>();
            IsActive = true;
        }

        public decimal CalculateTotalMonthlyFees()
        {
            decimal total = 0;
            foreach (var service in SubscribedServices)
            {
                total += service.MonthlyFee;
            }
            return total;
        }

        public override string ToString()
        {
            return $"{Name} ({Id})";
        }
    }
}