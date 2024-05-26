using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitPeak.Data.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUserChallengeFavorite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserChallenge_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserChallenge");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserChallenge_Challenges_ChallengeId",
                table: "ApplicationUserChallenge");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserChallenge",
                table: "ApplicationUserChallenge");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "ApplicationUserChallenge",
                newName: "ApplicationUserChallenges");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Challenges",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserChallenge_ChallengeId",
                table: "ApplicationUserChallenges",
                newName: "IX_ApplicationUserChallenges_ChallengeId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Challenges",
                type: "nvarchar(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Difficulty",
                table: "Challenges",
                type: "nvarchar(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Challenges",
                type: "nvarchar(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Challenges",
                type: "nvarchar(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserChallenges",
                table: "ApplicationUserChallenges",
                columns: new[] { "ApplicationUserId", "ChallengeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserChallenges_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserChallenges",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserChallenges_Challenges_ChallengeId",
                table: "ApplicationUserChallenges",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserChallenges_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserChallenges");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserChallenges_Challenges_ChallengeId",
                table: "ApplicationUserChallenges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserChallenges",
                table: "ApplicationUserChallenges");

            migrationBuilder.RenameTable(
                name: "ApplicationUserChallenges",
                newName: "ApplicationUserChallenge");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Challenges",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserChallenges_ChallengeId",
                table: "ApplicationUserChallenge",
                newName: "IX_ApplicationUserChallenge_ChallengeId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Challenges",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(MAX)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Difficulty",
                table: "Challenges",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(MAX)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Challenges",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(MAX)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Challenges",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(MAX)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserChallenge",
                table: "ApplicationUserChallenge",
                columns: new[] { "ApplicationUserId", "ChallengeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserChallenge_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserChallenge",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserChallenge_Challenges_ChallengeId",
                table: "ApplicationUserChallenge",
                column: "ChallengeId",
                principalTable: "Challenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
