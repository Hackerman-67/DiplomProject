using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS_Quadra.Migrations
{
    /// <inheritdoc />
    public partial class _AddTitleToCourseContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CourseContents",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "CourseContents");
        }
    }
}
