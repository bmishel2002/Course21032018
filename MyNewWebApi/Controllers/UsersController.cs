using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyNewWebAPi.Model;

namespace MyNewWebAPi.Controllers
{
    //[Produces("application/json")]
    [Route("api/Users")]
    [EnableCors("MyPolicy")]
    public class UsersController : Controller
    {
        private readonly UsersContext _context;

        public UsersController(UsersContext context)
        {
            _context = context;
        }

        // GET: api/Users
        // Получение всех сущностей
        [HttpGet]
        [FormatFilter]
        public IEnumerable<User> GetUsers()
        {
            // Возвращаем все сущности модели
            return _context.Users;
        }

        // GET: api/Users/5
        // Получение сущности по Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            // Отклонение запроса, если его модель не прошла валидацию
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Поиск сущности по Id
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);

            // Сущность не найдена
            if (user == null)
            {
                return NotFound();
            }

            // Возвращаем сущность
            return Ok(user);
        }

        // PUT: api/Users/5
        // Полное обновление сущности (необходима передача всех полей)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] int id,[FromBody] User user)
        {

            // Отклонение запроса, если его модель не прошла валидацию
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Отклонение запроса, если Id сущности в маршруте и теле запроса не совпадают
            if (id != user.Id)
            {
                return BadRequest();
            }

            // Обновление сущности
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                // Сохраняем изменения и ожидаем результат
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                // Выдача ошибки при отсутствии сущности в БД
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Успешно выполненный метод не возвращает контента
            return NoContent();
        }

        // PUT: api/Users/5
        // Частичное обновление сущности (необходима передача изменяемых полей)
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUser([FromRoute] int id, [FromBody] User user)
        {

            // Отклонение запроса, если его модель не прошла валидацию
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Поиск сущности в БД по Id из маршрута (поле Id в теле запроса может быть пропущено)
                var userInDb = _context.Users.FirstOrDefault(x => x.Id == id);

                // Выдача ошибки при отсутствии сущности в БД
                if (userInDb == null)
                {
                    return NotFound();
                }

                // Если в теле запроса передано поле Name, проводится изменение в сущности из БД
                if (!string.IsNullOrEmpty(user.Name))
                {
                    userInDb.Name = user.Name;
                }

                // Если в теле запроса передано поле Age, проводится изменение в сущности из БД
                if (user.Age > 0)
                {
                    userInDb.Age = user.Age;
                }

                // Сохранение изменений
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                // Выдача ошибки при отсутствии сущности в БД
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Успешно выполненный метод не возвращает контента
            return NoContent();
        }

        // POST: api/Users
        // Добавление сущности в БД  (необходима передача всех обязательных полей кроме Id)
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            // Отклонение запроса, если его модель не прошла валидацию
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // =================== Добавил на свое усмотрение для устранения критической ошибки ================
            // Если в теле запроса передано значение поля Id - выдавать ошибку
            if ( user.Id != 0 )
            {
                return BadRequest();
            }
            // =================== Добавил на свое усмотрение для устранения критической ошибки ================

            // Создаем пользователя и сохраняем изменения
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Возвращаем код успешного создания и созданную сущность
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }
        
        // DELETE: api/Users/5
        // Удаление сущности по Id из маршрута
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            // Отклонение запроса, если его модель не прошла валидацию
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Поиск сущности по Id
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);

            // Если сущность не найдена
            if (user == null)
            {
                return NotFound();
            }

            // Удаление сущности и сохранение изменений
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            // Возвращаем удаленную сущность
            return Ok(user);
        }

        [HttpOptions]
        public string OptionsUsers()
        {
            return "GET, PUT, POST, DELETE";
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}