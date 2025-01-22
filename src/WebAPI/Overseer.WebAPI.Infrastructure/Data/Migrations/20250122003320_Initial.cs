using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Overseer.WebAPI.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VersioningContainers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersioningContainers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Containers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    TypedContainerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Containers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Containers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VersioningContainerVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Link = table.Column<string>(type: "text", nullable: true),
                    ContainerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersioningContainerVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VersioningContainerVersions_VersioningContainers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "VersioningContainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VersioningContainerVersionTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ContainerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersioningContainerVersionTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VersioningContainerVersionTags_VersioningContainers_Contain~",
                        column: x => x.ContainerId,
                        principalTable: "VersioningContainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VersioningContainerVersionTagValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false),
                    TagId = table.Column<int>(type: "integer", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersioningContainerVersionTagValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VersioningContainerVersionTagValues_VersioningContainerVers~",
                        column: x => x.TagId,
                        principalTable: "VersioningContainerVersionTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VersioningContainerVersionVersioningContainerVersionTagValue",
                columns: table => new
                {
                    TagValuesId = table.Column<int>(type: "integer", nullable: false),
                    VersionsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersioningContainerVersionVersioningContainerVersionTagValue", x => new { x.TagValuesId, x.VersionsId });
                    table.ForeignKey(
                        name: "FK_VersioningContainerVersionVersioningContainerVersionTagValu~",
                        column: x => x.TagValuesId,
                        principalTable: "VersioningContainerVersionTagValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VersioningContainerVersionVersioningContainerVersionTagVal~1",
                        column: x => x.VersionsId,
                        principalTable: "VersioningContainerVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Containers_ProjectId",
                table: "Containers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_VersioningContainerVersions_ContainerId",
                table: "VersioningContainerVersions",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_VersioningContainerVersionTags_ContainerId",
                table: "VersioningContainerVersionTags",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_VersioningContainerVersionTagValues_TagId",
                table: "VersioningContainerVersionTagValues",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_VersioningContainerVersionVersioningContainerVersionTagValu~",
                table: "VersioningContainerVersionVersioningContainerVersionTagValue",
                column: "VersionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Containers");

            migrationBuilder.DropTable(
                name: "VersioningContainerVersionVersioningContainerVersionTagValue");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "VersioningContainerVersionTagValues");

            migrationBuilder.DropTable(
                name: "VersioningContainerVersions");

            migrationBuilder.DropTable(
                name: "VersioningContainerVersionTags");

            migrationBuilder.DropTable(
                name: "VersioningContainers");
        }
    }
}
