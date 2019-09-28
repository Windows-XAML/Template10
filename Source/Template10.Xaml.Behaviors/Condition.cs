using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Template10.Behaviors
{
    public sealed class Condition : DependencyObject
    {
        public enum Operators
        {
            EqualToRight = 0,
            NotEqualToRight = 1,
            LessThanRight = 2,
            LessThanOrEqualToRight = 3,
            GreaterThanRight = 4,
            GreaterThanOrEqualToRight = 5,
            IsNull = 6,
            IsNotNull = 7,
            IsTrue = 8,
            IsFalse = 9,
            IsNullOrEmpty = 10,
            IsNotNullOrEmpty = 11
        }

        public Operators Operator
        {
            get => (Operators)GetValue(OperatorProperty);
            set => SetValue(OperatorProperty, value);
        }
        public static readonly DependencyProperty OperatorProperty =
            DependencyProperty.Register(nameof(Operator), typeof(Operators),
                typeof(Condition), new PropertyMetadata(Operators.EqualToRight));

        public object LeftValue
        {
            get => GetValue(LeftValueProperty);
            set => SetValue(LeftValueProperty, value);
        }
        public static readonly DependencyProperty LeftValueProperty =
            DependencyProperty.Register(nameof(LeftValue), typeof(object),
                typeof(Condition), new PropertyMetadata(null));

        public object RightValue
        {
            get => GetValue(RightValueProperty);
            set => SetValue(RightValueProperty, value);
        }
        public static readonly DependencyProperty RightValueProperty =
            DependencyProperty.Register(nameof(RightValue), typeof(object),
                typeof(Condition), new PropertyMetadata(null));

        private int Compare<T>(T left, T right)
        {
            return Comparer<T>.Default.Compare(left, right);
        }

        public bool Execute()
        {
            var leftType = LeftValue?.GetType();
            var rightValue = (RightValue == null) ? null : Convert.ChangeType(RightValue, leftType);
            switch (Operator)
            {
                case Operators.EqualToRight: return Compare(LeftValue, rightValue) == 0;
                case Operators.NotEqualToRight: return Compare(LeftValue, rightValue) != 0;
                case Operators.LessThanRight: return Compare(LeftValue, rightValue) > 0;
                case Operators.LessThanOrEqualToRight: return Compare(LeftValue, rightValue) >= 0;
                case Operators.GreaterThanRight: return Compare(LeftValue, rightValue) < 0;
                case Operators.GreaterThanOrEqualToRight: return Compare(LeftValue, rightValue) <= 0;
                case Operators.IsNull: return LeftValue == null;
                case Operators.IsNotNull: return LeftValue != null;
                case Operators.IsTrue: return (bool?)LeftValue ?? false;
                case Operators.IsFalse: return !(bool?)LeftValue ?? false;
                case Operators.IsNullOrEmpty: return string.IsNullOrEmpty(LeftValue as string);
                case Operators.IsNotNullOrEmpty: return !string.IsNullOrEmpty(LeftValue as string);
                default: return Compare(LeftValue, rightValue) == 0;
            }
        }
    }
}
