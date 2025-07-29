using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            // DIコンテナから TodoContext が渡されるため注入
            _context = context;
        }

        /// <summary>
        /// Todo を全件取得
        /// </summary>
        /// <returns>IEnumerable<TodoItem></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        /// <summary>
        /// Id に一致する Todo を取得
        /// </summary>
        /// <param name="id">一意キー</param>
        /// <returns>TodoItem</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(Guid id)
        {
            var item = await _context.TodoItems.FindAsync(id);

            if (item == null) return NotFound();

            return item;
        }

        /// <summary>
        /// Todo を追加
        /// </summary>
        /// <param name="item">TodoItem</param>
        /// <returns>追加した TodoItem</returns>
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            // 201 Created + Location を返却
            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }

        /// <summary>
        /// Id と一致する Todo を更新
        /// </summary>
        /// <param name="id">一意キー</param>
        /// <param name="item">TodoItem</param>
        /// <returns>IActionResult/returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItem item)
        {
            if (id != item.Id) return BadRequest("IDが一致しません。");
            
            // 更新日時を更新
            item.UpdatedAtUTC = DateTime.UtcNow;

            // 対象の item を更新対象として、全カラム更新
            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            // 更新時に対象の item が存在していない場合を想定
            catch (DbUpdateConcurrencyException)
            {
                // 念のため存在チェック
                if (!_context.TodoItems.Any(item => item.Id == id)) return NotFound();

                throw;
            }

            return NoContent(); // 更新成功時は204
        }

        /// <summary>
        /// Id に一致する Todo を削除
        /// </summary>
        /// <param name="id">一意キー</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {
            var item = await _context.TodoItems.FindAsync(id);

            if (item == null) return NotFound();

            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent(); // 削除成功時は204
        }
    }
}