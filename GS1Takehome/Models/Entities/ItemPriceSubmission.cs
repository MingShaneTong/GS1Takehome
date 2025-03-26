using System.ComponentModel.DataAnnotations.Schema;

namespace GS1Takehome.Models.Entities
{
	/**
	 * Represents a submission for the price of a item.
	 */
	public class ItemPriceSubmission
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Gtin { get; set; }
		public decimal Price { get; set; }
		public DateTime RequestDatetime { get; set; }
		public DateTime? SubmissionDatetime { get; set; }
		public DateTime? SubmittedDatetime { get; set; }
		public DateTime? FailureDatetime { get; set; }
	}
}
