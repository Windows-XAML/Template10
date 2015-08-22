using System;
using System.Windows.Forms;

namespace ProjectWizard
{
    public partial class WizardForm : Form
    {
        public enum Choices { One, Two, None }
        public Choices Choice { get; set; } = Choices.None;

        public WizardForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Choice = Choices.One;
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Choice = Choices.Two;
            this.Dispose();
        }
    }
}
