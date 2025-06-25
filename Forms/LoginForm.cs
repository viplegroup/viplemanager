// Viple FilesVersion - LoginForm 1.0.0 - Date 25/06/2025
// Application créée par Viple SAS

using System;
using System.Drawing;
using System.Windows.Forms;

namespace VipleManagement
{
    public partial class LoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblTitle;
        private Panel pnlLogin;

        public LoginForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(400, 300);
            this.Text = "Viple - Authentification";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(45, 45, 48);
        }

        private void SetupUI()
        {
            // Panel principal
            pnlLogin = new Panel
            {
                Size = new Size(300, 200),
                Location = new Point(50, 50),
                BackColor = Color.FromArgb(37, 37, 38),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Titre
            lblTitle = new Label
            {
                Text = "VIPLE MANAGEMENT",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(50, 20),
                Size = new Size(200, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Nom d'utilisateur
            Label lblUsername = new Label
            {
                Text = "Nom d'utilisateur:",
                ForeColor = Color.White,
                Location = new Point(20, 60),
                Size = new Size(120, 20)
            };

            txtUsername = new TextBox
            {
                Location = new Point(20, 85),
                Size = new Size(260, 25),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Mot de passe
            Label lblPassword = new Label
            {
                Text = "Mot de passe:",
                ForeColor = Color.White,
                Location = new Point(20, 115),
                Size = new Size(100, 20)
            };

            txtPassword = new TextBox
            {
                Location = new Point(20, 140),
                Size = new Size(260, 25),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                UseSystemPasswordChar = true
            };

            // Bouton de connexion
            btnLogin = new Button
            {
                Text = "Se connecter",
                Location = new Point(110, 175),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            // Ajouter les contrôles
            pnlLogin.Controls.AddRange(new Control[] { lblUsername, txtUsername, lblPassword, txtPassword, btnLogin });
            this.Controls.AddRange(new Control[] { lblTitle, pnlLogin });
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (AuthenticationManager.Authenticate(username, password))
            {
                // Log de connexion
                LogManager.LogAction($"Connexion réussie pour {username}");
                
                this.Hide();
                MainForm mainForm = new MainForm();
                mainForm.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogManager.LogAction($"Tentative de connexion échouée pour {username}");
            }
        }
    }
}