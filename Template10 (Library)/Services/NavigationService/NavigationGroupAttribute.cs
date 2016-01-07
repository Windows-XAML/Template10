using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.NavigationService {
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class NavigationGroupAttribute : Attribute
	{
		public string GroupName { get; private set; }

		public NavigationGroupAttribute(string groupName)
		{
			if (String.IsNullOrEmpty(groupName))
				throw new ArgumentNullException(nameof(groupName));

			GroupName = groupName;
		}
	}
}
