using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Template10.Utils;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Behaviors
{
    public class UpdateSourceTriggerBehavior : DependencyObject, IBehavior
    {
        TextBox _textBox;
        Page _page;

        public DependencyObject AssociatedObject { get; private set; }
        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            _textBox = associatedObject as TextBox;
            _page = XamlUtils.Ancestor<Page>(_textBox);
            _textBox.TextChanged += _textBox_TextChanged;
        }

        private void _textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var fields = _page.GetType().GetRuntimeFields();
            var bindings = fields.FirstOrDefault(x => x.Name.Equals("Bindings"));
            if (bindings != null)
            {
                var update = bindings.GetType().GetTypeInfo().GetDeclaredMethod("Update");
                update?.Invoke(bindings, null);
            }
        }

        public void Detach()
        {
            _textBox.TextChanged -= _textBox_TextChanged;
        }
    }
}
