// Viple FilesVersion - ServiceCatalogForm 1.0.0 - Date 25/06/2025
// Application créée par Viple SAS

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VipleManagement.Models;
using VipleManagement.Services;

namespace VipleManagement.Forms.Services
{
    public partial class ServiceCatalogForm : Form
    {
        private ServiceManager serviceManager;
        private ProductManager productManager;
        private ListView lvServices;
        private ComboBox cmbCategory;
        private Button btnAddService;
        private Button btnEditService;
        private Button btnDeleteService;
        private Button btnDetails;
        private Panel detailsPanel;

        public ServiceCatalogForm()
        {
            InitializeComponent();
            serviceManager = new ServiceManager();
            productManager = new ProductManager();
            SetupUI();
            LoadCategories();
            LoadServices();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1000, 600);
            this.Text = "Viple - Catalogue de services";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;
        }

        private void SetupUI()
        {
            // Panel supérieur avec filtres
            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(37, 37, 38)
            };

            Label lblCategory = new Label
            {
                Text = "Catégorie:",
                Location = new Point(15, 15),
                Size = new Size(80, 20),
                ForeColor = Color.White
            };

            cmbCategory = new ComboBox
            {
                Location = new Point(100, 13),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White
            };
            cmbCategory.SelectedIndexChanged += CmbCategory_SelectedIndexChanged;

            topPanel.Controls.AddRange(new Control[] { lblCategory, cmbCategory });

