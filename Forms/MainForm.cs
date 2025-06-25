// Viple FilesVersion - MainForm 1.0.0 - Date 25/06/2025
// Application créée par Viple SAS

using System;
using System.Drawing;
using System.Windows.Forms;

namespace VipleManagement
{
    public partial class MainForm : Form
    {
        private MenuStrip menuStrip;
        private StatusStrip statusStrip;
        private Panel mainPanel;

        public MainForm()
        {
            InitializeComponent();
            SetupMenu();
            SetupStatusBar();
            LogManager.LogAction("Ouverture de l'application principale");
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1200, 800);
            this.Text = "Viple Management System - v1.0.0";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.WindowState = FormWindowState.Maximized;
        }

        private void SetupMenu()
        {
            menuStrip = new MenuStrip();
            menuStrip.BackColor = Color.FromArgb(37, 37, 38);
            menuStrip.ForeColor = Color.White;

            // Menu Clients
            ToolStripMenuItem clientsMenu = new ToolStripMenuItem("Clients");
            clientsMenu.DropDownItems.Add("Liste des clients", null, (s, e) => OpenClientsForm());
            clientsMenu.DropDownItems.Add("Ajouter un client", null, (s, e) => AddClient());

            // Menu Services
            ToolStripMenuItem servicesMenu = new ToolStripMenuItem("Services");
            servicesMenu.DropDownItems.Add("État des services", null, (s, e) => OpenServicesStatus());
            servicesMenu.DropDownItems.Add("Catalogue de services", null, (s, e) => OpenServicesCatalog());
            servicesMenu.DropDownItems.Add("Gestion des services", null, (s, e) => OpenServicesManagement());

            // Menu Liens
            ToolStripMenuItem linksMenu = new ToolStripMenuItem("Liens");
            linksMenu.DropDownItems.Add("Liens utiles", null, (s, e) => OpenLinksPage());

            // Menu Administration (selon le rôle)
            ToolStripMenuItem adminMenu = new ToolStripMenuItem("Administration");
            adminMenu.DropDownItems.Add("Gestion des utilisateurs", null, (s, e) => OpenUserManagement());
            adminMenu.DropDownItems.Add("Logs d'activité", null, (s, e) => OpenActivityLogs());
            adminMenu.Visible = AuthenticationManager.GetCurrentUserRole() == UserRole.Administrator;

            // Menu Système
            ToolStripMenuItem systemMenu = new ToolStripMenuItem("Système");
            systemMenu.DropDownItems.Add("À propos", null, (s, e) => ShowAbout());
            systemMenu.DropDownItems.Add("Déconnexion", null, (s, e) => Logout());

            menuStrip.Items.AddRange(new ToolStripItem[] { clientsMenu, servicesMenu, linksMenu, adminMenu, systemMenu });
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }

        private void SetupStatusBar()
        {
            statusStrip = new StatusStrip();
            statusStrip.BackColor = Color.FromArgb(37, 37, 38);
            statusStrip.ForeColor = Color.White;

            ToolStripStatusLabel userLabel = new ToolStripStatusLabel($"Utilisateur: {AuthenticationManager.GetCurrentUser()}");
            ToolStripStatusLabel roleLabel = new ToolStripStatusLabel($"Rôle: {AuthenticationManager.GetCurrentUserRole()}");
            ToolStripStatusLabel timeLabel = new ToolStripStatusLabel($"Connexion: {DateTime.Now:HH:mm:ss}");

            statusStrip.Items.AddRange(new ToolStripItem[] { userLabel, roleLabel, timeLabel });
            this.Controls.Add(statusStrip);
        }

        private void OpenClientsForm()
        {
            LogManager.LogAction("Ouverture de la liste des clients");
            // Implémenter l'ouverture du formulaire clients
        }

        private void AddClient()
        {
            LogManager.LogAction("Ajout d'un nouveau client");
            // Implémenter l'ajout de client
        }

        private void OpenServicesStatus()
        {
            LogManager.LogAction("Consultation de l'état des services");
            // Implémenter la consultation des services
        }

        private void OpenServicesCatalog()
        {
            LogManager.LogAction("Ouverture du catalogue de services");
            // Implémenter le catalogue
        }

        private void OpenServicesManagement()
        {
            LogManager.LogAction("Ouverture de la gestion des services");
            // Implémenter la gestion des services
        }

        private void OpenLinksPage()
        {
            LogManager.LogAction("Ouverture de la page des liens");
            // Implémenter la page des liens
        }

        private void OpenUserManagement()
        {
            LogManager.LogAction("Ouverture de la gestion des utilisateurs");
            // Implémenter la gestion des utilisateurs
        }

        private void OpenActivityLogs()
        {
            LogManager.LogAction("Consultation des logs d'activité");
            // Implémenter la consultation des logs
        }

        private void ShowAbout()
        {
            MessageBox.Show(
                "Viple Management System v1.0.0\n" +
                "Application créée par Viple SAS\n" +
                "Date de création: 25/06/2025\n\n" +
                "Système de gestion des services Viple",
                "À propos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void Logout()
        {
            LogManager.LogAction("Déconnexion de l'utilisateur");
            AuthenticationManager.Logout();
            this.Close();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }
    }
}