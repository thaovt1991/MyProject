using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System;

namespace HomemadeCakes.Common
{
    public abstract class NotificationBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected void OnPropertyChanged(Expression<Func<object>> expression)
        {
            string propertyName = GetPropertyName(expression);
            OnPropertyChanged(propertyName);
        }

        private string GetPropertyName(Expression<Func<object>> action)
        {
            Expression body = action.Body;
            if (body.NodeType == ExpressionType.Convert)
            {
                return ((MemberExpression)((UnaryExpression)body).Operand).Member.Name;
            }

            return ((MemberExpression)body).Member.Name;
        }
    }

}
