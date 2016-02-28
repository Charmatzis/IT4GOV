// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Reflection;

namespace IdentityServer4.Core.Resources
{
    public static class Scopes
    {
        public static string GetString(string name)
        {
            return typeof(Scopes).GetField(name)?.GetValue(null)?.ToString();
        }

        public const string address_DisplayName = "Η διεύθυνση";
        public const string all_claims_DisplayName = "Όλες οι πληροφορίες του πελάτη";
        public const string email_DisplayName = "Το email";
        public const string offline_access_DisplayName = "Εκτός σύνδεσης είδοσο";
        public const string openid_DisplayName = "Το αναγνωριστικό του χρήστη";
        public const string phone_DisplayName   = "Το τηλέφωνο";
        public const string profile_Description = "Οι γενικές πληροφορίες του χρήστη (Όνομα, επώνυμο, πατρώνυμο)";
        public const string profile_DisplayName = "Το προφιλ του χρήστη";
        public const string roles_DisplayName = "Οι ρόλοι του χρήστη";
    }
}
