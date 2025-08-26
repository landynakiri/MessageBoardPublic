using System.Data.Entity;
using Npgsql;

namespace Test.Server.SQLService
{
	public class CommonDbContext<T> : DbContext where T : class
	{
		public CommonDbContext(NpgsqlConnection npgsqlConnection) : base(npgsqlConnection, true)
		{

		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<T>()
						.Map(m => m.ToTable(typeof(T).Name, "public"));
		}


		public DbSet<T> Data { get; set; }
	}
}
