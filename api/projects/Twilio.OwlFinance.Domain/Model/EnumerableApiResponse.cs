using System.Collections.Generic;

namespace Twilio.OwlFinance.Domain.Model
{
    public class EnumerableApiResponse<T> : ApiResponse<IEnumerable<T>>
        where T : class
    {
        public EnumerableApiResponse()
            : base()
        { }

        public EnumerableApiResponse(IEnumerable<T> data)
            : base(data)
        { }
    }
}
