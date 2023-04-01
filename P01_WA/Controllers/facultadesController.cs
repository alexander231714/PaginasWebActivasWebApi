using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_WA.Models;

namespace P01_WA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class facultadesController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public facultadesController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }
        [HttpGet]
        [Route("getall")]
        public IActionResult Get()
        {
            List<facultades> listadoFacultades = (from e in _equiposContexto.facultades
                                                         select e).ToList();

            if (listadoFacultades.Count() == 0)
            {
                return NotFound();
            }
            else
                return Ok(listadoFacultades);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Crear([FromBody] facultades estadoNew)
        {
            try
            {
                _equiposContexto.facultades.Add(estadoNew);
                _equiposContexto.SaveChanges();

                return Ok(estadoNew.facultad_id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult actualizarFacultades(int id, [FromBody] facultades facultadesModificar)
        {
            facultades? facultadExiste = (from e in _equiposContexto.facultades
                                                   where e.facultad_id == id
                                                   select e).FirstOrDefault();
            if (facultadExiste == null)
            {
                return NotFound();
            }
            facultadExiste.nombre_facultad = facultadesModificar.nombre_facultad;

            _equiposContexto.Entry(facultadExiste).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(facultadExiste);

        }

        [HttpDelete]
        [Route("delete/{id}")]

        public IActionResult deleteFacultades(int id)
        {
            facultades? facultadExiste = (from e in _equiposContexto.facultades
                                                 where e.facultad_id == id
                                                 select e).FirstOrDefault();
            if (facultadExiste == null)
                return NotFound();


            _equiposContexto.Entry(facultadExiste).State = EntityState.Deleted;
            _equiposContexto.SaveChanges();

            return Ok(facultadExiste);
        }
    }
}
