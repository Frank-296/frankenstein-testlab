#nullable disable
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Api.Models;

namespace Api.Data;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

	public virtual DbSet<TestApplication> TestApplications { get; set; }
	public virtual DbSet<BusinessProcess> BusinessProcesses { get; set; }
	public virtual DbSet<SuiteExecution> SuiteExecutions { get; set; }
	public virtual DbSet<TestCase> TestCases { get; set; }
	public virtual DbSet<Execution> Executions { get; set; }
	public virtual DbSet<Defect> Defects { get; set; }
	public virtual DbSet<Company> Companies { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<TestApplication>().HasIndex(a => new { a.Name }).IsUnique(true);

		builder.Entity<BusinessProcess>().HasIndex(p => new { p.Name, p.TestApplicationId }).IsUnique(true);

		builder.Entity<TestCase>().HasIndex(c => new { c.Name, c.BusinessProcessId }).IsUnique(true);

		builder.Entity<Defect>().HasIndex(d => new { d.ExecutionId }).IsUnique(true);

		builder.Entity<Company>().HasIndex(c => new { c.Name }).IsUnique(true);

		builder.Entity<User>(b =>
		{
			b.HasMany(e => e.Claims).WithOne(e => e.User).HasForeignKey(uc => uc.UserId).IsRequired();
			b.HasMany(e => e.Logins).WithOne(e => e.User).HasForeignKey(ul => ul.UserId).IsRequired();
			b.HasMany(e => e.Tokens).WithOne(e => e.User).HasForeignKey(ut => ut.UserId).IsRequired();
			b.HasMany(e => e.UserRoles).WithOne(e => e.User).HasForeignKey(ur => ur.UserId).IsRequired();
		});

		builder.Entity<Role>(b =>
		{
			b.HasMany(e => e.UserRoles).WithOne(e => e.Role).HasForeignKey(ur => ur.RoleId).IsRequired();
			b.HasMany(e => e.RoleClaims).WithOne(e => e.Role).HasForeignKey(rc => rc.RoleId).IsRequired();
		});

		foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
			relationship.DeleteBehavior = DeleteBehavior.NoAction;
	}
}