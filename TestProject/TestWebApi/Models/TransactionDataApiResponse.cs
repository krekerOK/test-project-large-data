using System.Collections.Generic;
using TestAssembly.Models;

namespace TestWebApi.Models
{
    public class TransactionDataApiResponse : BaseApiResponse
    {
        public IEnumerable<TransactionalData> TransactionalDataItems { get; set; }
    }
}