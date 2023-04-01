using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_WA.Models;

namespace P01_WA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_equipoController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public estados_equipoController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }
        [HttpGet]
        [Route("getall")]
        public IActionResult Get()
        {
            List<estados_equipo> listadoEstado_equipo = (from e in _equiposContexto.estados_equipo
                                           select e).ToList();

            if (listadoEstado_equipo.Count() == 0)
            {
                return NotFound();
            }
            else
                return Ok(listadoEstado_equipo);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Crear([FromBody] estados_equipo estadoNew)
        {
            try
            {
                _equiposContexto.estados_equipo.Add(estadoNew);
                _equiposContexto.SaveChanges();
                // return Ok(equipoNuevo);
                return Ok(estadoNew.id_estados_equipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult actualizarEstado_equipo(int id, [FromBody] estados_equipo estado_equipoModificar)
        {
            estados_equipo? estado_equipoExiste = (from e in _equiposContexto.estados_equipo
                                     where e.id_estados_equipo == id
                                     select e).FirstOrDefault();
            if (estado_equipoExiste == null)
            {
                return NotFound();
            }

            estado_equipoExiste.descripcion = estado_equipoModificar.descripcion;
            estado_equipoExiste.estado = estado_equipoModificar.estado;

            _equiposContexto.Entry(estado_equipoExiste).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(estado_equipoExiste);

        }

        [HttpDelete]
        [Route("delete/{id}")]

        public IActionResult deleteEsta_equi(int id)
        {
            estados_equipo? estado_equiExiste = (from e in _equiposContexto.estados_equipo
                                      where e.id_estados_equipo == id
                                      select e).FirstOrDefault();
            if (estado_equiExiste == null)
                return NotFound();


            _equiposContexto.Entry(estado_equiExiste).State = EntityState.Deleted;
            _equiposContexto.SaveChanges();

            return Ok(estado_equiExiste);
        }
    }
}
