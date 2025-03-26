using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GS1Takehome.Models.Entities
{
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
