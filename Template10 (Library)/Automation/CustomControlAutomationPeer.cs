using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Template10.Automation
{
	/// <summary>
	/// https://msdn.microsoft.com/en-us/library/windows/apps/xaml/mt297667.aspx
	/// http://stackoverflow.com/questions/33964066/how-do-i-make-a-uwp-custom-i-e-templated-button-control-with-accessibility-su
	/// Each control class needs an associated AutomationPeer class that maps the behavior of the 
	/// specific control to a set of standards to be consumed by accessibility tools.
	/// 
	/// AutomationPeers also allow you support "interaction patterns" by implementing a pattern 
	/// provider interface. For this button example the Invoke pattern is appropriate. We need 
	/// to implement IInvokeProvider so add it to the class declaration:
	/// </summary>
	public class CustomControlAutomationPeer : FrameworkElementAutomationPeer, IInvokeProvider
	{
		/// <summary>
		/// The automation peer should define a type-safe constructor that uses an instance of the owner 
		/// control for base initialization.
		/// 
		/// The implementation passes the owner value on to the FrameworkElementAutomationPeer base, and 
		/// ultimately it is the FrameworkElementAutomationPeer that actually uses owner to set 
		/// FrameworkElementAutomationPeer.Owner.
		/// </summary>
		public CustomControlAutomationPeer(FrameworkElement owner) : base(owner)
		{
			//Debug.WriteLine("Constructor");
		}

		/// <summary>
		/// Whenever you define a new peer class, implement the GetClassNameCore method
		/// 
		/// Note  You might want to store the strings as constants rather than directly in the method body, 
		/// but that is up to you. For GetClassNameCore, you won't need to localize this string. 
		/// The LocalizedControlType property is used any time a localized string is needed by a 
		/// UI Automation client, not ClassName.
		/// </summary>
		/// <returns></returns>
		protected override string GetClassNameCore()
		{
			Debug.WriteLine("GetClassNameCore");

			return "CustomControl";
		}

		/// <summary>
		/// Stops the screen reader from being able to see the guts of our control by 
		/// implementing the GetChildrenCore() method by returning null 
		/// </summary>
		/// <returns>IList<AutomationPeer></returns>
		protected override IList<AutomationPeer> GetChildrenCore()
		{
			//Debug.WriteLine("GetChildrenCore :");
			IList<AutomationPeer> automationPeers = base.GetChildrenCore();
			foreach (var peer in automationPeers)
			{
				Debug.WriteLine("-------------------------------------------------------------");
				Debug.WriteLine("Children Peers");
				Debug.WriteLine("-------------------------------------------------------------");
				Debug.WriteLine("Name : " + peer.GetName());
				Debug.WriteLine("AutomationID : " + peer.GetAutomationId());
				Debug.WriteLine("AcceleratorKey : " + peer.GetAcceleratorKey());
				Debug.WriteLine("HelpText : " + peer.GetHelpText());
				Debug.WriteLine("IsEnable : " + peer.IsEnabled());
				Debug.WriteLine("IsKeyboardFocusable : " + peer.IsKeyboardFocusable());
				Debug.WriteLine("isContentElement :" + peer.IsContentElement());
				Debug.WriteLine("IsControlelement : " + peer.IsControlElement());
				Debug.WriteLine("Parent : " + peer.GetParent());
				Debug.WriteLine("LocalizedControlType : " + peer.GetLocalizedControlType());
				Debug.WriteLine("ItemType : " + peer.GetItemType());
				Debug.WriteLine("ItemStatus : " + peer.GetItemStatus());
				Debug.WriteLine("AccessKey : " + peer.GetAccessKey());
				Debug.WriteLine("Value : " + peer.ToString());
				Debug.WriteLine("IsRequiredForForm : " + peer.IsRequiredForForm());
				Debug.WriteLine("IsPassword : " + peer.IsPassword());
				Debug.WriteLine("Type : " + peer.GetType());
				Debug.WriteLine("PositionInSet : " + peer.GetPositionInSet());
				Debug.WriteLine("LocalizedLandmarkType : " + peer.GetLocalizedLandmarkType());
				Debug.WriteLine("Level : " + peer.GetLevel());
				Debug.WriteLine("ClickablePoint : " + peer.GetClickablePoint());
				Debug.WriteLine("AutomationControlType : " + peer.GetAutomationControlType());
			}
			return automationPeers;
			//return null;
		}
		/// <summary>
		/// speaks the button label text whenever we select a control.
		/// 
		/// We can make it say the text in the control label by implementing the GetNameCore() method:
		/// </summary>
		/// <returns>string</returns>
		protected override string GetNameCore()
		{
			Debug.WriteLine("GetNameCore : " + ((FrameworkElement)Owner).Name);
			return ((FrameworkElement)Owner).Name;
		}
		/// <summary>
		/// A peer's implementation of GetPatternCore returns the object that supports the pattern that is requested 
		/// in the input parameter. Specifically, a UI Automation client calls a method that is forwarded to the 
		/// provider's GetPattern method, and specifies a PatternInterface enumeration value that names the requested 
		/// pattern. Your override of GetPatternCore should return the object that implements the specified pattern. 
		/// That object is the peer itself, because the peer should implement the corresponding pattern interface any 
		/// time that it reports that it supports a pattern. If your peer does not have a custom implementation of a 
		/// pattern, but you know that the peer's base does implement the pattern, you can call the base type's 
		/// implementation of GetPatternCore from your GetPatternCore. A peer's GetPatternCore should return null if a 
		/// pattern is not supported by the peer. However, instead of returning null directly from your implementation, 
		/// you would usually rely on the call to the base implementation to return null for any unsupported pattern.
		/// When a pattern is supported, the GetPatternCore implementation can return this or Me.The expectation is that 
		/// the UI Automation client will cast the GetPattern return value to the requested pattern interface whenever 
		/// it is not null.
		/// If a peer class inherits from another peer, and all necessary support and pattern reporting is already 
		/// handled by the base class, implementing GetPatternCore isn't necessary. For example, if you are implementing 
		/// a range control that derives from RangeBase, and your peer derives from RangeBaseAutomationPeer, that peer 
		/// returns itself for PatternInterface.
		/// 
		/// If you are implementing a peer where you don't have all the support you need from a base peer class, or you 
		/// want to change or add to the set of base-inherited patterns that your peer can support, then you should 
		/// override GetPatternCore to enable UI Automation clients to use the patterns.
		/// </summary>
		/// <param name="patternInterface"></param>
		/// <returns></returns>
		protected override object GetPatternCore(PatternInterface patternInterface)
		{
			
			if (patternInterface == PatternInterface.Invoke)
			{
				Debug.WriteLine("patternInterface == PatternInterface.Invoke");
				return this;
			}
			else if (patternInterface == PatternInterface.SpreadsheetItem)
			{
				Debug.WriteLine("patternInterface == PatternInterface.SpreadsheetItem");
				return this;
			}
			else if (patternInterface == PatternInterface.Toggle)
			{
				Debug.WriteLine("patternInterface == PatternInterface.Toggle");
				return this;
			}
			else if (patternInterface == PatternInterface.ItemContainer)
			{
				Debug.WriteLine("patternInterface == PatternInterface.ItemContainer");
				return this;
			}
			else if (patternInterface == PatternInterface.Scroll)
			{
				Debug.WriteLine("patternInterface == PatternInterface.Scroll");
				//ItemsControl owner = (ItemsControl)base.Owner;
				//UIElement itemsHost = owner.ItemsHost;
				//ScrollViewer element = null;
				//while (itemsHost != owner)
				//{
				//	itemsHost = VisualTreeHelper.GetParent(itemsHost) as UIElement;
				//	element = itemsHost as ScrollViewer;
				//	if (element != null)
				//	{
				//		break;
				//	}
				//}
				//if (element != null)
				//{
				//	AutomationPeer peer = FrameworkElementAutomationPeer.CreatePeerForElement(element);
				//	if ((peer != null) && (peer is IScrollProvider))
				//	{
				//		return (IScrollProvider)peer;
				//	}
				//}
				//return this;
			}
			else if (patternInterface == PatternInterface.Selection)
			{
				Debug.WriteLine("patternInterface == PatternInterface.Selection");
				return this;
			}
			else if (patternInterface == PatternInterface.SelectionItem)
			{
				Debug.WriteLine("patternInterface == PatternInterface.SelectionItem");
				return this;
			}
			else if (patternInterface == PatternInterface.Window)
			{
				Debug.WriteLine("patternInterface == PatternInterface.Window");
				return this;
			}
			//Debug.WriteLine("patternInterface == null"); //base.GetPattern(patternInterface)");
			return null;  //base.GetPattern(patternInterface);
		}

		/// <summary>
		/// Some assistive technologies use the GetAutomationControlType value directly when reporting characteristics 
		/// of the items in a UI Automation tree, as additional information beyond the UI Automation Name. If your 
		/// control is significantly different from the control you are deriving from and you want to report a 
		/// different control type from what is reported by the base peer class used by the control, you must implement 
		/// a peer and override GetAutomationControlTypeCore in your peer implementation. This is particularly important 
		/// if you derive from a generalized base class such as ItemsControl or ContentControl, where the base peer 
		/// doesn't provide precise information about control type.
		/// 
		/// Your implementation of GetAutomationControlTypeCore describes your control by returning an 
		/// AutomationControlType value.Although you can return AutomationControlType.Custom, you should return one of 
		/// the more specific control types if it accurately describes your control's main scenarios.
		/// 
		/// Note:  Unless you specify AutomationControlType.Custom, you don't have to implement GetLocalizedControlTypeCore 
		/// to provide a LocalizedControlType property value to clients. UI Automation common infrastructure provides 
		/// translated strings for every possible AutomationControlType value other than AutomationControlType.Custom.
		/// </summary>
		/// <returns>AutomationControlType</returns>
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			Debug.WriteLine("GetAutomationControlTypeCore == AutomationControlType.Group");
			return AutomationControlType.Button;
		}
		/// <summary>
		/// Implement the IsContentElementCore and IsControlElementCore methods to indicate whether your control contains 
		/// data content or fulfills an interactive role in the user interface (or both). By default, both methods 
		/// return true. These settings improve the usability of assistive technologies such as screen readers, which 
		/// may use these methods to filter the automation tree. If your GetPatternCore method transfers pattern 
		/// handling to a subelement peer, the subelement peer's IsControlElementCore method can return false to hide 
		/// the subelement peer from the automation tree.
		/// </summary>
		/// <returns></returns>
		protected override bool IsContentElementCore()
		{
			Debug.WriteLine("IsContentElementCore");
			return base.IsContentElementCore();
		}

		protected override bool IsControlElementCore()
		{
			Debug.WriteLine("IsControlElementCore");
			return base.IsControlElementCore();
		}
		public void Invoke()
		{
			Debug.WriteLine("Invoke() " + ((FrameworkElement)Owner));
			//((HamburgerMenu)Owner).DoClick();
		}

		/// <summary>
		/// Sets the focus to a list and selects a string item in that list.
		/// </summary>
		/// <param name="listElement">The list element.</param>
		/// <param name="itemText">The text to select.</param>
		/// <remarks>
		/// This deselects any currently selected items. To add the item to the current selection 
		/// in a multiselect list, use AddToSelection instead of Select.
		/// </remarks>
		//public void SelectListItem(AutomationElement listElement, String itemText)
		//{
		//	if ((listElement == null) || (itemText == ""))
		//	{
		//		throw new ArgumentException("Argument cannot be null or empty.");
		//	}
		//	listElement.SetFocus();
		//	Condition cond = new PropertyCondition(
		//		AutomationElement.NameProperty, itemText, PropertyConditionFlags.IgnoreCase);
		//	AutomationElement elementItem = listElement.FindFirst(TreeScope.Children, cond);
		//	if (elementItem != null)
		//	{
		//		SelectionItemPattern pattern;
		//		try
		//		{
		//			pattern = elementItem.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
		//		}
		//		catch (InvalidOperationException ex)
		//		{
		//			Console.WriteLine(ex.Message);  // Most likely "Pattern not supported."
		//			return;
		//		}
		//		pattern.Select();
		//	}
		//}
	}
}
