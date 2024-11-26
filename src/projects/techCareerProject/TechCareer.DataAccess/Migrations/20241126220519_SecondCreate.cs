using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechCareer.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SecondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    About = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ApplicationDeadline = table.Column<DateTime>(type: "datetime", nullable: false),
                    ParticipationText = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VideoEducations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TotalHour = table.Column<float>(type: "real", nullable: false),
                    IsCertified = table.Column<bool>(type: "bit", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    InstructorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProgrammingLanguage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoEducations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoEducations_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 123, 121, 136, 186, 9, 145, 8, 75, 50, 139, 219, 86, 139, 71, 159, 232, 150, 21, 3, 94, 205, 202, 16, 196, 145, 170, 43, 31, 111, 214, 148, 157, 144, 196, 195, 146, 129, 83, 182, 217, 221, 85, 37, 30, 40, 36, 225, 247, 210, 144, 79, 131, 80, 166, 164, 165, 134, 242, 195, 89, 164, 55, 52, 96 }, new byte[] { 230, 180, 3, 82, 253, 170, 158, 127, 246, 121, 29, 63, 80, 191, 251, 201, 150, 125, 231, 228, 237, 134, 38, 210, 210, 65, 199, 246, 159, 188, 184, 82, 72, 232, 42, 130, 217, 139, 10, 223, 194, 157, 121, 7, 166, 48, 117, 251, 16, 12, 32, 168, 132, 60, 149, 128, 52, 132, 202, 78, 131, 165, 100, 186, 236, 175, 248, 119, 48, 43, 135, 85, 111, 57, 60, 148, 111, 52, 87, 129, 40, 95, 205, 155, 179, 43, 146, 152, 249, 96, 69, 77, 20, 186, 155, 127, 196, 162, 206, 3, 178, 7, 13, 112, 20, 202, 186, 64, 96, 38, 20, 171, 227, 226, 108, 226, 37, 20, 183, 74, 127, 161, 186, 88, 61, 209, 117, 102 } });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CategoryId",
                table: "Events",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoEducations_InstructorId",
                table: "VideoEducations",
                column: "InstructorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "VideoEducations");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 162, 67, 65, 77, 186, 149, 220, 10, 121, 104, 56, 253, 42, 203, 142, 53, 18, 190, 185, 37, 50, 26, 119, 111, 140, 187, 7, 65, 193, 90, 83, 226, 148, 88, 237, 58, 189, 31, 2, 30, 168, 70, 44, 120, 165, 77, 100, 81, 41, 9, 21, 130, 35, 124, 120, 234, 34, 45, 100, 142, 9, 4, 187, 88 }, new byte[] { 5, 101, 248, 80, 71, 109, 255, 123, 74, 180, 43, 254, 235, 243, 31, 121, 116, 25, 73, 145, 168, 41, 204, 224, 208, 32, 72, 249, 163, 250, 212, 122, 50, 251, 119, 137, 46, 83, 233, 230, 116, 133, 169, 249, 41, 99, 178, 89, 64, 240, 6, 23, 253, 171, 92, 202, 204, 63, 8, 165, 87, 99, 9, 102, 104, 56, 94, 102, 74, 170, 153, 198, 234, 236, 11, 29, 112, 79, 87, 181, 160, 78, 241, 114, 28, 93, 14, 238, 243, 116, 173, 91, 82, 40, 181, 120, 176, 172, 34, 253, 33, 173, 236, 55, 130, 52, 125, 119, 126, 148, 25, 83, 110, 104, 237, 89, 233, 211, 149, 80, 150, 248, 251, 58, 55, 224, 146, 38 } });
        }
    }
}
