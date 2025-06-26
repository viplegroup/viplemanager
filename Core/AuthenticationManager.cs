// Viple FilesVersion - AuthenticationManager 1.0.0 - Date 26/06/2025 01:50
// Application créée par Viple SAS

using System;
using VipleManagement.Models;
using VipleManagement.Services;

namespace VipleManagement.Core
{
    /// <summary>
    /// Gestionnaire d'authentification
    /// </summary>
    public static class AuthenticationManager
    {
        private static UserManager userManager;
        
        /// <summary>
        /// Utilisateur actuellement connecté
        /// </summary>
        public static User CurrentUser { get; private set; }
        
        /// <summary>
        /// Indique si un utilisateur est connecté
        /// </summary>
        public static bool IsLoggedIn => CurrentUser != null;
        
        /// <summary>
        /// Événement déclenché lors de la connexion d'un utilisateur
        /// </summary>
        public static event EventHandler<User> UserLoggedIn;
        
        /// <summary>
        /// Événement déclenché lors de la déconnexion d'un utilisateur
        /// </summary>
        public static event EventHandler UserLoggedOut;
        
        /// <summary>
        /// Initialiser le gestionnaire d'authentification
        /// </summary>
        static AuthenticationManager()
        {
            userManager = new UserManager();
        }
        
        /// <summary>
        /// Authentifier un utilisateur
        /// </summary>
        public static bool Login(string username, string password)
        {
            // Authentifier l'utilisateur
            if (userManager.AuthenticateUser(username, password))
            {
                // Récupérer l'utilisateur
                CurrentUser = userManager.GetUserByUsername(username);
                
                // Déclencher l'événement
                UserLoggedIn?.Invoke(null, CurrentUser);
                
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Déconnecter l'utilisateur actuel
        /// </summary>
        public static void Logout()
        {
            if (CurrentUser != null)
            {
                userManager.LogoutUser(CurrentUser.Id);
                
                // Déconnecter l'utilisateur
                CurrentUser = null;
                
                // Déclencher l'événement
                UserLoggedOut?.Invoke(null, EventArgs.Empty);
            }
        }
        
        /// <summary>
        /// Vérifier si l'utilisateur courant a un certain rôle
        /// </summary>
        public static bool HasRole(UserRole role)
        {
            return CurrentUser != null && CurrentUser.Role == role;
        }
        
        /// <summary>
        /// Vérifier si l'utilisateur courant peut gérer les clients
        /// </summary>
        public static bool CanManageClients()
        {
            return CurrentUser != null && CurrentUser.CanManageClients();
        }
        
        /// <summary>
        /// Vérifier si l'utilisateur courant peut gérer les services
        /// </summary>
        public static bool CanManageServices()
        {
            return CurrentUser != null && CurrentUser.CanManageServices();
        }
        
        /// <summary>
        /// Vérifier si l'utilisateur courant peut gérer les produits
        /// </summary>
        public static bool CanManageProducts()
        {
            return CurrentUser != null && CurrentUser.CanManageProducts();
        }
        
        /// <summary>
        /// Vérifier si l'utilisateur courant peut gérer les utilisateurs
        /// </summary>
        public static bool CanManageUsers()
        {
            return CurrentUser != null && CurrentUser.CanManageUsers();
        }
        
        /// <summary>
        /// Journaliser une action de l'utilisateur courant
        /// </summary>
        public static void LogUserAction(string actionType, string description, string entityId = "", string entityType = "")
        {
            if (CurrentUser != null)
            {
                userManager.LogUserAction(actionType, description, entityId, entityType, CurrentUser.Id);
            }
        }
    }
}