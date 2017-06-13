using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Windows.ApplicationModel.Activation;

namespace Template10
{
    public enum AdditionalKinds { Primary, Toast, SecondaryTile, Other, JumpListItem }

    public static class Extensions
    {
        /// <summary>
        /// This determines the simplest case for starting. This should handle 80% of common scenarios.
        /// When Other is returned the developer must determine start manually using IActivatedEventArgs.Kind
        /// </summary>
        public static AdditionalKinds DetermineStartCause(this Common.BootStrapper bootstrapper, IActivatedEventArgs args)
        {
            var defaultTileID = "App";

            if (args is ToastNotificationActivatedEventArgs)
            {
                return AdditionalKinds.Toast;
            }
            var e = args as ILaunchActivatedEventArgs;
            if (e?.TileId == defaultTileID && string.IsNullOrEmpty(e?.Arguments))
            {
                return AdditionalKinds.Primary;
            }
            else if (e?.TileId == defaultTileID && !string.IsNullOrEmpty(e?.Arguments))
            {
                return AdditionalKinds.JumpListItem;
            }
            else if (!string.IsNullOrEmpty(e?.TileId) && e?.TileId != defaultTileID)
            {
                return AdditionalKinds.SecondaryTile;
            }
            else
            {
                return AdditionalKinds.Other;
            }
        }
    }
}
