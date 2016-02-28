// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Reflection;

namespace IdentityServer4.Core.Resources
{
    public static class Messages
    {
        public static string GetString(string name)
        {
            return typeof(Messages).GetField(name)?.GetValue(null)?.ToString();
        }

        public const string ClientIdRequired = "Η ταυτοποίηση του πελάτη είναι απαραίτητη";
        public const string ExternalProviderError = "Υπήρξε σφάλμα κατά την είσοδο. Το σφάλμα είναι {0}";
        public const string invalid_request = "Η εφαρμογή του πελάτη δημιούργησε έναν άκυο αίτημα.";
        public const string invalid_scope = "Η εφαρμογή του πελάτη επιχείρησε να εισέλθει χωρίς να έχει την κατάλληλη έγκριση.";
        public const string InvalidUsernameOrPassword = "Άκυρο όνομα χρήστη ή κωδικός";
        public const string MissingClientId = "Λέιπει η ταυτότητα του πελάτη";
        public const string MissingToken = "Token λείπει";
        public const string NoExternalProvider = "Η εξωτερική είσοδος του παρόχου δεν ήταν ικανός να παράξει άδεια.";
        public const string NoMatchingExternalAccount = "Άκυρος λογαριασμός";
        public const string NoSignInCookie = "There is an error determining which application you are signing into.Return to the application and try again.";
        public const string NoSubjectFromExternalProvider = "Error authenticating with external provider";
        public const string PasswordRequired = "Ο κωδικός είναι απαραίτητος";
        public const string SslRequired = "SSL είναι απαραίτητο";
        public const string unauthorized_client = "The client application is not known or is not authorized.";
        public const string UnexpectedError = "Υπήρξε ένα άγνωστο σφάλμα";
        public const string unsupported_response_type = "The authorization server does not support the requested response type.";
        public const string UnsupportedMediaType = "Unsupported Media Type";
        public const string UsernameRequired = "Το όνομα χρήστη είναι απαραίτητο";
    }
}
