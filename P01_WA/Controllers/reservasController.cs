using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_WA.Models;

namespace P01_WA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public reservasController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }
        [HttpGet]
        [Route("getall")]
        public IActionResult Get()
        {
            List<reservas> listadoReservas = (from e in _equiposContexto.reservas
                                          select e).ToList();

            if (listadoReservas.Count() == 0)
            {
                return NotFound();
            }
            else
                return Ok(listadoReservas);
        }
        [HttpPost]
        [Route("add")]
        public IActionResult Crear([FromBody] reservas ReservaNew)
        {
            try
            {
                _equiposContexto.reservas.Add(ReservaNew);
                _equiposContexto.SaveChanges();

                return Ok(ReservaNew.reserva_id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("update/{id}")]
        public IActionResult updateReserva(int id, [FromBody] reservas ReservaModificar)
        {
            reservas? reservaExiste = (from e in _equiposContexto.reservas
                                   where e.reserva_id == id
                                   select e).FirstOrDefault();
            if (reservaExiste == null)
            {
                return NotFound();
            }

            reservaExiste.tiempo_reserva = ReservaModificar.tiempo_reserva;

            _equiposContexto.Entry(reservaExiste).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(reservaExiste);
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult deleteReserva(int id)
        {
            reservas? reservaExiste = (from e in _equiposContexto.reservas
                                   where e.reserva_id == id
                                   select e).FirstOrDefault();
            if (reservaExiste == null)
                return NotFound();


            _equiposContexto.Entry(reservaExiste).State = EntityState.Deleted;
            _equiposContexto.SaveChanges();

            return Ok(reservaExiste);
        }
    }
}
