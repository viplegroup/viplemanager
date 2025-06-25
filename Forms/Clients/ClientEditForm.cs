// Viple FilesVersion - ClientEditForm 1.0.0 - Date 25/06/2025
// Application créée par Viple SAS

using System;
using System.Drawing;
using System.Windows.Forms;
using VipleManagement.Models;
using VipleManagement.Services;

namespace VipleManagement.Forms.Clients
{
    public partial class ClientEditForm : Form
    {
        private ClientManager clientManager;
        private Client client;
        private bool isNewClient;

        private TextBox txtName;
        private TextBox txtAddress;
        private TextBox txtCity;
        private TextBox txtPostalCode;
        private TextBox txtEmail;
        private TextBox txtPhone;
        private TextBox txtContactPerson;
        private TextBox txtNotes;
        private CheckBox chkIsActive;
        private Button btnSave;
        private Button btnCancel;

        public ClientEditForm(Client client = null)
        {
            this.client = client ?? new Client();
            isNewClient = client == null;
            
            clientManager = new ClientManager();
            InitializeComponent();
            SetupUI();
            PopulateForm();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(500, 500);
            this.Text = isNewClient ? "Viple - Ajouter un client" : "Viple - Modifier un client";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;
        }

        private void SetupUI()
        {
            // Label et TextBox pour le nom
            Label lblName = new Label
            {
                Text = "Nom:",
                Location = new Point(20, 20),
                Size = new Size(100, 20),
                ForeColor = Color.White
            };

            txtName = new TextBox
            {
                Location = new Point(150, 20),
                Size = new Size(300, 20),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Label et TextBox pour l'adresse
            Label lblAddress = new Label
            {
                Text = "Adresse:",
                Location = new Point(20, 50),
                Size = new Size(100, 20),
                ForeColor = Color.White
            };

            txtAddress = new TextBox
            {
                Location = new Point(150, 50),
                Size = new Size(300, 20),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Label et TextBox pour la ville
            Label lblCity = new Label
            {
                Text = "Ville:",
                Location = new Point(20, 80),
                Size = new Size(100, 20),
                ForeColor = Color.White
            };

            txtCity = new TextBox
            {
                Location = new Point(150, 80),
                Size = new Size(300, 20),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Label et TextBox pour le code postal
            Label lblPostalCode = new Label
            {
                Text = "Code postal:",
                Location = new Point(20, 110),
                Size = new Size(100, 20),
                ForeColor = Color.White
            };

            txtPostalCode = new TextBox
            {
                Location = new Point(150, 110),
                Size = new Size(300, 20),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Label et TextBox pour l'email
            Label lblEmail = new Label
            {
                Text = "Email:",
                Location = new Point(20, 140),
                Size = new Size(100, 20),
                ForeColor = Color.White
            };

            txtEmail = new TextBox
            {
                Location = new Point(150, 140),
                Size = new Size(300, 20),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Label et TextBox pour le téléphone
            Label lblPhone = new Label
            {
                Text = "Téléphone:",
                Location = new Point(20, 170),
                Size = new Size(100, 20),
                ForeColor = Color.White
            };

            txtPhone = new TextBox
            {
                Location = new Point(150, 170),
                Size = new Size(300, 20),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Label et TextBox pour la personne de contact
            Label lblContactPerson = new Label
            {
                Text = "Contact:",
                Location = new Point(20, 200),
                Size = new Size(100, 20),
                ForeColor = Color.White
            };

            txtContactPerson = new TextBox
            {
                Location = new Point(150, 200),
                Size = new Size(300, 20),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Label et TextBox pour les notes
            Label lblNotes = new Label
            {
                Text = "Notes:",
                Location = new Point(20, 230),
                Size = new Size(100, 20),
                ForeColor = Color.White
            };

            txtNotes = new TextBox
            {
                Location = new Point(150, 230),
                Size = new Size(300, 80),
                Multiline = true,
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // CheckBox pour actif/inactif
            chkIsActive = new CheckBox
            {
                Text = "Client actif",
                Location = new Point(150, 320),
                Size = new Size(100, 20),
                ForeColor = Color.White,
                Checked = true
            };

            // Boutons
            btnSave = new Button
            {
                Text = "Enregistrer",
                DialogResult = DialogResult.OK,
                Location = new Point(150, 360),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Annuler",
                DialogResult = DialogResult.Cancel,
                Location = new Point(260, 360),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White
            };
            btnCancel.Click += BtnCancel_Click;

            // Ajouter les contrôles au formulaire
            this.Controls.AddRange(new Control[]
            {
                lblName, txtName,
                lblAddress, txtAddress,
                lblCity, txtCity,
                lblPostalCode, txtPostalCode,
                lblEmail, txtEmail,
                lblPhone, txtPhone,
                lblContactPerson, txtContactPerson,
                lblNotes, txtNotes,
                chkIsActive,
                btnSave, btnCancel
            });
        }

        private void PopulateForm()
        {
            txtName.Text = client.Name;
            txtAddress.Text = client.Address;
            txtCity.Text = client.City;
            txtPostalCode.Text = client.PostalCode;
            txtEmail.Text = client.Email;
            txtPhone.Text = client.Phone;
            txtContactPerson.Text = client.ContactPerson;
            txtNotes.Text = client.Notes;
            chkIsActive.Checked = client.IsActive;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                client.Name = txtName.Text;
                client.Address = txtAddress.Text;
                client.City = txtCity.Text;
                client.PostalCode = txtPostalCode.Text;
                client.Email = txtEmail.Text;
                client.Phone = txtPhone.Text;
                client.ContactPerson = txtContactPerson.Text;
                client.Notes = txtNotes.Text;
                client.IsActive = chkIsActive.Checked;
                client.LastModified = DateTime.Now;

                bool success;
                if (isNewClient)
                {
                    success = clientManager.AddClient(client);
                }
                else
                {
                    success = clientManager.UpdateClient(client);
                }

                if (success)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Une erreur s'est produite lors de l'enregistrement du client.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Le nom du client est obligatoire.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) && string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Veuillez saisir au moins une information de contact (email ou téléphone).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}