// Viple FilesVersion - ServiceManager 1.0.0 - Date 25/06/2025
// Application créée par Viple SAS

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using VipleManagement.Models;

namespace VipleManagement.Services
{
    public class ServiceManager
    {
        private List<Service> services;
        private string servicesFilePath = Path.Combine("vipledata", "services.vff");

        public ServiceManager()
        {
            services = new List<Service>();
            LoadServices();
        }

        public bool AddService(Service service)
        {
            try
            {
                services.Add(service);
                SaveServices();
                LogManager.LogAction($"Service ajouté: {service.Name}");
                return true;
            }
            catch (Exception ex)
            {
                LogManager.LogAction($"Erreur lors de l'ajout du service: {ex.Message}");
                return false;
            }
        }

        public bool UpdateService(Service service)
        {
            try
            {
                Service existingService = services.FirstOrDefault(s => s.Id == service.Id);
                if (existingService != null)
                {
                    int index = services.IndexOf(existingService);
                    services[index] = service;
                    SaveServices();
                    LogManager.LogAction($"Service mis à jour: {service.Name}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogManager.LogAction($"Erreur lors de la mise à jour du service: {ex.Message}");
                return false;
            }
        }

        public bool DeleteService(string serviceId)
        {
            try
            {
                Service serviceToDelete = services.FirstOrDefault(s => s.Id == serviceId);
                if (serviceToDelete != null)
                {
                    services.Remove(serviceToDelete);
                    SaveServices();
                    LogManager.LogAction($"Service supprimé: {serviceToDelete.Name}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogManager.LogAction($"Erreur lors de la suppression du service: {ex.Message}");
                return false;
            }
        }

        public Service GetServiceById(string serviceId)
        {
            return services.FirstOrDefault(s => s.Id == serviceId);
        }

        public List<Service> GetAllServices()
        {
            return services;
        }

        public List<Service> GetServicesByCategory(ServiceCategory category)
        {
            return services.Where(s => s.Category == category).ToList();
        }

        public async Task CheckAllServicesStatusAsync()
        {
            LogManager.LogAction("Vérification du statut de tous les services");
            foreach (var service in services.Where(s => s.RequiresMonitoring))
            {
                await Task.Run(() => service.CheckStatus());
                UpdateService(service);
            }
        }

        public async Task<bool> CheckServiceStatusAsync(string serviceId)
        {
            Service service = GetServiceById(serviceId);
            if (service != null)
            {
                LogManager.LogAction($"Vérification du statut du service: {service.Name}");
                bool status = await Task.Run(() => service.CheckStatus());
                UpdateService(service);
                return status;
            }
            return false;
        }

        private void LoadServices()
        {
            try
            {
                if (File.Exists(servicesFilePath))
                {
                    using (FileStream fs = new FileStream(servicesFilePath, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        services = (List<Service>)formatter.Deserialize(fs);
                    }
                }
                else
                {
                    // Créer quelques services de test si le fichier n'existe pas
                    CreateSampleServices();
                    SaveServices();
                }
            }
            catch (Exception ex)
            {
                LogManager.LogAction($"Erreur lors du chargement des services: {ex.Message}");
                services = new List<Service>();
                CreateSampleServices();
            }
        }

        private void SaveServices()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(servicesFilePath));
                using (FileStream fs = new FileStream(servicesFilePath, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, services);
                }
                UpdateFilesLog("services.vff", "modifiée");
            }
            catch (Exception ex)
            {
                LogManager.LogAction($"Erreur lors de la sauvegarde des services: {ex.Message}");
            }
        }

        private void CreateSampleServices()
        {
            services.Add(new Service
            {
                Name = "Hébergement Web Premium",
                Description = "Service d'hébergement web avec haute disponibilité",
                MonthlyFee = 29.99m,
                Category = ServiceCategory.WebHosting,
                Status = ServiceStatus.Running,
                RequiresMonitoring = true,
                MonitoringUrl = "https://example.com/status"
            });

            services.Add(new Service
            {
                Name = "Messagerie Pro",
                Description = "Service de messagerie professionnelle avec domaine personnalisé",
                MonthlyFee = 9.99m,
                Category = ServiceCategory.Email,
                Status = ServiceStatus.Running,
                RequiresMonitoring = true,
                MonitoringUrl = "https://mail.example.com/status"
            });

            services.Add(new Service
            {
                Name = "Stockage Cloud 1To",
                Description = "Service de stockage cloud avec chiffrement des données",
                MonthlyFee = 14.99m,
                Category = ServiceCategory.Storage,
                Status = ServiceStatus.Running,
                RequiresMonitoring = true,
                MonitoringUrl = "https://storage.example.com/status"
            });

            services.Add(new Service
            {
                Name = "Base de données SQL",
                Description = "Service de base de données SQL managée",
                MonthlyFee = 19.99m,
                Category = ServiceCategory.Database,
                Status = ServiceStatus.Running,
                RequiresMonitoring = true,
                MonitoringUrl = "https://db.example.com/status"
            });

            services.Add(new Service
            {
                Name = "Support Premium 24/7",
                Description = "Service de support technique disponible 24h/24 et 7j/7",
                MonthlyFee = 49.99m,
                Category = ServiceCategory.Support,
                Status = ServiceStatus.Running,
                RequiresMonitoring = false
            });
        }

        private void UpdateFilesLog(string fileName, string action)
        {
            string filesLogPath = "elioslogs-files.txt";
            string entry = $"- Fichier : {fileName} {action} le {DateTime.Now:dd/MM/yyyy} à {DateTime.Now:HH:mm}";
            
            // Vérifier si une entrée pour ce fichier existe déjà aujourd'hui
            if (File.Exists(filesLogPath))
            {
                string[] lines = File.ReadAllLines(filesLogPath);
                bool entryExists = false;
                
                foreach (string line in lines)
                {
                    if (line.Contains(fileName) && line.Contains(DateTime.Now.ToString("dd/MM/yyyy")))
                    {
                        entryExists = true;
                        break;
                    }
                }
                
                if (!entryExists)
                {
                    File.AppendAllText(filesLogPath, entry + Environment.NewLine);
                }
            }
            else
            {
                File.AppendAllText(filesLogPath, entry + Environment.NewLine);
            }
        }
    }
}