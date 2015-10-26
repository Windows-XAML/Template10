using System;

namespace Template10.Mvvm
{
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Windows.UI.Xaml;

	public class AwaitableDelegateCommand : System.Windows.Input.ICommand
	{
		private readonly Func<AwaitableDelegateCommandParameter, Task> _execute;
		private readonly Func<AwaitableDelegateCommandParameter, bool> _canExecute;
		public event EventHandler CanExecuteChanged;

		ConcurrentDictionary<string, Task> tasksInProgress = new ConcurrentDictionary<string, Task>();
		public AwaitableDelegateCommand(Func<AwaitableDelegateCommandParameter, Task> execute, Func<AwaitableDelegateCommandParameter, bool> canexecute = null)
		{
			if (execute == null)
				throw new ArgumentNullException(nameof(execute));
			_execute = execute;
			_canExecute = ap =>
			{
				Task taskExecuting;
				var f = tasksInProgress.TryGetValue(ap?.ExecutionKey ?? "", out taskExecuting);

				if (taskExecuting != null)
				{
					var isRunning = !(taskExecuting.Status == TaskStatus.Canceled ||
					taskExecuting.Status == TaskStatus.Faulted ||
					taskExecuting.Status == TaskStatus.RanToCompletion);
					if (isRunning)
					{
						return false;
					}
				}
				return (canexecute ?? (op => true))(ap);
			};
		}

		[DebuggerStepThrough]
		public bool CanExecute(object p = null)
		{
			try { return _canExecute(p as AwaitableDelegateCommandParameter); }
			catch { return false; }
		}

		public void Execute(object p = null)
		{

			string appKey = null;
			InternalExecute(p, out appKey);
		}

		private bool InternalExecute(object p, out string appKey)
		{
			appKey = null;
			if (!CanExecute(p))
				return false;
			try
			{
				var ap = p as AwaitableDelegateCommandParameter;
				appKey = ap?.ExecutionKey ?? Guid.NewGuid().ToString();

				Func<Task> e2 =
				   async () =>
				   {
					   try
					   {
						   await _execute(p as AwaitableDelegateCommandParameter);
						   Task taskSelf;
						   tasksInProgress.TryRemove(ap?.ExecutionKey ?? "", out taskSelf);
					   }
					   catch (Exception ex) { Debug.WriteLine(ex); Debugger.Break(); }
				   };

				var exeTask = e2();

				tasksInProgress.AddOrUpdate(ap?.ExecutionKey ?? "", exeTask, (_, __) => exeTask);
				return true;
			}
			catch { Debugger.Break(); }
			return false;
		}

		public async Task ExecuteAsync(AwaitableDelegateCommandParameter p = null)
		{

			string appKey = null;
			InternalExecute(p, out appKey);
			if (appKey == null)
			{
				throw new InvalidOperationException("unexpected execution key");
			}
			await AwaitByKey(appKey);

		}


		public async Task AwaitByKey(string executionKey)
		{
			Task taskExecuting;
			var f = tasksInProgress.TryGetValue(executionKey, out taskExecuting);

			if (taskExecuting != null)
			{
				await taskExecuting;
			}
		}

		public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}


	public class AwaitableDelegateCommandParameter : FrameworkElement  //<--this baseclass is about to get datacontext from the control
    {

        public string ExecutionKey
        {
            get { return (string)GetValue(ExecutionKeyProperty); }
            set { SetValue(ExecutionKeyProperty, value); }
        }
        public static readonly DependencyProperty ExecutionKeyProperty =
            DependencyProperty.Register(nameof(ExecutionKey), typeof(string), typeof(AwaitableDelegateCommandParameter), new PropertyMetadata(Guid.NewGuid().ToString()));




        public object InnerCommandExecutionParameter
        {
            get { return (object)GetValue(InnerCommandExecutionParameterProperty); }
            set { SetValue(InnerCommandExecutionParameterProperty, value); }
        }
                
        public static readonly DependencyProperty InnerCommandExecutionParameterProperty =
            DependencyProperty.Register(nameof(InnerCommandExecutionParameter), typeof(object), typeof(AwaitableDelegateCommandParameter), new PropertyMetadata(null));
         


    }

}
