using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.SessionState;

namespace VolleyRain.Models
{
    public class SessionDecorator
    {
        private readonly HttpSessionStateBase _context;

        public SessionDecorator(HttpSessionStateBase context)
        {
            _context = context;
        }

        public bool IsInitialized
        {
            get { return GetValue(() => IsInitialized); }
            set { SetValue(() => IsInitialized, value); }
        }

        public int UserID
        {
            get { return GetValue(() => UserID); }
            set { SetValue(() => UserID, value); }
        }

        public string UserName
        {
            get { return GetValue(() => UserName); }
            set { SetValue(() => UserName, value); }
        }

        public ICollection<int> Teams
        {
            get { return GetValue(() => Teams, new List<int>()); }
            set { SetValue(() => Teams, value); }
        }

        public int? SelectedTeamID
        {
            get { return GetValue(() => SelectedTeamID); }
            set { SetValue(() => SelectedTeamID, value); }
        }

        public bool ShowLogLevelFatal
        {
            get { return GetValue(() => ShowLogLevelFatal, true); }
            set { SetValue(() => ShowLogLevelFatal, value); }
        }

        public bool ShowLogLevelError
        {
            get { return GetValue(() => ShowLogLevelError, true); }
            set { SetValue(() => ShowLogLevelError, value); }
        }

        public bool ShowLogLevelWarn
        {
            get { return GetValue(() => ShowLogLevelWarn, true); }
            set { SetValue(() => ShowLogLevelWarn, value); }
        }

        public bool ShowLogLevelInfo
        {
            get { return GetValue(() => ShowLogLevelInfo, true); }
            set { SetValue(() => ShowLogLevelInfo, value); }
        }

        public bool ShowLogLevelDebug
        {
            get { return GetValue(() => ShowLogLevelDebug, false); }
            set { SetValue(() => ShowLogLevelDebug, value); }
        }

        public bool ShowLogLevelTrace
        {
            get { return GetValue(() => ShowLogLevelTrace, false); }
            set { SetValue(() => ShowLogLevelTrace, value); }
        }

        public void Clear()
        {
            _context.Clear();
        }

        private void SetValue<T>(Expression<Func<T>> property, T value)
        {
            var lambdaExpression = property as LambdaExpression;
            if (lambdaExpression == null) throw new ArgumentException("Invalid lambda expression", "property");

            var propertyName = GetPropertyName(lambdaExpression);
            _context[propertyName] = value;
        }

        private T GetValue<T>(Expression<Func<T>> property, T defaultValue = default(T))
        {
            var lambdaExpression = property as LambdaExpression;
            if (lambdaExpression == null) throw new ArgumentException("Invalid lambda expression", "property");

            var propertyName = GetPropertyName(lambdaExpression);
            try
            {
                return (T)_context[propertyName];
            }
            catch
            {
                return defaultValue;
            }
        }

        private string GetPropertyName(LambdaExpression lambdaExpression)
        {
            MemberExpression memberExpression;
            if (lambdaExpression.Body is UnaryExpression)
            {
                var unaryExpression = lambdaExpression.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = lambdaExpression.Body as MemberExpression;
            }
            return memberExpression.Member.Name;
        }
    }
}