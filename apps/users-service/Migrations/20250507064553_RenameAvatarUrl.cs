using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace users_service.Migrations
{
    /// <inheritdoc />
    public partial class RenameAvatarUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "profile_pic_url",
                table: "users",
                newName: "avatar_url");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "avatar_url",
                table: "users",
                newName: "profile_pic_url");
        }
    }
}
