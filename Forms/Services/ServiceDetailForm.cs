// Viple FilesVersion - ServiceDetailForm 1.0.0 - Date 25/06/2025
// Application créée par Viple SAS

using System;
using System.Drawing;
using System.Windows.Forms;
using VipleManagement.Models;
using VipleManagement.Services;

namespace VipleManagement.Forms.Services
{
    public partial class ServiceDetailForm : Form
    {
        private Service service;
        private ServiceManager serviceManager;
        private ProductManager productManager;
        private ClientManager clientManager;

        public ServiceDetailForm(Service service)
        {
            this.service = service;
            serviceManager = new ServiceManager();
            productManager = new ProductManager();
            clientManager = new ClientManager();
            
            InitializeComponent();
            LoadServiceDetails();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(800, 600);
            this.Text = "Viple - Détails du service";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;
        }

        private void LoadServiceDetails()
        {
            // Panel d'en-tête
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(37, 37, 38)
            };

            Label lblName = new Label
            {
                Text = service.Name,
                Location = new Point(20, 20),
                Size = new Size(500, 30),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White
            };

            Label lblCategory = new Label
            {
                Text = $"Catégorie: {service.Category} | Tarif mensuel: {service.MonthlyFee:C2} | Statut: {GetStatusText(service.Status)}",
                Location = new Point(20, 50),
                Size = new Size(500, 20),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Silver
            };

