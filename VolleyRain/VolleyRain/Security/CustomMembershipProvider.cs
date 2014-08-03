using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using VolleyRain.DataAccess;
using VolleyRain.Models;

namespace VolleyRain.Security
{
    public class CustomMembershipProvider : MembershipProvider
    {
        private int _cacheTimeoutInMinutes = 30;

        public override void Initialize(string name, NameValueCollection config)
        {
            int value;
            if (!string.IsNullOrEmpty(config["cacheTimeoutInMinutes"]) && int.TryParse(config["cacheTimeoutInMinutes"], out value))
            {
                _cacheTimeoutInMinutes = value;
            }

            base.Initialize(name, config);
        }

        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return false;

            using (var db = new DatabaseContext())
            {
                var user = db.Users.SingleOrDefault(u => u.Email == username);
                if (user == null) return false;

                using (var derivedBytes = new Rfc2898DeriveBytes(password, StringToByteArray(user.Salt)))
                {
                    var key = BitConverter.ToString(derivedBytes.GetBytes(64)).Replace("-", "");
                    return user.Password == key;
                }
            }
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var cacheKey = string.Format("UserData_{0}", username);
            if (HttpRuntime.Cache[cacheKey] != null) return (CustomMembershipUser)HttpRuntime.Cache[cacheKey];

            using (var db = new DatabaseContext())
            {
                // TODO: add is approved and is locked out checks
                var user = db.Users.SingleOrDefault(u => u.Email == username);
                if (user == null) return null;

                var membershipUser = new CustomMembershipUser(user);
                HttpRuntime.Cache.Insert(cacheKey, membershipUser, null, DateTime.Now.AddMinutes(_cacheTimeoutInMinutes), Cache.NoSlidingExpiration);

                return membershipUser;
            }
        }

        public byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out System.Web.Security.MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override System.Web.Security.MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override System.Web.Security.MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override System.Web.Security.MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(System.Web.Security.MembershipUser user)
        {
            throw new NotImplementedException();
        }
    }
}