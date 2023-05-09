using _01_EFCore.Entities;
using _01_EFCore.Models;
using MFramework.Services.FakeData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace _01_EFCore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private ToDoDbContext _db;

        public ToDoController(ToDoDbContext db)
        {
            _db = db;
        }

        [HttpGet("generate-fakedata")]
        public IActionResult GenerateFakeData()
        {
            if (_db.ToDos.Any())
            {
                return Ok("Geçici Veriler Zaten Oluşturuldu!");
            }
            for (int i = 1; i < 21; i++)
            {
                _db.ToDos.Add(new ToDo
                {
                    Header = TextData.GetSentence(),
                    IsCompleted = BooleanData.GetBoolean(),
                    Description = TextData.GetSentence(),

                });
            }

            _db.SaveChanges();
            return Ok("ok");
        }

        [HttpGet("list")]
        //[ProducesResponseType((int)HttpStatusCode.OK, Type= typeof(List<ToDo>))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<ToDoResponse>))]
        public IActionResult List()
        {
            //return Ok(_db.ToDos.ToList());

            //1.Map Etme Yöntemi

            //List<ToDo> list = _db.ToDos.ToList();
            //List<ToDoResponse> result = new List<ToDoResponse>();

            //foreach (ToDo toDo in list)
            //{
            //    result.Add(new ToDoResponse
            //    {
            //        Id = toDo.Id,
            //        Header = toDo.Header,
            //        Description = toDo.Description,
            //        IsCompleted = toDo.IsCompleted,
            //    });
            //}

            //return Ok(result);

            //2. Yöntem
            List<ToDoResponse> result = _db.ToDos.Select(x => new ToDoResponse
            {
                Description = x.Description,
                Header = x.Header,
                Id = x.Id,
                IsCompleted = x.IsCompleted
            }).ToList();

            return Ok(result);

            //!!!!AUTOMAPPER ARAŞTIRILACAK!!!!
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ToDoResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public IActionResult Create([FromBody] ToDoCreateModel model)
        {

            //if (ModelState.IsValid)
            //{
            ToDo toDo = new ToDo
            {
                Description = model.Description,
                Header = model.Header,
                IsCompleted = false
            };

            _db.ToDos.Add(toDo);
            int affectedRows = _db.SaveChanges();
            if (affectedRows > 0)
            {
                ToDoResponse result = new ToDoResponse
                {
                    Id = toDo.Id,
                    Description = toDo.Description,
                    Header = toDo.Header,
                    IsCompleted = toDo.IsCompleted
                };

                return Created(string.Empty, result);
            }
            else
            {
                return BadRequest("Kayıt Yapılamadı");
            }
            //}
            //else
            //{
            //    return BadRequest(ModelState);
            //}

        }

        [HttpGet("list/{id}")]
        //[ProducesResponseType((int)HttpStatusCode.OK, Type= typeof(List<ToDo>))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<ToDoResponse>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        public IActionResult ListId([FromRoute] int id)
        {
            ToDo todo = _db.ToDos.Find(id);

            if (todo == null)
            {
                return NotFound("Kayıt Bulunamadı");
            }

            else
            {
                ToDoResponse result = new ToDoResponse
                {
                    Id = todo.Id,
                    Description = todo.Description,
                    Header = todo.Header,
                    IsCompleted = todo.IsCompleted
                };

                return Ok(result);

            }


        }


        [HttpPut("edit/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ToDoResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]

        public IActionResult Update([FromRoute] int id, [FromBody] ToDoUpdateModel model)
        {
            ToDo todo = _db.ToDos.Find(id);

            if (todo == null)
            {
                return NotFound("Kayıt Bulunamadı");
            }
            else
            {
                todo.Description = model.Description;
                todo.Header = model.Header;
                todo.IsCompleted = model.IsCompleted;

                int affectedRows = _db.SaveChanges();
                if (affectedRows > 0)
                {
                    ToDoResponse result = new ToDoResponse
                    {
                        Id = todo.Id,
                        Description = todo.Description,
                        Header = todo.Header,
                        IsCompleted = todo.IsCompleted
                    };

                    return Ok(result);
                }
                else
                {
                    return BadRequest("Güncelleme Yapılamadı");
                }
            }
        }


        [HttpPatch("change-state/{id}/{isCompleted}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ToDoResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        public IActionResult UpdateIsCompleted([FromRoute] int id, [FromRoute] bool isCompleted)
        {
            ToDo toDo = _db.ToDos.Find(id);

            if (toDo == null)
            {
                return NotFound("Kayıt Bulunamadı");
            }

            toDo.IsCompleted = isCompleted;
            int affectedRows = _db.SaveChanges();
            if (affectedRows > 0)
            {
                ToDoResponse toDoResponse = new ToDoResponse
                {
                    Description = toDo.Description,
                    Header = toDo.Header,
                    IsCompleted = toDo.IsCompleted,
                    Id = toDo.Id
                };
                return Ok(toDoResponse);
            }
            else
            {
                return BadRequest("Kayıt Güncellenemedi!");
            }
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ToDoResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(string))]
        public IActionResult Delete([FromRoute] int id)
        {
            ToDo toDo = _db.ToDos.Find(id);

            if (toDo == null)
            {
                return NotFound("Kayıt Bulunamadı");
            }

            _db.ToDos.Remove(toDo);
            int affectedRows = _db.SaveChanges();
            if (affectedRows > 0)
            {
                return Ok("Kayıt Silindi!");
            }
            else
            {
                return BadRequest("Kayıt Silinemedi!");
            }
        }

        [HttpDelete("deleteAll")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(string))]
        public IActionResult DeleteAll()
        {
            List<ToDo> result = _db.ToDos.Select(t => new ToDo
            {
                Description = t.Description,
                Header = t.Header,
                Id = t.Id,
                IsCompleted = t.IsCompleted,

            }).ToList();


            foreach (ToDo t in result)
            {
                _db.ToDos.Remove(t);
            }

            _db.SaveChanges();
            int affectedRows = _db.SaveChanges();
            if (affectedRows > 0)
            {
                return Ok("Kayıt Silindi!");
            }
            else
            {
                return BadRequest("Kayıt Bulunamadı!");
            }

        
        }
    }
}
