using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Calbaza_Ioan_Lab2.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorsRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorID",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_AuthorID",
                table: "Book",
                column: "AuthorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Authors_AuthorID",
                table: "Book",
                column: "AuthorID",
                principalTable: "Authors",
                principalColumn: "ID");
            migrationBuilder.Sql(@"
    INSERT INTO [Authors] ([FirstName], [LastName])
    SELECT DISTINCT
        LEFT([Author], CHARINDEX(N' ', [Author]) - 1),
        SUBSTRING([Author], CHARINDEX(N' ', [Author]) + 1, LEN([Author]))
    FROM [Book]
    WHERE [Author] IS NOT NULL
      AND CHARINDEX(N' ', [Author]) > 0;

    UPDATE b
    SET b.[AuthorID] = a.[ID]
    FROM [Book] b
    INNER JOIN [Authors] a
        ON b.[Author] = a.[FirstName] + N' ' + a.[LastName];
");
            migrationBuilder.DropColumn(
                  name: "Author",
                  table: "Book");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Authors_AuthorID",
                table: "Book");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Book_AuthorID",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "AuthorID",
                table: "Book");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Book",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
