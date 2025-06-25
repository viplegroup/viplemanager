// Viple FilesVersion - ServiceEditForm 1.0.0 - Date 25/06/2025
// Application créée par Viple SAS

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VipleManagement.Models;
using VipleManagement.Services;

namespace VipleManagement.Forms.Services
{
    public partial class ServiceEditForm : Form
    {
        private ServiceManager serviceManager;
        private ProductManager productManager;
        private Service service;
        private bool isNewService;
        
        private TextBox txtName;
        private TextBox txtDescription;
        private ComboBox cmbCategory;
        private NumericUpDown nudMonthlyFee;
        private ComboBox cmbStatus;
        private CheckBox chkRequiresMonitoring;
        private TextBox txtMonitoringUrl;
        private ListBox lstSelectedProducts;
        private Button btnAddProduct;
        private Button btnRemoveProduct;
        private Button btnSave;
        private Button btnCancel;

        public ServiceEditForm(Service service = null)
        {
            this.service = service ?? new Service();
            isNewService = service == null;
            
            serviceManager = new ServiceManager();
            productManager = new ProductManager();
            
            InitializeComponent();
            SetupUI();
            PopulateForm();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(550, 580);
            this.Text = isNewService ? "Viple - Ajouter un service" : "Viple - Modifier un service";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;
            this.AutoScroll = true;
        }

