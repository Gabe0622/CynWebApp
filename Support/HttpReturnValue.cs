using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CynthiasWebApp.Support
{
    public class HttpReturnValue
    {
        public const string AdminAccount202 = "Successfully created an admin account";
        public const string AdminAccount401 = "You don't have admin rights.";
        public const string IncorrectPassword = "The password you entered is incorrect.";

        public const string ClientAccountExists = "The email you entered is associated with an existing account.Please sign in.";
        public const string ClientAccountCreated = "Successfully created client account";
        public const string ClientNotFound = "Unable to locate client in database.";
        public const string ClientDeleted = "Successfully deleted client.";
        public const string ClientEditedProfile = "Successfully edited profile.";
        public const string ClientCompletedService = "Successfully added completed service to database.";
        public const string ClientMadeAdminRequest = "Successfuly requested admin rights.";
        public const string ClientAddedRequest = "Successfully added request.";

        public const string AdminDeniedRequest = "Admin did not approve your request.";
        public const string AdminApprovedRequest = "Admin approved your request!";
        public const string NoClientRequests = "There are no client requests.";

        public const string ErrorMessage = "Something went wrong.  Please try again.";
        


        
    }
}
