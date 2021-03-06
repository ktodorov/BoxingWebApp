﻿using Boxing.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Contracts.Requests.Logins
{
    public class LogUserLoginRequest : IRequest<LoginDto>
    {
        public UserDto User { get; set; }
    }
}
