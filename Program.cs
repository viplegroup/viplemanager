// Viple FilesVersion - Program 1.0.0 - Date 25/06/2025
// Application créée par Viple SAS

using System;
using System.Windows.Forms;

namespace VipleManagement
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Vérifier si l'utilisateur est connecté
            if (!AuthenticationManager.IsUserLoggedIn())
            {
                Application.Run(new LoginForm());
            }
            else
            {
                Application.Run(new MainForm());
            }
        }
    }
}