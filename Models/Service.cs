// Viple FilesVersion - Service 1.0.0 - Date 25/06/2025
// Application créée par Viple SAS

using System;
using System.Collections.Generic;

namespace VipleManagement.Models
{
    [Serializable]
    public class Service
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MonthlyFee { get; set; }
        public ServiceCategory Category { get; set; }
        public ServiceStatus Status { get; set; }
        public List<Product> AssociatedProducts { get; set; }
        public DateTime LastChecked { get; set; }
        public string LastStatusMessage { get; set; }
        public bool RequiresMonitoring { get; set; }
        public string MonitoringUrl { get; set; }
        public DateTime CreationDate { get; set; }

        public Service()
        {
            Id = Guid.NewGuid().ToString();
            AssociatedProducts = new List<Product>();
            Status = ServiceStatus.Inactive;
            CreationDate = DateTime.Now;
            LastChecked = DateTime.Now;
        }

        public bool CheckStatus()
        {
            // Simulation de vérification de statut
            // Dans une vraie implémentation, vérifierait le service via une API ou autre mécanisme
            if (RequiresMonitoring && !string.IsNullOrEmpty(MonitoringUrl))
            {
                try
                {
                    // Simuler une vérification de statut
                    Random rand = new Random();
                    bool isWorking = rand.Next(100) > 10; // 90% de chance que ça fonctionne

                    Status = isWorking ? ServiceStatus.Running : ServiceStatus.Error;
                    LastStatusMessage = isWorking ? "Service fonctionnel" : "Service en erreur";
                    LastChecked = DateTime.Now;

                    return isWorking;
                }
                catch (Exception ex)
                {
                    LastStatusMessage = $"Erreur lors de la vérification: {ex.Message}";
                    Status = ServiceStatus.Error;
                    return false;
                }
            }

            return Status == ServiceStatus.Running;
        }
    }

    public enum ServiceCategory
    {
        WebHosting,
        Email,
        Database,
        Security,
        Backup,
        Storage,
        ApiService,
        Monitoring,
        Support,
        Other
    }

    public enum ServiceStatus
    {
        Running,
        Warning,
        Error,
        Maintenance,
        Inactive
    }
}