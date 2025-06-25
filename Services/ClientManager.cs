// Viple FilesVersion - ClientManager 1.0.0 - Date 25/06/2025
// Application créée par Viple SAS

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using VipleManagement.Models;

namespace VipleManagement.Services
{
    public class ClientManager
    {
        private List<Client> clients;
        private string clientsFilePath = Path.Combine("vipledata", "clients.vff");

        public ClientManager()
        {
            clients = new List<Client>();
            LoadClients();
        }

        public bool AddClient(Client client)
        {
            try
            {
                clients.Add(client);
                SaveClients();
                LogManager.LogAction($"Client ajouté: {client.Name}");
                return true;
            }
            catch (Exception ex)
            {
                LogManager.LogAction($"Erreur lors de l'ajout du client: {ex.Message}");
                return false;
            }
        }

        public bool UpdateClient(Client client)
        {
            try
            {
                Client existingClient = clients.FirstOrDefault(c => c.Id == client.Id);
                if (existingClient != null)
                {
                    int index = clients.IndexOf(existingClient);
                    client.LastModified = DateTime.Now;
                    clients[index] = client;
                    SaveClients();
                    LogManager.LogAction($"Client mis à jour: {client.Name}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogManager.LogAction($"Erreur lors de la mise à jour du client: {ex.Message}");
                return false;
            }
        }

        public bool DeleteClient(string clientId)
        {
            try
            {
                Client clientToDelete = clients.FirstOrDefault(c => c.Id == clientId);
                if (clientToDelete != null)
                {
                    clients.Remove(clientToDelete);
                    SaveClients();
                    LogManager.LogAction($"Client supprimé: {clientToDelete.Name}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogManager.LogAction($"Erreur lors de la suppression du client: {ex.Message}");
                return false;
            }
        }

        public Client GetClientById(string clientId)
        {
            return clients.FirstOrDefault(c => c.Id == clientId);
        }

        public List<Client> GetAllClients()
        {
            return clients;
        }

        public List<Client> SearchClients(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            return clients.Where(c =>
                c.Name.ToLower().Contains(searchTerm) ||
                c.ContactPerson.ToLower().Contains(searchTerm) ||
                c.Email.ToLower().Contains(searchTerm) ||
                c.Phone.Contains(searchTerm) ||
                c.City.ToLower().Contains(searchTerm)
            ).ToList();
        }

        private void LoadClients()
        {
            try
            {
                if (File.Exists(clientsFilePath))
                {
                    using (FileStream fs = new FileStream(clientsFilePath, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        clients = (List<Client>)formatter.Deserialize(fs);
                    }
                }
                else
                {
                    // Créer quelques clients de test si le fichier n'existe pas
                    CreateSampleClients();
                    SaveClients();
                }
            }
            catch (Exception ex)
            {
                LogManager.LogAction($"Erreur lors du chargement des clients: {ex.Message}");
                clients = new List<Client>();
                CreateSampleClients();
            }
        }

        private void SaveClients()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(clientsFilePath));
                using (FileStream fs = new FileStream(clientsFilePath, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, clients);
                }
                UpdateFilesLog("clients.vff", "modifiée");
            }
            catch (Exception ex)
            {
                LogManager.LogAction($"Erreur lors de la sauvegarde des clients: {ex.Message}");
            }
        }

        private void CreateSampleClients()
        {
            clients.Add(new Client
            {
                Name = "Entreprise ABC",
                Address = "123 Avenue Principale",
                City = "Paris",
                PostalCode = "75001",
                Email = "contact@abc.fr",
                Phone = "01 23 45 67 89",
                ContactPerson = "Jean Dupont",
                Notes = "Client premium"
            });

            clients.Add(new Client
            {
                Name = "Société XYZ",
                Address = "456 Rue Secondaire",
                City = "Lyon",
                PostalCode = "69001",
                Email = "info@xyz.fr",
                Phone = "04 56 78 90 12",
                ContactPerson = "Marie Martin",
                Notes = "Nouveau client"
            });

            clients.Add(new Client
            {
                Name = "Tech Solutions",
                Address = "789 Boulevard Tertiaire",
                City = "Marseille",
                PostalCode = "13001",
                Email = "support@techsolutions.fr",
                Phone = "04 91 23 45 67",
                ContactPerson = "Pierre Dubois",
                Notes = "Client avec contrat de maintenance"
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