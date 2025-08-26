using System.Data.Entity;
using Npgsql;

namespace Test.Server.SQLService
{
	public class PrimaryKeyDbContext<T, K> : CommonDbContext<T> where T : class, IPrimaryKeyContext<K>
	{
		public PrimaryKeyDbContext(NpgsqlConnection npgsqlConnection) : base(npgsqlConnection)
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<T>().HasKey(e => e.ID);
			base.OnModelCreating(modelBuilder);
		}
	}
}
