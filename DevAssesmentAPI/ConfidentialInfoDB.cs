using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DevAssesmentAPI
{
	public class ConfidentialInfoDB : DbContext
	{
        public ConfidentialInfoDB(DbContextOptions<ConfidentialInfoDB> options) : base(options) { }

        public DbSet<ConfidentialInfoModel> ConfidentialInfos => Set<ConfidentialInfoModel>();
    }
}
