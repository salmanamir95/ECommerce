using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_category",
                columns: table => new
                {
                    Cid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__category", x => x.Cid);
                });

            migrationBuilder.CreateTable(
                name: "_user_detail",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__user_detail", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "_cart",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cid = table.Column<int>(type: "int", nullable: false),
                    Uid = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__cart", x => x.Pid);
                    table.ForeignKey(
                        name: "FK__cart__user_detail_Uid",
                        column: x => x.Uid,
                        principalTable: "_user_detail",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "_user_other_info",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__user_other_info", x => x.Uid);
                    table.ForeignKey(
                        name: "FK__user_other_info__user_detail_Uid",
                        column: x => x.Uid,
                        principalTable: "_user_detail",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "_item",
                columns: table => new
                {
                    Iid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    Cid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__item", x => x.Iid);
                    table.ForeignKey(
                        name: "FK__item__cart_CartId",
                        column: x => x.CartId,
                        principalTable: "_cart",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__item__category_Cid",
                        column: x => x.Cid,
                        principalTable: "_category",
                        principalColumn: "Cid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "_order",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uid = table.Column<int>(type: "int", nullable: false),
                    CartId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__order", x => x.Oid);
                    table.ForeignKey(
                        name: "FK__order__cart_CartId",
                        column: x => x.CartId,
                        principalTable: "_cart",
                        principalColumn: "Pid");
                    table.ForeignKey(
                        name: "FK__order__user_detail_Uid",
                        column: x => x.Uid,
                        principalTable: "_user_detail",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX__cart_Uid",
                table: "_cart",
                column: "Uid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX__item_CartId",
                table: "_item",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX__item_Cid",
                table: "_item",
                column: "Cid");

            migrationBuilder.CreateIndex(
                name: "IX__order_CartId",
                table: "_order",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX__order_Uid",
                table: "_order",
                column: "Uid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_item");

            migrationBuilder.DropTable(
                name: "_order");

            migrationBuilder.DropTable(
                name: "_user_other_info");

            migrationBuilder.DropTable(
                name: "_category");

            migrationBuilder.DropTable(
                name: "_cart");

            migrationBuilder.DropTable(
                name: "_user_detail");
        }
    }
}
