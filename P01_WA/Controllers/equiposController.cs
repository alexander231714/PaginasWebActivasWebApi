using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01_WA.Models;
using Microsoft.EntityFrameworkCore;

namespace P01_WA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public equiposController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }
        [HttpGet]
        [Route("getall")]
        public IActionResult Get()
        {
            List<equipos> listadoEquipo = (from e in _equiposContexto.equipos
                                           select e).ToList();

            if(listadoEquipo.Count() == 0)
            {
                return NotFound();
            }
            else
                return Ok(listadoEquipo);
        }

        // localhost:4455/api/equipos/getbyid?id=23&nombre=pwa
        // localhost:4455/api/equipos/getbyi/23/pwa
        [HttpGet]
        [Route("getbyid/{id}")]
     
        public IActionResult Get(int id)
        {
            equipos? unEquipo = (from e in _equiposContexto.equipos
                          where e.id_equipos == id
                          select e).FirstOrDefault();
            if(unEquipo == null)
                return NotFound();
            return Ok(unEquipo);
        }

        [HttpGet]
        [Route("find")]
        public IActionResult buscar(string filtro)
        {
            List<equipos>equiposList = (from e in _equiposContexto.equipos
                                        where e.nombre.Contains(filtro)
                                        || e.descripcion.Contains(filtro)
                                        select e).ToList();
           // if(equiposList.Count==0)
           // {
           //     return NotFound();
           // }
           // return Ok(equiposList);
           if(equiposList.Any())
            {
                return Ok(equiposList);
            }
           return NotFound();
        }
        [HttpPost]
        [Route("add")]
        public IActionResult Crear([FromBody] equipos equipoNuevo)
        {
            try
            {
                equipoNuevo.estado = "A";
                _equiposContexto.equipos.Add(equipoNuevo);
                _equiposContexto.SaveChanges();
               // return Ok(equipoNuevo);
                return Ok(equipoNuevo.id_equipos);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult actualizarEquipo(int id, [FromBody] equipos equipoModificar)
        {
            equipos? equipoExiste = (from e in _equiposContexto.equipos
                                     where e.id_equipos == id
                                     select e).FirstOrDefault();
            if(equipoExiste == null)
            {
                return NotFound();
            }

            equipoExiste.nombre = equipoModificar.nombre;
            equipoExiste.descripcion = equipoModificar.descripcion;

            _equiposContexto.Entry(equipoExiste).State = EntityState.Modified;
            _equiposContexto.SaveChanges();
            
            return Ok(equipoExiste);
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult eliminarEquipo(int id)
        {
            equipos? equipoExiste = (from e in _equiposContexto.equipos
                                     where e.id_equipos == id
                                     select e).FirstOrDefault();
            if (equipoExiste == null) { 
                return NotFound(); }
            equipoExiste.estado= "I";
            //_equiposContexto.equipos.Attach(equipoExiste);
            //_equiposContexto.equipos.Remove(equipoExiste);
            _equiposContexto.SaveChanges();

            return Ok(equipoExiste);

        }
    }
}
