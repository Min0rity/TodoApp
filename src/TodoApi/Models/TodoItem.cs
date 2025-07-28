using System.ComponentModel.DataAnnotations;
using TodoApi.Validation;

namespace TodoApi.Models
{
    /// <summary>
    /// Todoのタスク
    /// </summary>
    public class TodoItem
    {
        /// <summary>
        /// 一意キー
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// タスクのタイトル
        /// </summary>
        [Required]
        [MultiByteLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 終了期限日時（UTCで保存）
        /// </summary>
        public DateTime? DueAtUTC { get; set; }

        /// <summary>
        /// 日時のタイムゾーン（例: Asia/Tokyo）
        /// </summary>
        /// <remarks>グローバル利用を想定（夏時間にも対応）</remarks>
        public string? TimeZoneId { get; set; } = TimeZoneInfo.Local.Id;

        /// <summary>
        /// 備考
        /// </summary>
        public string? Note { get; set; }

        /// <summary>
        /// タスク完了済みか
        /// </summary>
        public bool IsCompleted { get; set; }
    }
}