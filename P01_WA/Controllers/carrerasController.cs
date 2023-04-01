using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_WA.Models;

namespace P01_WA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class carrerasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public carrerasController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }
        [HttpGet]
        [Route("getall")]
        public IActionResult Get()
        {
            List<carreras> listadoCarreras= (from e in _equiposContexto.carreras
                                                  select e).ToList();

            if (listadoCarreras.Count() == 0)
            {
                return NotFound();
            }
            else
                return Ok(listadoCarreras);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Crear([FromBody] carreras carrerasNew)
        {
            try
            {
                _equiposContexto.carreras.Add(carrerasNew);
                _equiposContexto.SaveChanges();

                return Ok(carrerasNew.facultad_id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult actualizarFacultades(int id, [FromBody] carreras CarreraModificar)
        {
            carreras? carreraExiste = (from e in _equiposContexto.carreras
                                          where e.carrera_id == id
                                          select e).FirstOrDefault();
            if (carreraExiste == null)
            {
                return NotFound();
            }
            carreraExiste.nombre_carrera = CarreraModificar.nombre_carrera;
            //carreraExiste.facultad_id = CarreraModificar.facultad_id;

            _equiposContexto.Entry(carreraExiste).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(carreraExiste);
        }
        [HttpDelete]
        [Route("delete/{id}")]

        public IActionResult deleteCarreras(int id)
        {
            carreras? carreraExiste = (from e in _equiposContexto.carreras
                                          where e.carrera_id == id
                                          select e).FirstOrDefault();
            if (carreraExiste == null)
                return NotFound();


            _equiposContexto.Entry(carreraExiste).State = EntityState.Deleted;
            _equiposContexto.SaveChanges();

            return Ok(carreraExiste);
        }
    }
}
