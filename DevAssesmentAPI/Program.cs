using System.Text;
using DevAssesmentAPI;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ConfidentialInfoDB>(opt => opt.UseInMemoryDatabase("ConfidentialInfoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();


app.MapPost("/HashedConfidentialList", async (ConfidentialInfoModel confidentialInfo, ConfidentialInfoDB db) =>
{
    static string HashConfidentialString(string rawConfidentialString) {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawConfidentialString));

            StringBuilder hashedString = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                hashedString.Append(bytes[i].ToString("x2"));
            }
            return hashedString.ToString();
        }
    }

    string secretString = confidentialInfo.SecretInformation is null ? "" : confidentialInfo.SecretInformation;
    confidentialInfo.SecretInformation = HashConfidentialString(secretString);
    db.ConfidentialInfos.Add(confidentialInfo);
    await db.SaveChangesAsync();

    return Results.Created($"/HashedConfidentialList/{confidentialInfo.SecretInformation}", confidentialInfo);
});

app.Run();