using Windows.System.Profile;
using Windows.UI.Xaml;

namespace Template10.Triggers
{
	/// <summary>
	/// Trigger for detecting various Windows Device Families
	/// </summary>
	public class DeviceFamilyStateTrigger : StateTriggerBase
	{
		private static readonly string NativeDeviceFamily;

		static DeviceFamilyStateTrigger()
		{
			NativeDeviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;
		}

		/// <summary>
		/// Gets or sets the device family to trigger on.
		/// </summary>
		/// <value>The device family.</value>
		public DeviceFamily DeviceFamily
		{
			get { return (DeviceFamily)GetValue(DeviceFamilyProperty); }
			set { SetValue(DeviceFamilyProperty, value); }
		}

		/// <summary>
		/// Identifies the <see cref="DeviceFamily"/> DependencyProperty
		/// </summary>
		public static readonly DependencyProperty DeviceFamilyProperty =
			DependencyProperty.Register("DeviceFamily", typeof(DeviceFamily), typeof(DeviceFamilyStateTrigger),
			new PropertyMetadata(DeviceFamily.Unknown, OnDeviceTypePropertyChanged));

		private static void OnDeviceTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var deviceFamilyStateTrigger = (DeviceFamilyStateTrigger) d;
			var deviceFamily = (DeviceFamily) e.NewValue;
			switch (NativeDeviceFamily)
			{
				case "Windows.Mobile":
					deviceFamilyStateTrigger.SetActive(deviceFamily == DeviceFamily.Mobile);
					break;
				case "Windows.Desktop":
					deviceFamilyStateTrigger.SetActive(deviceFamily == DeviceFamily.Desktop);
					break;
				case "Windows.Team":
					deviceFamilyStateTrigger.SetActive(deviceFamily == DeviceFamily.Team);
					break;
				case "Windows.IoT":
					deviceFamilyStateTrigger.SetActive(deviceFamily == DeviceFamily.IoT);
					break;
				case "Windows.Xbox":
					deviceFamilyStateTrigger.SetActive(deviceFamily == DeviceFamily.Xbox);
					break;
				case "Windwos.Universal":
					deviceFamilyStateTrigger.SetActive(deviceFamily == DeviceFamily.Universal);
					break;
				default:
					deviceFamilyStateTrigger.SetActive(deviceFamily == DeviceFamily.Unknown);
					break;
			}
		}
	}

	/// <summary>
	/// Device Families Enumeration
	/// </summary>
	public enum DeviceFamily
	{
		/// <summary>
		/// Unknown Device Family
		/// </summary>
		Unknown,
		/// <summary>
		/// Universal Device Family
		/// </summary>
		Universal,
		/// <summary>
		/// Windows Desktop
		/// </summary>
		Desktop,
		/// <summary>
		/// Windows Mobile
		/// </summary>
		Mobile,
		/// <summary>
		/// Windows Team (Surface Hub)
		/// </summary>
		Team,
		/// <summary>
		/// Windows IoT
		/// </summary>
		IoT,
		/// <summary>
		/// Xbox
		/// </summary>
		Xbox
	}
}