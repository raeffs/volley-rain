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
                return VerifyKey(password, user.Password, user.Salt);
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

        public override MembershipUser CreateUser(
            string username,
            string password,
            string email,
            string passwordQuestion,
            string passwordAnswer,
            bool isApproved,
            object providerUserKey,
            out MembershipCreateStatus status)
        {
            using (var db = new DatabaseContext())
            {
                if (db.Users.Any(u => u.Email == email))
                {
                    status = MembershipCreateStatus.DuplicateEmail;
                    return null;
                }

                string key;
                string salt;
                GenerateKey(password, out key, out salt);

                var user = new User
                {
                    Email = email,
                    Password = key,
                    Salt = salt,
                    Name = string.Empty,
                    Surname = string.Empty,
                    IsApproved = true,
                    IsLockedOut = false
                };
                user.Roles.Add(db.Roles.Single(r => r.IsDefaultUserRole));
                db.Users.Add(user);
                db.SaveChanges();

                status = MembershipCreateStatus.Success;
                return new CustomMembershipUser(user);
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            using (var db = new DatabaseContext())
            {
                if (db.Users.None(u => u.Email == username)) return false;

                var user = db.Users.Single(u => u.Email == username);

                string key;
                string salt;
                GenerateKey(newPassword, out key, out salt);

                user.Password = key;
                user.Salt = salt;
                db.SaveChanges();
                return true;
            }
        }

        public override string ResetPassword(string username, string answer)
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

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(System.Web.Security.MembershipUser user)
        {
            throw new NotImplementedException();
        }

        private void GenerateKey(string password, out string key, out string salt)
        {
            using (var derivedBytes = new Rfc2898DeriveBytes(password, 64))
            {
                salt = BitConverter.ToString(derivedBytes.Salt).Replace("-", "");
                key = BitConverter.ToString(derivedBytes.GetBytes(64)).Replace("-", "");
            }
        }

        private bool VerifyKey(string password, string key, string salt)
        {
            using (var derivedBytes = new Rfc2898DeriveBytes(password, StringToByteArray(salt)))
            {
                var keyToVerify = BitConverter.ToString(derivedBytes.GetBytes(64)).Replace("-", "");
                return key == keyToVerify;
            }
        }

        private byte[] StringToByteArray(string hexString)
        {
            int NumberChars = hexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            return bytes;
        }
    }
}