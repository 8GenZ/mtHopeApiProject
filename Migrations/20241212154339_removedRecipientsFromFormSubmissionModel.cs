using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mtHopeApiProject.Migrations
{
    /// <inheritdoc />
    public partial class removedRecipientsFromFormSubmissionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormSubmissions_Recipients_RecipientId",
                table: "FormSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_FormSubmissions_RecipientId",
                table: "FormSubmissions");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "FormSubmissions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipientId",
                table: "FormSubmissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmissions_RecipientId",
                table: "FormSubmissions",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormSubmissions_Recipients_RecipientId",
                table: "FormSubmissions",
                column: "RecipientId",
                principalTable: "Recipients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
