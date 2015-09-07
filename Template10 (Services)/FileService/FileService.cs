using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.FileService
{
    class FileService
    {
        FileHelper _helper = new FileHelper();

        //public async Task<List<Models.ColorInfo>> ReadColorsAsync(string key)
        //{
        //    try
        //    {
        //        return await _helper.ReadFileAsync<List<Models.ColorInfo>>(key, FileHelper.StorageStrategies.Roaming);
        //    }
        //    catch { return null; }
        //}

        //public async Task WriteColors(string key, List<Models.ColorInfo> colors)
        //{
        //    await _helper.WriteFileAsync(key, colors, FileHelper.StorageStrategies.Roaming);
        //}
    }
}
