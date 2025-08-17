using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LMS_Quadra.Migrations
{
    /// <inheritdoc />
    public partial class _alternativa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e5812f33-9966-4b99-8d5b-4d5b4d5b4d5b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6924f44-aa77-4c88-9d6c-5d6c5d6c5d6c");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "d4804f22-8875-4a64-9c1b-1d5a3a1f1e5a", "a1234b56-789c-0def-1234-567890abcdef" });

            migrationBuilder.DeleteData(
                table: "Workers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4804f22-8875-4a64-9c1b-1d5a3a1f1e5a");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1234b56-789c-0def-1234-567890abcdef");

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WorkerPositions",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d4804f22-8875-4a64-9c1b-1d5a3a1f1e5a", "admin-role-stamp", "Admin", "ADMIN" },
                    { "e5812f33-9966-4b99-8d5b-4d5b4d5b4d5b", "hr-role-stamp", "HR", "HR" },
                    { "f6924f44-aa77-4c88-9d6c-5d6c5d6c5d6c", "employee-role-stamp", "Employee", "EMPLOYEE" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "a1234b56-789c-0def-1234-567890abcdef", 0, "admin-concurrency-stamp", "admin@admin.com", true, false, null, "ADMIN@ADMIN.COM", "ADMIN", "AQAAAAIAAYagAAAAEKCm2i9hxAyKRj6NlWjBmd2MqhGKu6NDJLnHQENmFcRedHf8iySytHYTs1l+fwhw8Q==", null, false, "admin-security-stamp", false, "Admin" });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Head", "PhoneNumber", "Title" },
                values: new object[] { 1, "Иванов Иван", "+7-999-999-99-99", "Отдел ИТ" });

            migrationBuilder.InsertData(
                table: "WorkerPositions",
                columns: new[] { "Id", "Title" },
                values: new object[] { 1, "Администратор ИС" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "d4804f22-8875-4a64-9c1b-1d5a3a1f1e5a", "a1234b56-789c-0def-1234-567890abcdef" });

            migrationBuilder.InsertData(
                table: "Workers",
                columns: new[] { "Id", "DateBirth", "DepartmentId", "Name", "PhoneNumber", "UserId", "WorkerPositionId" },
                values: new object[] { 1, new DateTime(2003, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Моторико Владимир Дмитриевич", "+7-999-999-99-99", "a1234b56-789c-0def-1234-567890abcdef", 1 });
        }
    }
}
