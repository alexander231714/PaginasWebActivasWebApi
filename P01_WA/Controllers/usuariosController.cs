using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_WA.Models;

namespace P01_WA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public usuariosController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }
        [HttpGet]
        [Route("getall")]
        public IActionResult Get()
        {
            List<usuarios> listadoUsuarios = (from e in _equiposContexto.usuarios
                                              select e).ToList();

            if (listadoUsuarios.Count() == 0)
            {
                return NotFound();
            }
            else
                return Ok(listadoUsuarios);
        }
        [HttpPost]
        [Route("add")]
        public IActionResult Crear([FromBody] usuarios UsuarioNew)
        {
            try
            {
                _equiposContexto.usuarios.Add(UsuarioNew);
                _equiposContexto.SaveChanges();

                return Ok(UsuarioNew.usuario_id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("update/{id}")]
        public IActionResult updateUsuarios(int id, [FromBody] usuarios UsuarioModificar)
        {
            usuarios? usuarioExiste = (from e in _equiposContexto.usuarios
                                       where e.usuario_id == id
                                       select e).FirstOrDefault();
            if (usuarioExiste == null)
            {
                return NotFound();
            }
            usuarioExiste.nombre = UsuarioModificar.nombre;
            usuarioExiste.carnet = UsuarioModificar.carnet;
            //usuarioExiste.carrera_id = UsuarioModificar.carrera_id;

            _equiposContexto.Entry(usuarioExiste).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(usuarioExiste);
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult deleteUsuarios(int id)
        {
            usuarios? usuarioExiste = (from e in _equiposContexto.usuarios
                                       where e.usuario_id == id
                                       select e).FirstOrDefault();
            if (usuarioExiste == null)
                return NotFound();


            _equiposContexto.Entry(usuarioExiste).State = EntityState.Deleted;
            _equiposContexto.SaveChanges();

            return Ok(usuarioExiste);
        }
    }
}
