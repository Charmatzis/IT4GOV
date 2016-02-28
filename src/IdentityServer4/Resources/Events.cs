// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Reflection;

namespace IdentityServer4.Core.Resources
{
    public static class Events
    {
        public static string GetString(string name)
        {
            return typeof(Events).GetField(name)?.GetValue(null)?.ToString();
        }

        public const string ClientPermissionsRevoked = "Τα δικαιώματα του πελάτη έχουν ανακληθεί";
        public const string CspReport = "Έκθεση Πολιτικής Ασφάλειας Περιεχομένου (CSP)";
        public const string ExternalLoginError = "Σφάλμα εξωτερικής σύνδεσης";
        public const string ExternalLoginFailure = "Αποτυχία εξωτερικής σύνδεσης";
        public const string ExternalLoginSuccess = "Επιτυχία εξωτερικής σύνδεσης";
        public const string LocalLoginFailure = "Αποτυχία εσωτερίκής σύνδεσης";
        public const string LocalLoginSuccess = "Αποτυχία εσωτερικής σύνδεσης";
        public const string LogoutEvent = "Αποτυχία ενέργιας";
        public const string PartialLogin = "Μερικής σύνδεσης";
        public const string PartialLoginComplete = "Η μερική είσοδος πέτυχε";
        public const string PreLoginFailure = "Αποτυχία προ-εισόδου";
        public const string PreLoginSuccess = "Επιτύχια προ-εισόδου";
        public const string ResourceOwnerFlowLoginFailure = "Resource Owner Password Flow Login Failure";
        public const string ResourceOwnerFlowLoginSuccess = "Resource Owner Password Flow Login Success";
    }
}
