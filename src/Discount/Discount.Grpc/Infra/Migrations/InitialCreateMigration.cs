using FluentMigrator;

namespace Discount.Grpc.Infra.Migrations
{
    [Migration(1, "InitialCreate")]
    public class InitialCreateMigration : Migration
    {
        public override void Up()
        {
            Create.Table("Coupon")
                .InSchema("public")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("ProductName").AsString(24).NotNullable()
                .WithColumn("Description").AsCustom("TEXT")
                .WithColumn("Amount").AsInt32();

            Insert.IntoTable("Coupon")
                .Row(new {ProductName = "IPhone X", Description = "IPhone Discount", Amount = 150})
                .Row(new {ProductName = "Samsung 10", Description = "Samsung Discount", Amount = 100});
        }

        public override void Down()
        {
            Delete.Table("Coupon");
        }
    }
}
