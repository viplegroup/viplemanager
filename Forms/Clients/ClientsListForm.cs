// Viple FilesVersion - ClientsListForm 1.0.0 - Date 25/06/2025
// Application créée par Viple SAS

using System;
using System.Drawing;
using System.Windows.Forms;
using VipleManagement.Models;
using VipleManagement.Services;

namespace VipleManagement.Forms.Clients
{
    public partial class ClientsListForm : Form
    {
        private ClientManager clientManager;
        private DataGridView dgvClients;
        private TextBox txtSearch;
        private Button btnAddClient;
        private Button btnEditClient;
        private Button btnDeleteClient;
        private Button btnRefresh;

        public ClientsListForm()
        {
            InitializeComponent();
            clientManager = new ClientManager();
            SetupUI();
            LoadClients();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1000, 600);
            this.Text = "Viple - Liste des clients";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;
        }

        private void SetupUI()
        {
            // Panel de recherche
            Panel searchPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(37, 37, 38)
            };

            Label lblSearch = new Label
            {
                Text = "Rechercher:",
                ForeColor = Color.White,
                Location = new Point(10, 15),
                Size = new Size(80, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            txtSearch = new TextBox
            {
                Location = new Point(95, 15),
                Size = new Size(300, 20),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            btnRefresh = new Button
            {
                Text = "Actualiser",
                Location = new Point(410, 10),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            btnRefresh.Click += BtnRefresh_Click;

            searchPanel.Controls.AddRange(new Control[] { lblSearch, txtSearch, btnRefresh });

            // Panel des boutons
            Panel buttonsPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(37, 37, 38)
            };

            btnAddClient = new Button
            {
                Text = "Ajouter un client",
                Location = new Point(10, 10),
                Size = new Size(120, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            btnAddClient.Click += BtnAddClient_Click;

            btnEditClient = new Button
            {
                Text = "Modifier",
                Location = new Point(140, 10),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            btnEditClient.Click += BtnEditClient_Click;

            btnDeleteClient = new Button
            {
                Text = "Supprimer",
                Location = new Point(250, 10),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(204, 0, 0),
                ForeColor = Color.White
            };
            btnDeleteClient.Click += BtnDeleteClient_Click;

            buttonsPanel.Controls.AddRange(new Control[] { btnAddClient, btnEditClient, btnDeleteClient });

            // DataGridView
            dgvClients = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Black,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                GridColor = Color.FromArgb(60, 60, 60)
            };
            dgvClients.EnableHeadersVisualStyles = false;
            dgvClients.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgvClients.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvClients.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 45, 48);
            dgvClients.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dgvClients.ColumnHeadersHeight = 40;

            dgvClients.DefaultCellStyle.BackColor = Color.FromArgb(37, 37, 38);
            dgvClients.DefaultCellStyle.ForeColor = Color.White;
            dgvClients.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dgvClients.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvClients.DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            dgvClients.RowTemplate.Height = 30;

            dgvClients.CellDoubleClick += DgvClients_CellDoubleClick;

            // Ajout des contrôles
            this.Controls.AddRange(new Control[] { dgvClients, searchPanel, buttonsPanel });
        }

        private void LoadClients()
        {
            dgvClients.DataSource = null;
            dgvClients.Columns.Clear();

            var clients = clientManager.GetAllClients();
            
            dgvClients.DataSource = clients;

            // Configurer les colonnes
            if (dgvClients.Columns.Count > 0)
            {
                dgvClients.Columns["Id"].Visible = false;
                dgvClients.Columns["SubscribedServices"].Visible = false;
                dgvClients.Columns["Notes"].Visible = false;
                dgvClients.Columns["CreationDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvClients.Columns["LastModified"].DefaultCellStyle.Format = "dd/MM/yyyy";

                // Renommer les colonnes
                dgvClients.Columns["Name"].HeaderText = "Nom";
                dgvClients.Columns["Address"].HeaderText = "Adresse";
                dgvClients.Columns["City"].HeaderText = "Ville";
                dgvClients.Columns["PostalCode"].HeaderText = "Code Postal";
                dgvClients.Columns["Email"].HeaderText = "Email";
                dgvClients.Columns["Phone"].HeaderText = "Téléphone";
                dgvClients.Columns["ContactPerson"].HeaderText = "Contact";
                dgvClients.Columns["CreationDate"].HeaderText = "Date de création";
                dgvClients.Columns["LastModified"].HeaderText = "Dernière modification";
                dgvClients.Columns["IsActive"].HeaderText = "Actif";
            }

            LogManager.LogAction("Liste des clients chargée");
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text;
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                LoadClients();
            }
            else
            {
                dgvClients.DataSource = clientManager.SearchClients(searchTerm);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadClients();
        }

        private void BtnAddClient_Click(object sender, EventArgs e)
        {
            ClientEditForm form = new ClientEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadClients();
            }
        }

        private void BtnEditClient_Click(object sender, EventArgs e)
        {
            EditSelectedClient();
        }

        private void DgvClients_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                EditSelectedClient();
            }
        }

        private void EditSelectedClient()
        {
            if (dgvClients.SelectedRows.Count > 0)
            {
                string clientId = dgvClients.SelectedRows[0].Cells["Id"].Value.ToString();
                Client client = clientManager.GetClientById(clientId);
                
                if (client != null)
                {
                    ClientEditForm form = new ClientEditForm(client);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadClients();
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un client.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDeleteClient_Click(object sender, EventArgs e)
        {
            if (dgvClients.SelectedRows.Count > 0)
            {
                string clientId = dgvClients.SelectedRows[0].Cells["Id"].Value.ToString();
                string clientName = dgvClients.SelectedRows[0].Cells["Name"].Value.ToString();

                DialogResult result = MessageBox.Show(
                    $"Êtes-vous sûr de vouloir supprimer le client '{clientName}' ?",
                    "Confirmation de suppression",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    if (clientManager.DeleteClient(clientId))
                    {
                        LoadClients();
                    }
                    else
                    {
                        MessageBox.Show("Échec de la suppression du client.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un client.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}