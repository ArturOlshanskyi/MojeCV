using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MojeCV.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doswiadczenia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Stanowisko = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Firma = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    DataOd = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataDo = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Opis = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doswiadczenia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Osoby",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Imie = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Nazwisko = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Stanowisko = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Telefon = table.Column<string>(type: "TEXT", nullable: true),
                    Miasto = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    OMnie = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Osoby", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Umiejetnosci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Kategoria = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Poziom = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Umiejetnosci", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wyksztalcenia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Uczelnia = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Kierunek = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Poziom = table.Column<string>(type: "TEXT", maxLength: 80, nullable: true),
                    DataOd = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataDo = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wyksztalcenia", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Doswiadczenia");

            migrationBuilder.DropTable(
                name: "Osoby");

            migrationBuilder.DropTable(
                name: "Umiejetnosci");

            migrationBuilder.DropTable(
                name: "Wyksztalcenia");
        }
    }
}
