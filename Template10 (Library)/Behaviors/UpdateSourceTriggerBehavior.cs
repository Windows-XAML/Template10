using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Template10.Utils;
using static Template10.Utils.XamlUtils;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Behaviors
{
    [TypeConstraint(typeof(TextBox))]
    public class UpdateSourceTriggerBehavior : DependencyObject, IBehavior
    {
        TextBox _textBox;
        Page _page;

        public DependencyObject AssociatedObject { get; private set; }
        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            _textBox = associatedObject as TextBox;
            _textBox.TextChanged += _textBox_TextChanged;
        }

        private void _textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _page = _page ?? _textBox.Ancestor<Page>();
            var field = _page.GetType().GetTypeInfo().GetDeclaredField("Bindings");
            var bindings = field?.GetValue(_page);
            var update = bindings?.GetType().GetRuntimeMethod("Update", new Type[] { });
            //update?.Invoke(bindings, null);
        }

        public void Detach()
        {
            _textBox.TextChanged -= _textBox_TextChanged;
        }
    }
}
