using System;
using System.Collections.Generic;
using System.Text;

namespace Luna.Biz.DataTransferObjects
{
    public class TravelStateDTO
    {
        public DateTime TimeOfArrival { get; }

        public TravelStateDTO(DateTime timeOfArrival)
        {
            TimeOfArrival = timeOfArrival;
        }
    }
}
