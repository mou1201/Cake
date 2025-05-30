using CakeShop.Models;

namespace CakeShop.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // 檢查資料庫是否存在，如果不存在則建立
            context.Database.EnsureCreated();

            // 檢查是否已有蛋糕資料     
            if (context.Cakes.Any())     
            {
                return;   // DB has been seeded
            }
            var cakes = new Cake[]
                        {
                new Cake{Name="黑森林蛋糕", Description="濃郁滑順的巧克力", Price=550.00m, ImageUrl="/images/black forest.png"},
                new Cake{Name="繽紛樂園蛋糕", Description="新鮮水果搭配輕盈鮮奶油", Price=620.00m, ImageUrl="/images/colorful paradise.png"},
                new Cake{Name="檸檬香露蛋糕", Description="清爽檸檬風味的重乳酪蛋糕", Price=580.00m, ImageUrl="/images/lemon sherbet.png"},
                new Cake{Name="緹娜布朗", Description=" 法式栗子泥與瑪斯卡邦乳酪慕斯的完美結合", Price=650.00m, ImageUrl="/images/tina brown.png"},
                new Cake{Name="提拉米蘇", Description="義大利經典咖啡酒香甜點", Price=500.00m, ImageUrl="/images/tiramisu.png"}
                        };

            foreach (Cake c in cakes)
            {
                context.Cakes.Add(c);
            }
            context.SaveChanges(); // 儲存蛋糕資料

            // 可以在這裡加入其他的初始資料，例如管理員帳號等
        }
    }
}

