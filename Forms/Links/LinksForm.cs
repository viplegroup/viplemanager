// Viple FilesVersion - LinksForm 1.0.0 - Date 25/06/2025
// Application créée par Viple SAS

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace VipleManagement.Forms.Links
{
    public partial class LinksForm : Form
    {
        private ListView lvLinks;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnOpen;
        private WebBrowser webPreview;
        private List<LinkItem> links;
        private string linksFilePath = Path.Combine("vipledata", "links.vff");

        public LinksForm()
        {
            InitializeComponent();
            SetupUI();
            LoadLinks();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(900, 600);
            this.Text = "Viple - Liens utiles";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.ForeColor = Color.White;
        }

        private void SetupUI()
        {
            // SplitContainer principal
            SplitContainer splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 300,
                BackColor = Color.FromArgb(37, 37, 38)
            };

            // Panel gauche : liste des liens
            Panel leftPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(37, 37, 38)
            };

            lvLinks = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = false,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            lvLinks.Columns.Add("Nom", 180);
            lvLinks.Columns.Add("Catégorie", 100);
            lvLinks.SelectedIndexChanged += LvLinks_SelectedIndexChanged;
            lvLinks.DoubleClick += LvLinks_DoubleClick;

            // Panel de boutons sous la liste
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(37, 37, 38)
            };

            btnAdd = new Button
            {
                Text = "Ajouter",
                Location = new Point(10, 10),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            btnAdd.Click += BtnAdd_Click;

            btnEdit = new Button
            {
                Text = "Modifier",
                Location = new Point(100, 10),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            btnEdit.Click += BtnEdit_Click;

            btnDelete = new Button
            {
                Text = "Supprimer",
                Location = new Point(190, 10),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(204, 0, 0),
                ForeColor = Color.White
            };
            btnDelete.Click += BtnDelete_Click;

            buttonPanel.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete });
            leftPanel.Controls.AddRange(new Control[] { lvLinks, buttonPanel });

            // Panel droit : aperçu et détails du lien
            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(37, 37, 38),
                Padding = new Padding(10)
            };

            Label lblPreview = new Label
            {
                Text = "Aperçu du lien",
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White
            };

            webPreview = new WebBrowser
            {
                Dock = DockStyle.Fill,
                ScriptErrorsSuppressed = true
            };

            btnOpen = new Button
            {
                Text = "Ouvrir dans le navigateur",
                Dock = DockStyle.Bottom,
                Height = 30,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White
            };
            btnOpen.Click += BtnOpen_Click;

            rightPanel.Controls.AddRange(new Control[] { lblPreview, webPreview, btnOpen });

            // Assemblage final
            splitContainer.Panel1.Controls.Add(leftPanel);
            splitContainer.Panel2.Controls.Add(rightPanel);
            this.Controls.Add(splitContainer);
        }

        private void LoadLinks()
        {
            links = new List<LinkItem>();
            
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(linksFilePath));
                
                if (File.Exists(linksFilePath))
                {
                    using (StreamReader reader = new StreamReader(linksFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<LinkItem>));
                        links = (List<LinkItem>)serializer.Deserialize(reader);
                    }
                }
                else
                {
                    // Créer des liens par défaut
                    links.Add(new LinkItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Site Viple",
                        Url = "https://www.viple.fr",
                        Category = "Entreprise",
                        Description = "Site officiel de Viple SAS"
                    });

                    links.Add(new LinkItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Documentation technique",
                        Url = "https://docs.viple.fr",
                        Category = "Support",
                        Description = "Documentation technique des services Viple"
                    });

                    links.Add(new LinkItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Portail client",
                        Url = "https://client.viple.fr",
                        Category = "Clientèle",
                        Description = "Portail d'accès pour les clients Viple"
                    });

                    SaveLinks();
                }

                DisplayLinks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des liens: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveLinks()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(linksFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<LinkItem>));
                    serializer.Serialize(writer, links);
                }

                UpdateFilesLog("links.vff", "modifiée");
                LogManager.LogAction("Liens enregistrés");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement des liens: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayLinks()
        {
            lvLinks.Items.Clear();
            
            foreach (var link in links)
            {
                ListViewItem item = new ListViewItem(link.Name);
                item.SubItems.Add(link.Category);
                item.Tag = link;
                lvLinks.Items.Add(item);
            }

            // Désactiver les boutons si aucun lien n'est sélectionné
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnOpen.Enabled = false;
            
            // Effacer la prévisualisation
            webPreview.DocumentText = "<html><body style='background-color:#1E1E1E; color:white; font-family:Segoe UI; text-align:center; padding-top:50px;'><h2>Sélectionnez un lien pour afficher un aperçu</h2></body></html>";
        }

        private void LvLinks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvLinks.SelectedItems.Count > 0)
            {
                LinkItem link = (LinkItem)lvLinks.SelectedItems[0].Tag;
                
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnOpen.Enabled = true;
                
                