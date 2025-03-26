using GS1Takehome.Models;
using GS1Takehome.Models.Entities;
using GS1Takehome.Models.Services;
using Hangfire;

namespace GS1Takehome.UnitTest
{
	[TestClass]
	public sealed class PriceModelTest
	{
		public MockPriceContext context;

		[TestInitialize]
		public void TestSetup()
		{
			context = new MockPriceContext();
			GlobalConfiguration.Configuration.UseInMemoryStorage();
			//var server = new BackgroundJobServer();
		}

		[TestCleanup]
		public void TestCleanup()
		{
			context.ItemPriceSubmissions.RemoveRange(context.ItemPriceSubmissions);
		}

		[TestMethod]
		public void TestSuccessful()
		{
			var service = new SuccessfulRetailerService();
			var model = new PriceModel(service, context, 0);
			var itemPrice = new ItemPrice("TestGtin1", (decimal)1.00);
			
			// check submitted
			var submissionId = model.SubmitPrice(itemPrice);
			model.WaitToSubmitPrice(model.GetPriceSubmission(submissionId));
			var status = model.GetSubmitPriceStatus(submissionId);
			Assert.IsNotNull(status);
			Assert.AreEqual(PriceStatus.Submitted, status);

			// check database
			var record = model.GetPriceSubmission(submissionId);
			Assert.IsNotNull(record);
			Assert.IsNotNull(record.RequestDatetime);
			Assert.IsNotNull(record.SubmissionDatetime);
			Assert.IsNotNull(record.SubmittedDatetime);
			Assert.IsNull(record.FailureDatetime);
		}

		[TestMethod]
		public void TestFailure()
		{
			var service = new FailureRetailerService();
			var model = new PriceModel(service, context, 0);
			var itemPrice = new ItemPrice("TestGtin2", (decimal)1.00);

			// check submitted
			var submissionId = model.SubmitPrice(itemPrice);
			model.WaitToSubmitPrice(model.GetPriceSubmission(submissionId));
			Assert.ThrowsException<Exception>(() => model.GetSubmitPriceStatus(0));

			// check database
			var record = model.GetPriceSubmission(submissionId);
			Assert.IsNotNull(record);
			Assert.IsNotNull(record.RequestDatetime);
			Assert.IsNull(record.SubmissionDatetime);
			Assert.IsNull(record.SubmittedDatetime);
			Assert.IsNotNull(record.FailureDatetime);
		}

		[TestMethod]
		public void TestWrongId()
		{
			var service = new FailureRetailerService();
			var model = new PriceModel(service, context, 0);
			var itemPrice = new ItemPrice("TestGtin2", (decimal)1.00);

			// check submitted
			var submissionId = model.SubmitPrice(itemPrice);
			Assert.AreNotEqual(submissionId, 0);
			Assert.ThrowsException<Exception>(() => model.GetSubmitPriceStatus(0));
		}

		[TestMethod]
		public void TestPending()
		{
			var service = new FailureRetailerService();
			var model = new PriceModel(service, context, 0);
			var itemPrice = new ItemPrice("TestGtin2", (decimal)1.00);

			// check PendingSubmission
			var submissionId = model.SubmitPrice(itemPrice);
			var status = model.GetSubmitPriceStatus(submissionId);
			Assert.IsNotNull(status);
			Assert.AreEqual(PriceStatus.PendingSubmission, status);

			// check database
			var record = model.GetPriceSubmission(submissionId);
			Assert.IsNotNull(record);
			Assert.IsNotNull(record.RequestDatetime);
			Assert.IsNull(record.SubmissionDatetime);
			Assert.IsNull(record.SubmittedDatetime);
			Assert.IsNull(record.FailureDatetime);
		}
	}
}
