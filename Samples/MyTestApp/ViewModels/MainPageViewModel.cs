using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Template10.Mvvm;

namespace MyTestApp.ViewModels {
	public class MainPageViewModel : ViewModelBase {
		#region ICommand TestCommand

		DelegateCommand<object> _testCommand;
		public ICommand TestCommand {
			get {
				return _testCommand ?? (_testCommand = new DelegateCommand<object>(
					(o) => {
						System.Diagnostics.Debug.WriteLine(o);
					}
				));
			}
		}

		#endregion
	}
}
