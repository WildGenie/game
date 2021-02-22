using Microsoft.EntityFrameworkCore.Migrations;

namespace Repositories.Migrations
{
	public partial class AddedCustomUserRoleToContext : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				"Discriminator",
				"AspNetUserRoles");

			migrationBuilder.DropColumn(
				"Discriminator",
				"AspNetRoleClaims");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				"Discriminator",
				"AspNetUserRoles",
				"longtext CHARACTER SET utf8mb4",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				"Discriminator",
				"AspNetRoleClaims",
				"longtext CHARACTER SET utf8mb4",
				nullable: false,
				defaultValue: "");
		}
	}
}