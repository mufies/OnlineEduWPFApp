using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementBusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherIdToClassSubject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "ClassSubjects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubjects_TeacherId",
                table: "ClassSubjects",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassSubjects_UserAccounts_TeacherId",
                table: "ClassSubjects",
                column: "TeacherId",
                principalTable: "UserAccounts",
                principalColumn: "UserAccountId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassSubjects_UserAccounts_TeacherId",
                table: "ClassSubjects");

            migrationBuilder.DropIndex(
                name: "IX_ClassSubjects_TeacherId",
                table: "ClassSubjects");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "ClassSubjects");
        }
    }
}
