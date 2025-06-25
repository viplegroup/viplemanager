// Viple FilesVersion - LogManager 1.0.0 - Date 25/06/2025
// Application créée par Viple SAS

using System;
using System.IO;

namespace VipleManagement
{
    public static class LogManager
    {
        private static string logFilePath = Path.Combine("vipledata", "user_actions.vff");

        public static void LogAction(string action)
        {
            Directory.CreateDirectory("vipledata");
            
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {AuthenticationManager.GetCurrentUser()} ({AuthenticationManager.GetCurrentUserRole()}): {action}";
            
            try
            {
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                UpdateFilesLog("user_actions.vff", "modifiée");
            }
            catch (Exception ex)
            {
                // En cas d'erreur, ne pas bloquer l'application
                Console.WriteLine($"Erreur lors de l'écriture du log: {ex.Message}");
            }
        }

        public static string[] GetAllLogs()
        {
            if (File.Exists(logFilePath))
            {
                return File.ReadAllLines(logFilePath);
            }
            return new string[0];
        }

        private static void UpdateFilesLog(string fileName, string action)
        {
            string filesLogPath = "elioslogs-files.txt";
            string entry = $"- Fichier : {fileName} {action} le {DateTime.Now:dd/MM/yyyy} à {DateTime.Now:HH:mm}";
            File.AppendAllText(filesLogPath, entry + Environment.NewLine);
        }
    }
}