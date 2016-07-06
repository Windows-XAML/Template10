using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.NagService
{
    public interface INagService
    {
        Task Register(Nag nag, int launches, TimeSpan duration);

        Task Register(Nag nag, int launches);

        Task Register(Nag nag, TimeSpan duration);

        Task<bool> ResponseExists(string nagId);

        Task<NagResponseInfo> GetResponse(string nagId);

        Task DeleteResponse(string nagId);
    }
}
