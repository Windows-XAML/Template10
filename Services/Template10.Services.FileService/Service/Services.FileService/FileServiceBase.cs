using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Storage;

namespace Template10.Services.FileService
{
    public abstract class FileServiceBase
    {
        public FileServiceBase() => Helper = new FileHelper();

        protected FileHelper Helper { get; private set; }
    }
}
