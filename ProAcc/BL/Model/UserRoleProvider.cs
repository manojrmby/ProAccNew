using ProACC_DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ProAcc.BL;
using System.Web.Security;

namespace ProAcc.BL.Model
{
    public class UserRoleProvider : RoleProvider
    {
        private ProAccEntities db = new ProAccEntities();
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            DBHelper dB = new DBHelper("SP_Role", CommandType.StoredProcedure);
            dB.addIn("@UserName", username);
            string userRoles = (dB.ExecuteScalar()).ToString();

            return new string[] { userRoles };
            //throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}