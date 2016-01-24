﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Contracts
{
    /// <summary>
    /// Marker interface to represent a request with a void response
    /// </summary>
    public interface IRequest : IRequest<Unit> { }


    /// <summary>
    /// Marker interface to represent a request with a response
    /// </summary>
    /// <typeparam name="TResponse">Response type</typeparam>
    public interface IRequest<out TResponse> { }
}