using System.Linq;
using System.Web.Mvc;
using TestAssembly.Models;
using TestAssembly.Services;
using TestAssembly.Utils;
using TestWebApi.Models;

namespace TestWebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpPost]
        public JsonResult GenerateReferenceData()
        {
            var referenceDataService = new ReferenceDataService();
            var elapsedTime = PerformanceTools.MeasureElapsedTime(() => referenceDataService.Generate());

            return Json(new BaseApiResponse { ElapsedTime = elapsedTime });
        }

        [HttpPost]
        public JsonResult GenerateTransactionData(int batchCount, int rowsInFile)
        {
            var transactionDataService = new TransactionDataService();
            var elapsedTime = PerformanceTools.MeasureElapsedTime(() => transactionDataService.Generate(batchCount, rowsInFile));

            return Json(new BaseApiResponse { ElapsedTime = elapsedTime });
        }

        [HttpPost]
        public JsonResult ImportTransactionData()
        {
            var transactionDataService = new TransactionDataService();
            var elapsedTime = PerformanceTools.MeasureElapsedTime(() => transactionDataService.Import());

            return Json(new BaseApiResponse { ElapsedTime = elapsedTime });
        }

        [HttpPost]
        public JsonResult GetTransactionData(int skip, int take)
        {
            var transactionDataService = new TransactionDataService();

            var transactionalDataItems = Enumerable.Empty<TransactionalData>();

            var elapsedTime = PerformanceTools.MeasureElapsedTime(() =>
                    {
                        transactionalDataItems = transactionDataService.FetchTransactionalData(skip, take);
                    });

            return Json(new TransactionDataApiResponse
            {
                ElapsedTime = elapsedTime,
                TransactionalDataItems = transactionalDataItems
            });
        }
    }
}
