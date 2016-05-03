using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Behaviors
{
    public class FocusAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
         var ui = (TargetObject == null ? sender : TargetObject) as Control;
         if (ui != null)
            ui.Focus(FocusState.Programmatic);
         return null;
        }

      /// 
      /// Backing storage for the Target property
      /// 
      public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
          "TargetObject",
          typeof(Control),
          typeof(FocusAction),
          new PropertyMetadata(null));

      /// 
      /// Control to set the focus to.
      /// 
      public Control TargetObject
      {
         get
         {
            return (Control) base.GetValue(TargetProperty);
         }

         set
         {
            base.SetValue(TargetProperty, value);
         }
      }
   }
}
