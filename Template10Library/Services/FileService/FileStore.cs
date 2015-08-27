using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Template10.Services.FileService
{
	public class FileStore
	{
		public static readonly FileStore Local;
		public static readonly FileStore Roaming;
		public static readonly FileStore Temporary;

		protected StorageFolder Folder { get; }

		static FileStore()
		{
			Local = new FileStore(ApplicationData.Current.LocalFolder);
			Roaming = new FileStore(ApplicationData.Current.RoamingFolder);
			Temporary = new FileStore(ApplicationData.Current.TemporaryFolder);
		}

		private FileStore(StorageFolder folder)
		{
			Folder = folder;
		}

		// Folders too? better "item" or wihtout any further naming??
		public async Task<bool> FileExistsAsync(string fileName)
		{
			return await Folder.TryGetItemAsync(fileName) != null;
		}

		public async Task<bool> DeleteFileAsync(string fileName)
		{
			IStorageItem file = await Folder.TryGetItemAsync(fileName);

			if (file != null)
				await file.DeleteAsync();

			return await FileExistsAsync(fileName);
		}



	}
}
