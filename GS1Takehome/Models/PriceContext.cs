using GS1Takehome.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GS1Takehome.Models
{
	public class PriceContext : DbContext
	{
		public DbSet<ItemPriceSubmission> ItemPriceSubmissions { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlite($"Data Source=GS1Takehome.db");
	}
}
