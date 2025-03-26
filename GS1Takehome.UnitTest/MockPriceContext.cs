using Microsoft.EntityFrameworkCore;

namespace GS1Takehome.Models
{
	public class MockPriceContext : PriceContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseInMemoryDatabase(databaseName: "TestDatabase");
	}
}
