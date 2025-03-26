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

		/**
		 * Initialises a price submission by creating a submission request and queueing a submit price task. Returns submission id. 
		 */
		public int SubmitPrice(ItemPrice itemPrice)
		{
			ItemPriceSubmission submission = CreatePriceSubmissionRequest(itemPrice);
			CreateSubmitPriceTask(submission);
			return submission.Id;
		}

		/**
		 * Creates a record of the item price submission in the database. 
		 */
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

		/**
		 * Gets the submission data from the given id. 
		 */
		private ItemPriceSubmission? GetPriceSubmission(int id) 
		{
			using (var db = new PriceContext())
			{
				return db.ItemPriceSubmissions
					.Where(s => s.Id == id)
					.FirstOrDefault();
			}
		}

		/**
		 * Gets the submission status of the given id. 
		 */
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

		/**
		 * Queues a submit price task for the given submission. 
		 */
		private void CreateSubmitPriceTask(ItemPriceSubmission submission)
		{
			BackgroundJob.Enqueue(() => WaitToSubmitPrice(submission));
		}

		/**
		 * Waits until price can be submitted then submits the price and updates database entry. 
		 */
		public void WaitToSubmitPrice(ItemPriceSubmission submission)
		{
			// waits until service says can submit or out of attempts
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

		/**
		 * Saves the submission date for the given submission id. 
		 */
		private void SaveSubmissionDate(int id, DateTime dateTime) 
		{
			using (var db = new PriceContext())
			{
				var submission = db.ItemPriceSubmissions.Where(s => s.Id == id).First();
				submission.SubmissionDatetime = dateTime;
				db.SaveChanges();
			}
		}

		/**
		 * Saves the submitted date for the given submission id. 
		 */
		private void SaveSubmittedDate(int id, DateTime dateTime)
		{
			using (var db = new PriceContext())
			{
				var submission = db.ItemPriceSubmissions.Where(s => s.Id == id).First();
				submission.SubmittedDatetime = dateTime;
				db.SaveChanges();
			}
		}


		/**
		 * Saves the submission date for the given submission id. 
		 */
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
