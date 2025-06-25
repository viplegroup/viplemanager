// Viple FilesVersion - Product 1.0.0 - Date 25/06/2025
// Application créée par Viple SAS

using System;

namespace VipleManagement.Models
{
    [Serializable]
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public ProductCategory Category { get; set; }
        public string Manufacturer { get; set; }
        public string LicenseKey { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }

        public Product()
        {
            Id = Guid.NewGuid().ToString();
            PurchaseDate = DateTime.Now;
            ExpirationDate = DateTime.Now.AddYears(1);
        }

        public bool IsLicenseExpired()
        {
            return DateTime.Now > ExpirationDate;
        }

        public int DaysUntilExpiration()
        {
            if (IsLicenseExpired()) return 0;
            return (ExpirationDate - DateTime.Now).Days;
        }
    }

    public enum ProductCategory
    {
        Software,
        Hardware,
        License,
        VirtualMachine,
        CloudService,
        NetworkEquipment,
        SecuritySolution,
        Other
    }
}