            // Création du SplitContainer principal
            SplitContainer splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 350,
                BackColor = Color.FromArgb(37, 37, 38)
            };

            // Panel gauche pour la liste des services
            Panel leftPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(37, 37, 38)
            };

            // ListView des services
            lvServices = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = false,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            lvServices.Columns.Add("Nom", 200);
            lvServices.Columns.Add("Catégorie", 120);
            lvServices.Columns.Add("Tarif mensuel", 100);
            lvServices.Columns.Add("Statut", 100);
            lvServices.SelectedIndexChanged += LvServices_SelectedIndexChanged;
            lvServices.DoubleClick += LvServices_DoubleClick;

            leftPanel.Controls.Add(lvServices);

            // Panel de boutons sous la liste
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(37, 37, 38)
            };

            btnAddService = new Button
            {
                Text = "Ajouter",
                Location = new Point(10, 10),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            btnAddService.Click += BtnAddService_Click;

            btnEditService = new Button
            {
                Text = "Modifier",
                Location = new Point(100, 10),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            btnEditService.Click += BtnEditService_Click;

            btnDeleteService = new Button
            {
                Text = "Supprimer",
                Location = new Point(190, 10),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(204, 0, 0),
                ForeColor = Color.White
            };
            btnDeleteService.Click += BtnDeleteService_Click;

            buttonPanel.Controls.AddRange(new Control[] { btnAddService, btnEditService, btnDeleteService });

            // Panel de détails à droite
            detailsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(37, 37, 38),
                Padding = new Padding(10)
            };

            Label lblDetailsTitle = new Label
            {
                Text = "Détails du service",
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Panel contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(30, 30, 30)
            };

            btnDetails = new Button
            {
                Text = "Voir les détails complets",
                Dock = DockStyle.Bottom,
                Height = 30,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            btnDetails.Click += BtnDetails_Click;

            detailsPanel.Controls.AddRange(new Control[] { lblDetailsTitle, contentPanel, btnDetails });

            // Assemblage final
            splitContainer.Panel1.Controls.Add(leftPanel);
            leftPanel.Controls.Add(buttonPanel);
            splitContainer.Panel2.Controls.Add(detailsPanel);

            this.Controls.AddRange(new Control[] { splitContainer, topPanel });
        }

        private void LoadCategories()
        {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("Toutes les catégories");
            
            foreach (ServiceCategory category in Enum.GetValues(typeof(ServiceCategory)))
            {
                cmbCategory.Items.Add(category);
            }
            
            cmbCategory.SelectedIndex = 0;
        }

        private void LoadServices()
        {
            lvServices.Items.Clear();
            
            List<Service> services;
            if (cmbCategory.SelectedIndex == 0) // "Toutes les catégories"
            {
                services = serviceManager.GetAllServices();
            }
            else
            {
                ServiceCategory selectedCategory = (ServiceCategory)cmbCategory.SelectedItem;
                services = serviceManager.GetServicesByCategory(selectedCategory);
            }

            foreach (var service in services)
            {
                ListViewItem item = new ListViewItem(service.Name);
                item.SubItems.Add(service.Category.ToString());
                item.SubItems.Add(service.MonthlyFee.ToString("C2"));
                item.SubItems.Add(GetStatusText(service.Status));
                item.Tag = service.Id;
                
                // Colorer selon le statut
                switch (service.Status)
                {
                    case ServiceStatus.Running:
                        item.ForeColor = Color.LightGreen;
                        break;
                    case ServiceStatus.Warning:
                        item.ForeColor = Color.Orange;
                        break;
                    case ServiceStatus.Error:
                        item.ForeColor = Color.Red;
                        break;
                    case ServiceStatus.Maintenance:
                        item.ForeColor = Color.LightBlue;
                        break;
                    default:
                        item.ForeColor = Color.Gray;
                        break;
                }

                lvServices.Items.Add(item);
            }

            // Redimensionner les colonnes
            lvServices.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            ClearDetails();
            LogManager.LogAction("Catalogue de services chargé");
        }

        private void DisplayServiceDetails(string serviceId)
        {
            ClearDetails();
            
            Service service = serviceManager.GetServiceById(serviceId);
            if (service != null)
            {
                Panel contentPanel = (Panel)detailsPanel.Controls[1]; // Le panel de contenu
                
                // Informations du service
                TableLayoutPanel infoTable = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 10,
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                    BackColor = Color.FromArgb(30, 30, 30)
                };
                
                infoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                infoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

                // Nom
                AddDetailRow(infoTable, 0, "Nom:", service.Name);
                
                // Description
                AddDetailRow(infoTable, 1, "Description:", service.Description);
                
                // Catégorie
                AddDetailRow(infoTable, 2, "Catégorie:", service.Category.ToString());
                
                // Tarif mensuel
                AddDetailRow(infoTable, 3, "Tarif mensuel:", service.MonthlyFee.ToString("C2"));
                
                // Statut
                AddDetailRow(infoTable, 4, "Statut:", GetStatusText(service.Status));
                
                // Date de création
                AddDetailRow(infoTable, 5, "Créé le:", service.CreationDate.ToString("dd/MM/yyyy"));
                
                // Dernière vérification
                AddDetailRow(infoTable, 6, "Dernière vérification:", service.LastChecked.ToString("dd/MM/yyyy HH:mm:ss"));
                
                // Message de statut
                AddDetailRow(infoTable, 7, "Message:", service.LastStatusMessage ?? "");
                
                // Surveillance requise
                AddDetailRow(infoTable, 8, "Surveillance:", service.RequiresMonitoring ? "Oui" : "Non");
                
                // URL de surveillance
                if (service.RequiresMonitoring)
                {
                    AddDetailRow(infoTable, 9, "URL de surveillance:", service.MonitoringUrl ?? "");
                }
                
                // Produits associés
                if (service.AssociatedProducts.Count > 0)
                {
                    Label lblProducts = new Label
                    {
                        Text = "Produits associés:",
                        ForeColor = Color.White,
                        Dock = DockStyle.Top,
                        Height = 20
                    };
                    
                    ListView lvProducts = new ListView
                    {
                        View = View.Details,
                        FullRowSelect = true,
                        Height = 100,
                        Dock = DockStyle.Bottom,
                        BackColor = Color.FromArgb(37, 37, 38),
                        ForeColor = Color.White,
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    
                    lvProducts.Columns.Add("Nom", 150);
                    lvProducts.Columns.Add("Catégorie", 100);
                    lvProducts.Columns.Add("Version", 80);
                    
                    foreach (var productId in service.AssociatedProducts)
                    {
                        Product product = productManager.GetProductById(productId.Id);
                        if (product != null)
                        {
                            ListViewItem item = new ListViewItem(product.Name);
                            item.SubItems.Add(product.Category.ToString());
                            item.SubItems.Add(product.Version ?? "N/A");
                            lvProducts.Items.Add(item);
                        }
                    }
                    
                    contentPanel.Controls.Add(lvProducts);
                    contentPanel.Controls.Add(lblProducts);
                }
                
                contentPanel.Controls.Add(infoTable);
            }
        }

        private void AddDetailRow(TableLayoutPanel table, int row, string label, string value)
        {
            Label lblName = new Label
            {
                Text = label,
                ForeColor = Color.Silver,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Margin = new Padding(3)
            };
            
            Label lblValue = new Label
            {
                Text = value,
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(3)
            };
            
            table.Controls.Add(lblName, 0, row);
            table.Controls.Add(lblValue, 1, row);
        }

        private void ClearDetails()
        {
            Panel contentPanel = (Panel)detailsPanel.Controls[1];
            contentPanel.Controls.Clear();
            
            Label lblNoSelection = new Label
            {
                Text = "Sélectionnez un service pour afficher ses détails",
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            
            contentPanel.Controls.Add(lblNoSelection);
            btnDetails.Enabled = false;
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

        private void CmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadServices();
        }

        private void LvServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvServices.SelectedItems.Count > 0)
            {
                string serviceId = lvServices.SelectedItems[0].Tag.ToString();
                DisplayServiceDetails(serviceId);
                btnDetails.Enabled = true;
            }
            else
            {
                ClearDetails();
            }
        }

        private void BtnAddService_Click(object sender, EventArgs e)
        {
            ServiceEditForm form = new ServiceEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadServices();
            }
        }

        private void BtnEditService_Click(object sender, EventArgs e)
        {
            if (lvServices.SelectedItems.Count > 0)
            {
                string serviceId = lvServices.SelectedItems[0].Tag.ToString();
                Service service = serviceManager.GetServiceById(serviceId);
                
                if (service != null)
                {
                    ServiceEditForm form = new ServiceEditForm(service);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadServices();
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un service.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDeleteService_Click(object sender, EventArgs e)
        {
            if (lvServices.SelectedItems.Count > 0)
            {
                string serviceId = lvServices.SelectedItems[0].Tag.ToString();
                string serviceName = lvServices.SelectedItems[0].Text;

                DialogResult result = MessageBox.Show(
                    $"Êtes-vous sûr de vouloir supprimer le service '{serviceName}' ?",
                    "Confirmation de suppression",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    if (serviceManager.DeleteService(serviceId))
                    {
                        LoadServices();
                    }
                    else
                    {
                        MessageBox.Show("Échec de la suppression du service.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un service.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LvServices_DoubleClick(object sender, EventArgs e)
        {
            BtnEditService_Click(sender, e);
        }

        private void BtnDetails_Click(object sender, EventArgs e)
        {
            if (lvServices.SelectedItems.Count > 0)
            {
                string serviceId = lvServices.SelectedItems[0].Tag.ToString();
                Service service = serviceManager.GetServiceById(serviceId);
                
                if (service != null)
                {
                    ServiceDetailForm form = new ServiceDetailForm(service);
                    form.ShowDialog();
                }
            }
        }
    }
}