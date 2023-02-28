using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TodoList.Postgre.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "todo_tasks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    parenttaskid = table.Column<int>(name: "parent_task_id", type: "integer", nullable: true),
                    summary = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    duedate = table.Column<DateTime>(name: "due_date", type: "timestamp with time zone", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_todo_tasks", x => x.id);
                    table.ForeignKey(
                        name: "FK_todo_tasks_todo_tasks_parent_task_id",
                        column: x => x.parenttaskid,
                        principalTable: "todo_tasks",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_todo_tasks_parent_task_id",
                table: "todo_tasks",
                column: "parent_task_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "todo_tasks");
        }
    }
}
