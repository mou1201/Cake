using CakeShop.Data;
using CakeShop.Models; // �T�O using �F�A���ҫ��R�W�Ŷ�
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)); // �Ϊ� UseSqlite, UseNpgsql ��
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ====================> �ˬd�íק�o�@�϶� <====================
// **���I�G** �T�O AddDefaultIdentity �� AddIdentity ���x���ѼƬO ApplicationUser
builder.Services.AddDefaultIdentity<ApplicationUser>(options => { // <-- ���w ApplicationUser
                                                                  // �b�o�̳]�w Identity �ﶵ�A�Ҧp�O�_�ݭn�T�{ Email�A�K�X�W�h��
    options.SignIn.RequireConfirmedAccount = false; // �}�o�ɳ]�� false ����K
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
    // **���I�G** �T�O�s����A�� ApplicationDbContext
    .AddEntityFrameworkStores<ApplicationDbContext>();
// ==============================================================

// �p�G�A�ݭn�ϥΨ���޲z (Roles)�A�h��� AddIdentity
// builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => { // <-- ���w ApplicationUser �M IdentityRole
//         options.SignIn.RequireConfirmedAccount = false;
//         // ... ��L�ﶵ ...
//     })
//     .AddEntityFrameworkStores<ApplicationDbContext>()
//     .AddDefaultTokenProviders(); // AddIdentity �q�`�ݭn�o��

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // �p�G�A���ϥ� Razor Pages (�� Identity UI �N�O)�A�ݭn�o��

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ====================> �ˬd Middleware ���� <====================
// **���I�G** UseAuthentication �����b UseAuthorization ���e
app.UseAuthentication(); // �ҥ����ҥ\��
app.UseAuthorization(); // �ҥα��v�\��
// ==============================================================


// **���I�G** �T�O MapRazorPages() �Q�I�s�A�o�� Identity �������~��B�@
app.MapRazorPages(); // �M�g Identity UI (Razor Pages) ������

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ��Ʈw��l�� (�p�G������)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        // context.Database.Migrate(); // �i��
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}


app.Run();