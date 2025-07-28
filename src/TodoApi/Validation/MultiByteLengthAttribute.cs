using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace TodoApi.Validation
{
    /// <summary>
    /// バイト数によって文字幅を判定する属性
    /// </summary>
    public class MultiByteLengthAttribute : ValidationAttribute
    {
        private readonly int _maxByteLength;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="maxByteLength">最大バイト数</param>
        public MultiByteLengthAttribute(int maxByteLength)
        {
            _maxByteLength = maxByteLength;

            // 全角幅は 2で割って切り捨て
            int maxFullWidth = _maxByteLength / 2;

            ErrorMessage = $"全角{(maxFullWidth)}文字、半角{_maxByteLength}文字を超える入力はできません。";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // null は許容
            if (value is null) return ValidationResult.Success;

            // string 型以外はエラー
            if (value is not string input)
                return new ValidationResult("MultiByteLengthAttribute は string 型のプロパティにのみ使用できます。");

            // Unicodeコードポイントが 0x7F 以下なら半角扱い
            int length = input.Sum(c => GetCharWidth(c));

            if (length > _maxByteLength)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }

        /// <summary>
        /// 文字幅取得
        /// </summary>
        /// <param name="c">対象の文字</param>
        /// <returns>文字幅（バイト数）</returns>
        private int GetCharWidth(char c)
        {
            // Unicode East Asian Width 判定（簡易版）
            var category = CharUnicodeInfo.GetUnicodeCategory(c);

            // ASCII は常に半角
            if (c <= 0x7f) return 1;

            // 半角カナの範囲 (U+FF61 - U+FF9F)
            if (c >= '\uFF61' && c <= '\uFF9F') return 1;

            // 全角英数字・全角カナ・CJK統合漢字 → 全角
            // 環境によって変わるものは2バイト扱いとする
            return 2;
        }
    }
}