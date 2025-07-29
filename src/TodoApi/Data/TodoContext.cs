using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }

        // TodoItems テーブルにアクセスするためのプロパティ。
        // TodoItem クラスを テーブルと対応付ける。
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}