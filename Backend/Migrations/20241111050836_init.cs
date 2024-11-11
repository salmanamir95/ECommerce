using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
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
                name: "_user_other_info",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Uid = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__user_other_info", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "_user_detail",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserOtherInfoEmail = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__user_detail", x => x.Uid);
                    table.ForeignKey(
                        name: "FK__user_detail__user_other_info_UserOtherInfoEmail",
                        column: x => x.UserOtherInfoEmail,
                        principalTable: "_user_other_info",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateTable(
                name: "_cart",
                columns: table => new
                {
                    cid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uid = table.Column<int>(type: "int", nullable: false),
                    userUid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__cart", x => x.cid);
                    table.ForeignKey(
                        name: "FK__cart__user_detail_userUid",
                        column: x => x.userUid,
                        principalTable: "_user_detail",
                        principalColumn: "Uid");
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
                    Cid = table.Column<int>(type: "int", nullable: false),
                    Cartcid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__item", x => x.Iid);
                    table.ForeignKey(
                        name: "FK__item__cart_Cartcid",
                        column: x => x.Cartcid,
                        principalTable: "_cart",
                        principalColumn: "cid");
                    table.ForeignKey(
                        name: "FK__item__category_Cid",
                        column: x => x.Cid,
                        principalTable: "_category",
                        principalColumn: "Cid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "_order",
                columns: table => new
                {
                    Oid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uid = table.Column<int>(type: "int", nullable: false),
                    Cid = table.Column<int>(type: "int", nullable: false),
                    userUid = table.Column<int>(type: "int", nullable: true),
                    cartcid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__order", x => x.Oid);
                    table.ForeignKey(
                        name: "FK__order__cart_cartcid",
                        column: x => x.cartcid,
                        principalTable: "_cart",
                        principalColumn: "cid");
                    table.ForeignKey(
                        name: "FK__order__user_detail_userUid",
                        column: x => x.userUid,
                        principalTable: "_user_detail",
                        principalColumn: "Uid");
                });

            migrationBuilder.CreateIndex(
                name: "IX__cart_userUid",
                table: "_cart",
                column: "userUid");

            migrationBuilder.CreateIndex(
                name: "IX__item_Cartcid",
                table: "_item",
                column: "Cartcid");

            migrationBuilder.CreateIndex(
                name: "IX__item_Cid",
                table: "_item",
                column: "Cid");

            migrationBuilder.CreateIndex(
                name: "IX__order_cartcid",
                table: "_order",
                column: "cartcid");

            migrationBuilder.CreateIndex(
                name: "IX__order_userUid",
                table: "_order",
                column: "userUid");

            migrationBuilder.CreateIndex(
                name: "IX__user_detail_UserOtherInfoEmail",
                table: "_user_detail",
                column: "UserOtherInfoEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_item");

            migrationBuilder.DropTable(
                name: "_order");

            migrationBuilder.DropTable(
                name: "_category");

            migrationBuilder.DropTable(
                name: "_cart");

            migrationBuilder.DropTable(
                name: "_user_detail");

            migrationBuilder.DropTable(
                name: "_user_other_info");
        }
    }
}
