using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_WA.Models;

namespace P01_WA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class marcasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public marcasController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }
        [HttpGet]
        [Route("getall")]
        public IActionResult Get()
        {
            List<marcas> listadoMarcas = (from e in _equiposContexto.marcas
                                              select e).ToList();

            if (listadoMarcas.Count() == 0)
            {
                return NotFound();
            }
            else
                return Ok(listadoMarcas);
        }
        [HttpPost]
        [Route("add")]
        public IActionResult Crear([FromBody] marcas MarcasNew)
        {
            try
            {
                _equiposContexto.marcas.Add(MarcasNew);
                _equiposContexto.SaveChanges();

                return Ok(MarcasNew.id_marcas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("update/{id}")]
        public IActionResult updateMarcas(int id, [FromBody] marcas MarcasModificar)
        {
            marcas? marcaExiste = (from e in _equiposContexto.marcas
                                       where e.id_marcas == id
                                       select e).FirstOrDefault();
            if (marcaExiste == null)
            {
                return NotFound();
            }
            //marcaExiste.nombre_marca = MarcasModificar.nombre_marca;
            marcaExiste.estados = MarcasModificar.estados;

            _equiposContexto.Entry(marcaExiste).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(marcaExiste);
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult deleteMarcas(int id)
        {
            marcas? marcaExiste = (from e in _equiposContexto.marcas
                                       where e.id_marcas == id
                                       select e).FirstOrDefault();
            if (marcaExiste == null)
                return NotFound();


            _equiposContexto.Entry(marcaExiste).State = EntityState.Deleted;
            _equiposContexto.SaveChanges();

            return Ok(marcaExiste);
        }
    }
}