            // Indicateur de statut
            Panel statusIndicator = new Panel
            {
                Location = new Point(700, 20),
                Size = new Size(40, 40),
                BackColor = GetStatusColor(service.Status)
            };
            statusIndicator.Paint += (s, e) => 
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(60, 60, 60), 1), 0, 0, statusIndicator.Width - 1, statusIndicator.Height - 1);
            };

            headerPanel.Controls.AddRange(new Control[] { lblName, lblCategory, statusIndicator });

            // TabControl pour organiser les détails
            TabControl tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Appearance = TabAppearance.FlatButtons,
                ItemSize = new Size(100, 30)
            };

            // Style du TabControl
            tabControl.DrawItem += (s, e) => 
            {
                Graphics g = e.Graphics;
                TabPage tp = tabControl.TabPages[e.Index];
                Rectangle r = tabControl.GetTabRect(e.Index);

                if (e.Index == tabControl.SelectedIndex)
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(0, 122, 204)), r);
                }
                else
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(45, 45, 48)), r);
                }

                // Centrer le texte
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                g.DrawString(tp.Text, tabControl.Font, new SolidBrush(Color.White), r, sf);
            };
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;

            // Onglet "Informations"
            TabPage tabInfo = new TabPage("Informations");
            tabInfo.BackColor = Color.FromArgb(30, 30, 30);
            tabInfo.ForeColor = Color.White;
            SetupInfoTab(tabInfo);
            
            // Onglet "Produits associés"
            TabPage tabProducts = new TabPage("Produits");
            tabProducts.BackColor = Color.FromArgb(30, 30, 30);
            tabProducts.ForeColor = Color.White;
            SetupProductsTab(tabProducts);
            
            // Onglet "Clients"
            TabPage tabClients = new TabPage("Clients");
            tabClients.BackColor = Color.FromArgb(30, 30, 30);
            tabClients.ForeColor = Color.White;
            SetupClientsTab(tabClients);
            
            // Onglet "Historique"
            TabPage tabHistory = new TabPage("Historique");
            tabHistory.BackColor = Color.FromArgb(30, 30, 30);
            tabHistory.ForeColor = Color.White;
            SetupHistoryTab(tabHistory);

            tabControl.TabPages.AddRange(new TabPage[] { tabInfo, tabProducts, tabClients, tabHistory });

            // Panel de boutons en bas
            Panel bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(37, 37, 38)
            };

            Button btnClose = new Button
            {
                Text = "Fermer",
                Location = new Point(680, 10),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White
            };
            btnClose.Click += (s, e) => this.Close();
            
            Button btnEdit = new Button
            {
                Text = "Modifier",
                Location = new Point(570, 10),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            btnEdit.Click += BtnEdit_Click;
            
            Button btnCheckStatus = new Button
            {
                Text = "Vérifier le statut",
                Location = new Point(20, 10),
                Size = new Size(120, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Enabled = service.RequiresMonitoring
            };
            btnCheckStatus.Click += BtnCheckStatus_Click;

            bottomPanel.Controls.AddRange(new Control[] { btnClose, btnEdit, btnCheckStatus });

            this.Controls.AddRange(new Control[] { headerPanel, tabControl, bottomPanel });
        }

        private void SetupInfoTab(TabPage tab)
        {
            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                ColumnCount = 2,
                RowCount = 8,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

            // Description
            AddRow(table, 0, "Description:", service.Description);
            
            // Date de création
            AddRow(table, 1, "Date de création:", service.CreationDate.ToString("dd/MM/yyyy HH:mm:ss"));
            
            // Dernière vérification
            AddRow(table, 2, "Dernière vérification:", service.LastChecked.ToString("dd/MM/yyyy HH:mm:ss"));
            
            // Message de statut
            AddRow(table, 3, "Message de statut:", service.LastStatusMessage ?? "Aucune information");
            
            // Surveillance requise
            AddRow(table, 4, "Surveillance requise:", service.RequiresMonitoring ? "Oui" : "Non");
            
            // URL de surveillance
            if (service.RequiresMonitoring)
            {
                AddRow(table, 5, "URL de surveillance:", service.MonitoringUrl ?? "Non définie");
            }
            
            // Nombre de produits associés
            AddRow(table, 6, "Produits associés:", service.AssociatedProducts.Count.ToString());
            
            // ID
            AddRow(table, 7, "ID:", service.Id);

            tab.Controls.Add(table);
        }

        private void SetupProductsTab(TabPage tab)
        {
            ListView listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = false,
                BackColor = Color.FromArgb(37, 37, 38),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            
            listView.Columns.Add("Nom", 200);
            listView.Columns.Add("Catégorie", 120);
            listView.Columns.Add("Version", 100);
            listView.Columns.Add("Fabricant", 150);
            listView.Columns.Add("Date d'expiration", 120);
            
            foreach (var productRef in service.AssociatedProducts)
            {
                Product product = productManager.GetProductById(productRef.Id);
                if (product != null)
                {
                    ListViewItem item = new ListViewItem(product.Name);
                    item.SubItems.Add(product.Category.ToString());
                    item.SubItems.Add(product.Version ?? "N/A");
                    item.SubItems.Add(product.Manufacturer ?? "N/A");
                    item.SubItems.Add(product.ExpirationDate.ToString("dd/MM/yyyy"));
                    
                    // Colorer les produits expirés
                    if (product.IsLicenseExpired())
                    {
                        item.ForeColor = Color.Red;
                    }
                    else if (product.DaysUntilExpiration() < 30)
                    {
                        item.ForeColor = Color.Orange;
                    }
                    
                    listView.Items.Add(item);
                }
            }
            
            if (listView.Items.Count == 0)
            {
                Label lblNoProducts = new Label
                {
                    Text = "Aucun produit associé à ce service",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Gray
                };
                tab.Controls.Add(lblNoProducts);
            }
            else
            {
                tab.Controls.Add(listView);
            }
        }

        private void SetupClientsTab(TabPage tab)
        {
            ListView listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = false,
                BackColor = Color.FromArgb(37, 37, 38),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            
            listView.Columns.Add("Nom", 200);
            listView.Columns.Add("Ville", 120);
            listView.Columns.Add("Contact", 150);
            listView.Columns.Add("Email", 200);
            listView.Columns.Add("Téléphone", 120);
            
            // Rechercher les clients qui ont souscrit à ce service
            var allClients = clientManager.GetAllClients();
            bool clientsFound = false;
            
            foreach (var client in allClients)
            {
                foreach (var subscribedService in client.SubscribedServices)
                {
                    if (subscribedService.Id == service.Id)
                    {
                        ListViewItem item = new ListViewItem(client.Name);
                        item.SubItems.Add(client.City);
                        item.SubItems.Add(client.ContactPerson);
                        item.SubItems.Add(client.Email);
                        item.SubItems.Add(client.Phone);
                        listView.Items.Add(item);
                        clientsFound = true;
                        break;
                    }
                }
            }
            
            if (!clientsFound)
            {
                Label lblNoClients = new Label
                {
                    Text = "Aucun client ne s'est abonné à ce service",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Gray
                };
                tab.Controls.Add(lblNoClients);
            }
            else
            {
                tab.Controls.Add(listView);
            }
        }

        private void SetupHistoryTab(TabPage tab)
        {
            // Cette fonction pourrait être implémentée pour afficher l'historique des statuts du service
            // Pour l'instant, on affiche juste un message
            Label lblNoHistory = new Label
            {
                Text = "L'historique des statuts n'est pas encore implémenté",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Gray
            };
            tab.Controls.Add(lblNoHistory);
        }

        private void AddRow(TableLayoutPanel table, int row, string label, string value)
        {
            Label lblName = new Label
            {
                Text = label,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                ForeColor = Color.Silver,
                Margin = new Padding(3)
            };
            
            Label lblValue = new Label
            {
                Text = value,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.White,
                Margin = new Padding(3)
            };
            
            table.Controls.Add(lblName, 0, row);
            table.Controls.Add(lblValue, 1, row);
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            ServiceEditForm form = new ServiceEditForm(service);
            if (form.ShowDialog() == DialogResult.OK)
            {
                // Recharger le service depuis le gestionnaire
                service = serviceManager.GetServiceById(service.Id);
                
                // Recréer le formulaire avec les nouvelles données
                this.Controls.Clear();
                LoadServiceDetails();
            }
        }

        private async void BtnCheckStatus_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Enabled = false;
            btn.Text = "Vérification en cours...";
            
            try
            {
                await serviceManager.CheckServiceStatusAsync(service.Id);
                
                // Recharger le service depuis le gestionnaire
                service = serviceManager.GetServiceById(service.Id);
                
                // Recréer le formulaire avec les nouvelles données
                this.Controls.Clear();
                LoadServiceDetails();
                
                LogManager.LogAction($"Statut du service vérifié: {service.Name} - {service.Status}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la vérification du statut: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Enabled = true;
                btn.Text = "Vérifier le statut";
            }
        }

        private Color GetStatusColor(ServiceStatus status)
        {
            switch (status)
            {
                case ServiceStatus.Running:
                    return Color.FromArgb(0, 170, 0); // Vert
                case ServiceStatus.Warning:
                    return Color.FromArgb(255, 170, 0); // Orange
                case ServiceStatus.Error:
                    return Color.FromArgb(220, 0, 0); // Rouge
                case ServiceStatus.Maintenance:
                    return Color.FromArgb(0, 120, 215); // Bleu
                case ServiceStatus.Inactive:
                    return Color.FromArgb(120, 120, 120); // Gris
                default:
                    return Color.Gray;
            }
        }

        private string GetStatusText(ServiceStatus status)
        {
            switch (status)
            {
                case ServiceStatus.Running:
                    return "En fonctionnement";
                case ServiceStatus.Warning:
                    return "Avertissement";
                case ServiceStatus.Error:
                    return "Erreur";
                case ServiceStatus.Maintenance:
                    return "En maintenance";
                case ServiceStatus.Inactive:
                    return "Inactif";
                default:
                    return "Inconnu";
            }
        }
    }
}