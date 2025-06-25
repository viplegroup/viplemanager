// Viple FilesVersion - ServicesStatusForm 1.0.0 - Date 25/06/2025
// Application créée par Viple SAS

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using VipleManagement.Models;
using VipleManagement.Services;

namespace VipleManagement.Forms.Services
{
    public partial class ServicesStatusForm : Form
    {
        private ServiceManager serviceManager;
        private System.Windows.Forms.Timer refreshTimer;
        private FlowLayoutPanel servicesPanel;
        private Button btnRefresh;
        private Button btnCheckAll;
        private ComboBox cmbCategoryFilter;

        public ServicesStatusForm()
        {
            InitializeComponent();
            serviceManager = new ServiceManager();
            SetupUI();
            LoadServices();
            InitializeRefreshTimer();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1000, 600);
            this.Text = "Viple - État des services";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;
        }

        private void SetupUI()
        {
            // Panel de filtres et d'actions
            Panel filterPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(37, 37, 38)
            };

            Label lblCategory = new Label
            {
                Text = "Catégorie:",
                ForeColor = Color.White,
                Location = new Point(10, 15),
                Size = new Size(70, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            cmbCategoryFilter = new ComboBox
            {
                Location = new Point(85, 14),
                Size = new Size(150, 25),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategoryFilter.Items.Add("Toutes les catégories");
            foreach (ServiceCategory category in Enum.GetValues(typeof(ServiceCategory)))
            {
                cmbCategoryFilter.Items.Add(category);
            }
            cmbCategoryFilter.SelectedIndex = 0;
            cmbCategoryFilter.SelectedIndexChanged += CmbCategoryFilter_SelectedIndexChanged;

            btnRefresh = new Button
            {
                Text = "Actualiser",
                Location = new Point(700, 10),
                Size = new Size(120, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            btnRefresh.Click += BtnRefresh_Click;

            btnCheckAll = new Button
            {
                Text = "Vérifier tous",
                Location = new Point(830, 10),
                Size = new Size(120, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            btnCheckAll.Click += BtnCheckAll_Click;

            filterPanel.Controls.AddRange(new Control[] { lblCategory, cmbCategoryFilter, btnRefresh, btnCheckAll });

            // Panel principal de contenus avec scroll
            Panel mainContainer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(30, 30, 30)
            };

            // FlowLayoutPanel pour afficher les services
            servicesPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = Color.FromArgb(30, 30, 30),
                Padding = new Padding(10)
            };

            mainContainer.Controls.Add(servicesPanel);
            this.Controls.AddRange(new Control[] { mainContainer, filterPanel });
        }

        private void LoadServices()
        {
            servicesPanel.Controls.Clear();
            
            List<Service> services;
            if (cmbCategoryFilter.SelectedIndex == 0) // "Toutes les catégories"
            {
                services = serviceManager.GetAllServices();
            }
            else
            {
                ServiceCategory selectedCategory = (ServiceCategory)cmbCategoryFilter.SelectedItem;
                services = serviceManager.GetServicesByCategory(selectedCategory);
            }

            foreach (var service in services)
            {
                Panel serviceCard = CreateServiceCard(service);
                servicesPanel.Controls.Add(serviceCard);
            }

            LogManager.LogAction("État des services chargé");
        }

        private Panel CreateServiceCard(Service service)
        {
            Panel card = new Panel
            {
                Size = new Size(300, 180),
                Margin = new Padding(10),
                BackColor = Color.FromArgb(37, 37, 38),
                BorderStyle = BorderStyle.None
            };

            // Border personnalisée
            card.Paint += (s, e) => {
                var borderColor = GetStatusColor(service.Status);
                using (var pen = new Pen(borderColor, 2))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            // Titre du service
            Label lblName = new Label
            {
                Text = service.Name,
                Location = new Point(10, 10),
                Size = new Size(280, 25),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            // Catégorie
            Label lblCategory = new Label
            {
                Text = $"Catégorie: {service.Category}",
                Location = new Point(10, 40),
                Size = new Size(280, 20),
                ForeColor = Color.Silver,
                Font = new Font("Segoe UI", 9)
            };

            // Statut
            Panel statusPanel = new Panel
            {
                Location = new Point(10, 65),
                Size = new Size(280, 30),
                BackColor = Color.FromArgb(45, 45, 48)
            };

            Panel statusIndicator = new Panel
            {
                Location = new Point(10, 5),
                Size = new Size(20, 20),
                BackColor = GetStatusColor(service.Status)
            };

            Label lblStatus = new Label
            {
                Text = $"Statut: {GetStatusText(service.Status)}",
                Location = new Point(40, 5),
                Size = new Size(230, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9)
            };

            statusPanel.Controls.AddRange(new Control[] { statusIndicator, lblStatus });

            // Dernière vérification
            Label lblLastCheck = new Label
            {
                Text = $"Dernière vérification: {service.LastChecked:dd/MM/yyyy HH:mm:ss}",
                Location = new Point(10, 100),
                Size = new Size(280, 20),
                ForeColor = Color.Silver,
                Font = new Font("Segoe UI", 8)
            };

            // Message de statut
            Label lblStatusMessage = new Label
            {
                Text = service.LastStatusMessage ?? "Aucune information",
                Location = new Point(10, 120),
                Size = new Size(280, 20),
                ForeColor = Color.Silver,
                Font = new Font("Segoe UI", 8)
            };

            // Bouton de vérification
            Button btnCheck = new Button
            {
                Text = "Vérifier",
                Location = new Point(210, 145),
                Size = new Size(80, 25),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Tag = service.Id
            };
            btnCheck.Click += async (s, e) => await CheckService((string)((Button)s).Tag, card);

            card.Controls.AddRange(new Control[] { lblName, lblCategory, statusPanel, lblLastCheck, lblStatusMessage, btnCheck });

            return card;
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

        private async Task CheckService(string serviceId, Panel card)
        {
            try
            {
                Button btnCheck = null;
                foreach (Control control in card.Controls)
                {
                    if (control is Button button && button.Text == "Vérifier")
                    {
                        btnCheck = button;
                        break;
                    }
                }

                if (btnCheck != null)
                {
                    btnCheck.Enabled = false;
                    btnCheck.Text = "Vérification...";
                }

                bool status = await serviceManager.CheckServiceStatusAsync(serviceId);
                
                // Recharger uniquement le service concerné
                Service updatedService = serviceManager.GetServiceById(serviceId);
                if (updatedService != null)
                {
                    int index = servicesPanel.Controls.IndexOf(card);
                    if (index >= 0)
                    {
                        servicesPanel.Controls.RemoveAt(index);
                        Panel updatedCard = CreateServiceCard(updatedService);
                        servicesPanel.Controls.Add(updatedCard);
                        servicesPanel.Controls.SetChildIndex(updatedCard, index);
                    }
                }

                LogManager.LogAction($"Service vérifié: {serviceId} - Statut: {status}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la vérification du service: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadServices();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadServices();
        }

        private async void BtnCheckAll_Click(object sender, EventArgs e)
        {
            btnCheckAll.Enabled = false;
            btnCheckAll.Text = "Vérification en cours...";
            btnRefresh.Enabled = false;
            
            try
            {
                await serviceManager.CheckAllServicesStatusAsync();
                LoadServices();
                
                LogManager.LogAction("Tous les services ont été vérifiés");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la vérification des services: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCheckAll.Enabled = true;
                btnCheckAll.Text = "Vérifier tous";
                btnRefresh.Enabled = true;
            }
        }

        private void InitializeRefreshTimer()
        {
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 5 * 60 * 1000; // 