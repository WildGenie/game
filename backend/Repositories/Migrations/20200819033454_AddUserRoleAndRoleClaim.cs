using Microsoft.EntityFrameworkCore.Migrations;

namespace Repositories.Migrations
{
	public partial class AddUserRoleAndRoleClaim : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				"Discriminator",
				"AspNetUserRoles",
				nullable: false);

			migrationBuilder.AddColumn<string>(
				"Discriminator",
				"AspNetRoleClaims",
				nullable: false);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				"Discriminator",
				"AspNetUserRoles");

			migrationBuilder.DropColumn(
				"Discriminator",
				"AspNetRoleClaims");
		}
	}
}