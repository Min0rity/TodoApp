using Microsoft.EntityFrameworkCore;
using TodoApi;

// Webアプリ構築前の準備
var builder = WebApplication.CreateBuilder(args);

// PostgreSQL 用ドライバを使用して DbContext を登録（接続文字列は appsettings.json から取得）
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// サービスの登録

// Controllers フォルダにある API コントローラを自動検出して利用可能にする
builder.Services.AddControllers();

// 属性ルーティングのコントローラを対象にエンドポイント情報収集（Swagger 用）
builder.Services.AddEndpointsApiExplorer();

// Swagger の仕様ドキュメントを自動生成
builder.Services.AddSwaggerGen();

// Webアプリを構築
var app = builder.Build();

// 開発時のみ Swagger を有効化
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTP アクセスを HTTPS にリダイレクト
app.UseHttpsRedirection();

// コントローラのルーティング
// [ApiController] 属性付きのコントローラをエンドポイントにマッピング
app.MapControllers();

// Webサーバーを起動
app.Run();