        private void SetupUI()
        {
            // Titre principal
            Label lblTitle = new Label
            {
                Text = isNewService ? "Nouveau service" : "Modifier le service",
                Location = new Point(20, 20),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White
            };

            // Nom du service
            Label lblName = new Label
            {
                Text = "Nom du service:",
                Location = new Point(20, 60),
                Size = new Size(120, 20),
                ForeColor = Color.White
            };

            txtName = new TextBox
            {
                Location = new Point(160, 60),
                Size = new Size(350, 20),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Description
            Label lblDescription = new Label
            {
                Text = "Description:",
                Location = new Point(20, 90),
                Size = new Size(120, 20),
                ForeColor = Color.White
            };

            txtDescription = new TextBox
            {
                Location = new Point(160, 90),
                Size = new Size(350, 60),
                Multiline = true,
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Catégorie
            Label lblCategory = new Label
            {
                Text = "Catégorie:",
                Location = new Point(20, 160),
                Size = new Size(120, 20),
                ForeColor = Color.White
            };

            cmbCategory = new ComboBox
            {
                Location = new Point(160, 160),
                Size = new Size(200, 20),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White
            };

            foreach (ServiceCategory category in Enum.GetValues(typeof(ServiceCategory)))
            {
                cmbCategory.Items.Add(category);
            }

            // Tarif mensuel
            Label lblMonthlyFee = new Label
            {
                Text = "Tarif mensuel (€):",
                Location = new Point(20, 190),
                Size = new Size(120, 20),
                ForeColor = Color.White
            };

            nudMonthlyFee = new NumericUpDown
            {
                Location = new Point(160, 190),
                Size = new Size(100, 20),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                Minimum = 0,
                Maximum = 10000,
                DecimalPlaces = 2,
                Increment = 0.01m
            };

            // Statut
            Label lblStatus = new Label
            {
                Text = "Statut:",
                Location = new Point(20, 220),
                Size = new Size(120, 20),
                ForeColor = Color.White
            };

            cmbStatus = new ComboBox
            {
                Location = new Point(160, 220),
                Size = new Size(200, 20),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White
            };

            foreach (ServiceStatus status in Enum.GetValues(typeof(ServiceStatus)))
            {
                cmbStatus.Items.Add(status);
            }

            // Surveillance
            chkRequiresMonitoring = new CheckBox
            {
                Text = "Nécessite une surveillance",
                Location = new Point(160, 250),
                Size = new Size(200, 20),
                ForeColor = Color.White
            };
            chkRequiresMonitoring.CheckedChanged += ChkRequiresMonitoring_CheckedChanged;

            // URL de surveillance
            Label lblMonitoringUrl = new Label
            {
                Text = "URL de surveillance:",
                Location = new Point(20, 280),
                Size = new Size(120, 20),
                ForeColor = Color.White
            };

            txtMonitoringUrl = new TextBox
            {
                Location = new Point(160, 280),
                Size = new Size(350, 20),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Produits associés
            Label lblProducts = new Label
            {
                Text = "Produits associés:",
                Location = new Point(20, 320),
                Size = new Size(120, 20),
                ForeColor = Color.White
            };

            lstSelectedProducts = new ListBox
            {
                Location = new Point(160, 320),
                Size = new Size(350, 100),
                BackColor = Color.FromArgb(51, 51, 55),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                SelectionMode = SelectionMode.MultiSimple
            };

            // Boutons pour les produits
            btnAddProduct = new Button
            {
                Text = "Ajouter",
                Location = new Point(160, 430),
                Size = new Size(80, 25),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            btnAddProduct.Click += BtnAddProduct_Click;

            btnRemoveProduct = new Button
            {
                Text = "Supprimer",
                Location = new Point(250, 430),
                Size = new Size(80, 25),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(204, 0, 0),
                ForeColor = Color.White
            };
            btnRemoveProduct.Click += BtnRemoveProduct_Click;

            // Boutons d'action
            btnSave = new Button
            {
                Text = "Enregistrer",
                DialogResult = DialogResult.OK,
                Location = new Point(160, 490),
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
                Location = new Point(270, 490),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White
            };
            btnCancel.Click += BtnCancel_Click;

            // Ajouter les contrôles au formulaire
            this.Controls.AddRange(new Control[]
            {
                lblTitle,
                lblName, txtName,
                lblDescription, txtDescription,
                lblCategory, cmbCategory,
                lblMonthlyFee, nudMonthlyFee,
                lblStatus, cmbStatus,
                chkRequiresMonitoring,
                lblMonitoringUrl, txtMonitoringUrl,
                lblProducts, lstSelectedProducts,
                btnAddProduct, btnRemoveProduct,
                btnSave, btnCancel
            });
        }

        private void PopulateForm()
        {
            txtName.Text = service.Name;
            txtDescription.Text = service.Description;
            cmbCategory.SelectedItem = service.Category;
            nudMonthlyFee.Value = service.MonthlyFee;
            cmbStatus.SelectedItem = service.Status;
            chkRequiresMonitoring.Checked = service.RequiresMonitoring;
            txtMonitoringUrl.Text = service.MonitoringUrl;

            // Remplir la liste des produits associés
            LoadSelectedProducts();

            // État initial du champ URL de surveillance
            txtMonitoringUrl.Enabled = service.RequiresMonitoring;
        }

        private void LoadSelectedProducts()
        {
            lstSelectedProducts.Items.Clear();
            foreach (var product in service.AssociatedProducts)
            {
                Product fullProduct = productManager.GetProductById(product.Id);
                if (fullProduct != null)
                {
                    lstSelectedProducts.Items.Add(fullProduct);
                }
            }
        }

        private void ChkRequiresMonitoring_CheckedChanged(object sender, EventArgs e)
        {
            txtMonitoringUrl.Enabled = chkRequiresMonitoring.Checked;
        }

        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            ProductSelectorForm selector = new ProductSelectorForm(service.AssociatedProducts);
            if (selector.ShowDialog() == DialogResult.OK)
            {
                foreach (var product in selector.SelectedProducts)
                {
                    if (!ContainsProduct(service.AssociatedProducts, product))
                    {
                        service.AssociatedProducts.Add(product);
                    }
                }
                LoadSelectedProducts();
            }
        }

        private bool ContainsProduct(List<Product> products, Product product)
        {
            foreach (var p in products)
            {
                if (p.Id == product.Id)
                {
                    return true;
                }
            }
            return false;
        }

        private void BtnRemoveProduct_Click(object sender, EventArgs e)
        {
            if (lstSelectedProducts.SelectedItem != null)
            {
                Product product = (Product)lstSelectedProducts.SelectedItem;
                service.AssociatedProducts.RemoveAll(p => p.Id == product.Id);
                LoadSelectedProducts();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                service.Name = txtName.Text;
                service.Description = txtDescription.Text;
                service.Category = (ServiceCategory)cmbCategory.SelectedItem;
                service.MonthlyFee = nudMonthlyFee.Value;
                service.Status = (ServiceStatus)cmbStatus.SelectedItem;
                service.RequiresMonitoring = chkRequiresMonitoring.Checked;
                service.MonitoringUrl = txtMonitoringUrl.Text;

                bool success;
                if (isNewService)
                {
                    success = serviceManager.AddService(service);
                }
                else
                {
                    success = serviceManager.UpdateService(service);
                }

                if (success)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Une erreur s'est produite lors de l'enregistrement du service.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Le nom du service est obligatoire.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (cmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner une catégorie.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return false;
            }

            if (cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un statut.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbStatus.Focus();
                return false;
            }

            if (chkRequiresMonitoring.Checked && string.IsNullOrWhiteSpace(txtMonitoringUrl.Text))
            {
                MessageBox.Show("L'URL de surveillance est obligatoire si la surveillance est activée.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMonitoringUrl.Focus();
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

    // Formulaire de sélection des produits à associer au service
    public class ProductSelectorForm : Form
    {
        private ProductManager productManager;
        private ListView lvProducts;
        private Button btnOk;
        private Button btnCancel;
        private List<Product> existingProducts;

        public List<Product> SelectedProducts { get; private set; }

        public ProductSelectorForm(List<Product> existingProducts)
        {
            this.existingProducts = existingProducts ?? new List<Product>();
            SelectedProducts = new List<Product>();
            productManager = new ProductManager();
            InitializeComponent();
            LoadProducts();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(600, 400);
            this.Text = "Viple - Sélectionner des produits";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;

            // ListView des produits
            lvProducts = new ListView
            {
                Location = new Point(20, 20),
                Size = new Size(550, 300),
                CheckBoxes = true,
                View = View.Details,
                FullRowSelect = true,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            lvProducts.Columns.Add("Nom", 200);
            lvProducts.Columns.Add("Catégorie", 100);
            lvProducts.Columns.Add("Version", 80);
            lvProducts.Columns.Add("Fabricant", 120);

            // Boutons
            btnOk = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(390, 330),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            btnOk.Click += BtnOk_Click;

            btnCancel = new Button
            {
                Text = "Annuler",
                DialogResult = DialogResult.Cancel,
                Location = new Point(480, 330),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White
            };

            this.Controls.AddRange(new Control[] { lvProducts, btnOk, btnCancel });
        }

        private void LoadProducts()
        {
            lvProducts.Items.Clear();
            List<Product> products = productManager.GetAllProducts();

            foreach (var product in products)
            {
                ListViewItem item = new ListViewItem(product.Name);
                item.SubItems.Add(product.Category.ToString());
                item.SubItems.Add(product.Version ?? "N/A");
                item.SubItems.Add(product.Manufacturer ?? "N/A");
                item.Tag = product;
                
                // Vérifier si le produit est déjà associé
                bool alreadyAssociated = existingProducts.Any(p => p.Id == product.Id);
                item.Checked = alreadyAssociated;
                
                lvProducts.Items.Add(item);
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvProducts.CheckedItems)
            {
                Product product = (Product)item.Tag;
                SelectedProducts.Add(product);
            }
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}