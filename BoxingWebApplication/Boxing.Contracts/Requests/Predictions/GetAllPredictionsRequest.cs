﻿using Boxing.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Contracts.Requests.Predictions
{
    public class GetAllPredictionsRequest : IRequest<IEnumerable<PredictionDto>>
    {
        public int Skip { get; set; }

        public int Take { get; set; }
    }
}
