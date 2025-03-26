using GS1Takehome.Models.Entities;
using GS1Takehome.Models.Services;
using Hangfire;

namespace GS1Takehome.Models
{
	public class PriceModel
	{
		public const int MaxAttempts = 3;
		public const int IntervalMilliseconds = 15000;
		private IDataReceiverService RetailerService;

		public PriceModel(IDataReceiverService retailerService)
		{
			RetailerService = retailerService;
		}

		public int SubmitPrice(ItemPrice itemPrice)
		{
			ItemPriceSubmission submission = CreatePriceSubmissionRequest(itemPrice);
			CreateSubmitPriceTask(submission);
			return submission.Id;
		}

		private ItemPriceSubmission CreatePriceSubmissionRequest(ItemPrice itemPrice)
		{
			using (var db = new PriceContext())
			{
				var submission = new ItemPriceSubmission
				{
					Gtin = itemPrice.Gtin,
					Price = itemPrice.Price,
					RequestDatetime = DateTime.Now
				};
				db.ItemPriceSubmissions.Add(submission);
				db.SaveChanges();
				return submission;
			}
		}

		private ItemPriceSubmission? GetPriceSubmission(int id) 
		{
			using (var db = new PriceContext())
			{
				return db.ItemPriceSubmissions
					.Where(s => s.Id == id)
					.FirstOrDefault();
			}
		}


		public PriceStatus GetSubmitPriceStatus(int id)
		{
			var priceSubmission = GetPriceSubmission(id);
			if (priceSubmission == null)
			{
				throw new Exception("Item Price Submission Id was not found. ");
			}
			if (priceSubmission.FailureDatetime != null)
			{
				throw new Exception("Item Price Submission has failed.");
			}

			return priceSubmission.SubmittedDatetime != null ? 
				PriceStatus.Submitted : PriceStatus.PendingSubmission;
		}

		private void CreateSubmitPriceTask(ItemPriceSubmission submission)
		{
			BackgroundJob.Enqueue(() => WaitToSubmitPrice(submission));
		}

		public void WaitToSubmitPrice(ItemPriceSubmission submission)
		{
			bool canSubmit = false;

			for (int attempt = 0; attempt < MaxAttempts; attempt++)
			{
				if (!canSubmit)
				{
					Thread.Sleep(IntervalMilliseconds);
				}

				canSubmit = RetailerService.CanSubmitPrice(submission.Gtin);
			}

			if (canSubmit)
			{
				SaveSubmissionDate(submission.Id, DateTime.Now);
				RetailerService.SubmitPrice(new ItemPrice(submission.Gtin, submission.Price));
				SaveSubmittedDate(submission.Id, DateTime.Now);
			}
			else
			{
				SaveFailureDate(submission.Id, DateTime.Now);
			}
		}

		private void SaveSubmissionDate(int id, DateTime dateTime) 
		{
			using (var db = new PriceContext())
			{
				var submission = db.ItemPriceSubmissions.Where(s => s.Id == id).First();
				submission.SubmissionDatetime = dateTime;
				db.SaveChanges();
			}
		}

		private void SaveSubmittedDate(int id, DateTime dateTime)
		{
			using (var db = new PriceContext())
			{
				var submission = db.ItemPriceSubmissions.Where(s => s.Id == id).First();
				submission.SubmittedDatetime = dateTime;
				db.SaveChanges();
			}
		}

		private void SaveFailureDate(int id, DateTime dateTime)
		{
			using (var db = new PriceContext())
			{
				var submission = db.ItemPriceSubmissions.Where(s => s.Id == id).First();
				submission.FailureDatetime = dateTime;
				db.SaveChanges();
			}
		}
	}
}
