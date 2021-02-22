using Microsoft.EntityFrameworkCore.Migrations;

namespace Repositories.Migrations
{
	public partial class AddPendingEmail : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				"PendingEmail",
				"AspNetUsers",
				nullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				"PendingEmail",
				"AspNetUsers");
		}
	}
}