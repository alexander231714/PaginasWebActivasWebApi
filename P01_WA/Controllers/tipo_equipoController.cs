using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_WA.Models;

namespace P01_WA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipo_equipoController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public tipo_equipoController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }
        [HttpGet]
        [Route("getall")]
        public IActionResult Get()
        {
            List<tipo_equipo> listadoTipo_equipo = (from e in _equiposContexto.tipo_equipo
                                          select e).ToList();

            if (listadoTipo_equipo.Count() == 0)
            {
                return NotFound();
            }
            else
                return Ok(listadoTipo_equipo);
        }
        [HttpPost]
        [Route("add")]
        public IActionResult Crear([FromBody] tipo_equipo TipoEquipoNew)
        {
            try
            {
                _equiposContexto.tipo_equipo.Add(TipoEquipoNew);
                _equiposContexto.SaveChanges();

                return Ok(TipoEquipoNew.id_tipo_equipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("update/{id}")]
        public IActionResult updateTipo_equipo(int id, [FromBody] tipo_equipo tipoEquipoModificar)
        {
            tipo_equipo? tipoEquipo_existe = (from e in _equiposContexto.tipo_equipo
                                   where e.id_tipo_equipo == id
                                   select e).FirstOrDefault();
            if (tipoEquipo_existe == null)
            {
                return NotFound();
            }
            tipoEquipo_existe.estado = tipoEquipoModificar.estado;
            //tipoEquipo_existe.descripcion = tipoEquipoModificar.descripcion;

            _equiposContexto.Entry(tipoEquipo_existe).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(tipoEquipo_existe);
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult deleteTipo_equipo(int id)
        {
            tipo_equipo? tipoEquipo_existe = (from e in _equiposContexto.tipo_equipo
                                   where e.id_tipo_equipo == id
                                   select e).FirstOrDefault();
            if (tipoEquipo_existe == null)
                return NotFound();


            _equiposContexto.Entry(tipoEquipo_existe).State = EntityState.Deleted;
            _equiposContexto.SaveChanges();

            return Ok(tipoEquipo_existe);
        }
    }
}
