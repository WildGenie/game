using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repositories.Migrations
{
    public partial class AddedSpecies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    PluralName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    ForceSensitive = table.Column<bool>(nullable: false),
                    HpCoefficient = table.Column<float>(nullable: false),
                    StrengthModifier = table.Column<short>(nullable: false),
                    DexterityModifier = table.Column<short>(nullable: false),
                    ConstitutionModifier = table.Column<short>(nullable: false),
                    IntelligenceModifier = table.Column<short>(nullable: false),
                    CharismaModifier = table.Column<short>(nullable: false),
                    WisdomModifier = table.Column<short>(nullable: false),
                    AwarenessModifier = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Species_Name",
                table: "Species",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Species_PluralName",
                table: "Species",
                column: "PluralName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Species");
        }
    }
}
