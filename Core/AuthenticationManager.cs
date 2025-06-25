// Viple FilesVersion - AuthenticationManager 1.0.0 - Date 25/06/2025
// Application créée par Viple SAS

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VipleManagement
{
    public static class AuthenticationManager
    {
        private static string currentUser = "";
        private static UserRole currentUserRole = UserRole.Employee;
        private static Dictionary<string, UserInfo> users;
        private static string usersFilePath = Path.Combine("vipledata", "users.vff");

        static AuthenticationManager()
        {
            LoadUsers();
        }

        public static bool Authenticate(string username, string password)
        {
            if (users.ContainsKey(username))
            {
                string hashedPassword = HashPassword(password);
                if (users[username].Password == hashedPassword)
                {
                    currentUser = username;
                    currentUserRole = users[username].Role;
                    SaveLoginSession();
                    return true;
                }
            }
            return false;
        }

        public static bool IsUserLoggedIn()
        {
            string sessionFile = Path.Combine("vipledata", "session.vff");
            return File.Exists(sessionFile);
        }

        public static string GetCurrentUser()
        {
            return currentUser;
        }

        public static UserRole GetCurrentUserRole()
        {
            return currentUserRole;
        }

        private static void LoadUsers()
        {
            users = new Dictionary<string, UserInfo>();
            
            // Créer le dossier si nécessaire
            Directory.CreateDirectory("vipledata");
            
            if (File.Exists(usersFilePath))
            {
                string[] lines = File.ReadAllLines(usersFilePath);
                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line) && !line.StartsWith("#"))
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length >= 3)
                        {
                            UserRole role = (UserRole)Enum.Parse(typeof(UserRole), parts[2]);
                            users[parts[0]] = new UserInfo
                            {
                                Username = parts[0],
                                Password = parts[1],
                                Role = role
                            };
                        }
                    }
                }
            }
            else
            {
                CreateDefaultUsers();
            }
        }

        private static void CreateDefaultUsers()
        {
            // Créer des utilisateurs par défaut
            users["admin"] = new UserInfo
            {
                Username = "admin",
                Password = HashPassword("admin123"),
                Role = UserRole.Administrator
            };

            users["manager"] = new UserInfo
            {
                Username = "manager",
                Password = HashPassword("manager123"),
                Role = UserRole.Manager
            };

            users["employee"] = new UserInfo
            {
                Username = "employee",
                Password = HashPassword("emp123"),
                Role = UserRole.Employee
            };

            SaveUsers();
        }

        private static void SaveUsers()
        {
            List<string> lines = new List<string>
            {
                "# Viple Users File Format (.vff)",
                "# Format: username|hashedpassword|role",
                "# Créé par Viple SAS"
            };

            foreach (var user in users.Values)
            {
                lines.Add($"{user.Username}|{user.Password}|{user.Role}");
            }

            File.WriteAllLines(usersFilePath, lines);
        }

        private static void SaveLoginSession()
        {
            string sessionData = $"user={currentUser}\nrole={currentUserRole}\nlogintime={DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            File.WriteAllText(Path.Combine("vipledata", "session.vff"), sessionData);
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static void Logout()
        {
            currentUser = "";
            currentUserRole = UserRole.Employee;
            string sessionFile = Path.Combine("vipledata", "session.vff");
            if (File.Exists(sessionFile))
            {
                File.Delete(sessionFile);
            }
        }
    }

    public class UserInfo
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Employee,
        Manager,
        Administrator
    }
}