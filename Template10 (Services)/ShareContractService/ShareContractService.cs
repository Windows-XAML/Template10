﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Template10.Services.ShareContractService
{
    public class ShareContractService : IShareContractService
    {
        ShareContractHelper _helper = new ShareContractHelper();

        public void ShowUI()
        {
            _helper.ShowUI();
        }
    }
}
