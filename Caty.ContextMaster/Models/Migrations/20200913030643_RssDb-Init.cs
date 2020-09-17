using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Caty.ContextMaster.Models.Migrations
{
    public partial class RssDbInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RssFeeds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(nullable: false),
                    Generator = table.Column<string>(nullable: true),
                    FeedId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RssFeeds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RssSources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    RssName = table.Column<string>(nullable: true),
                    RssUrl = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RssSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RssItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    AuthorLink = table.Column<string>(nullable: true),
                    AuthorEmail = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(nullable: false),
                    ItemId = table.Column<string>(nullable: true),
                    ContentLink = table.Column<string>(nullable: true),
                    FeedId = table.Column<string>(nullable: true),
                    FeedId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RssItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RssItems_RssFeeds_FeedId1",
                        column: x => x.FeedId1,
                        principalTable: "RssFeeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RssItems_FeedId1",
                table: "RssItems",
                column: "FeedId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RssItems");

            migrationBuilder.DropTable(
                name: "RssSources");

            migrationBuilder.DropTable(
                name: "RssFeeds");
        }
    }
}
