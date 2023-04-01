using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_WA.Models;

namespace P01_WA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_reservaController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;
        public estados_reservaController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult Get()
        {
            List<estados_reserva> listadoEstado_reserva = (from e in _equiposContexto.estados_reserva
                                                         select e).ToList();

            if (listadoEstado_reserva.Count() == 0)
            {
                return NotFound();
            }
            else
                return Ok(listadoEstado_reserva);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Crear([FromBody] estados_reserva estadoReserva_new)
        {
            try
            {
                _equiposContexto.estados_reserva.Add(estadoReserva_new);
                _equiposContexto.SaveChanges();
                return Ok(estadoReserva_new.estado_res_id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult actualizarEstado_Reserva(int id, [FromBody] estados_reserva estado_reservaModificar)
        {
            estados_reserva? estado_reservaExiste = (from e in _equiposContexto.estados_reserva
                                                   where e.estado_res_id == id
                                                   select e).FirstOrDefault();
            if (estado_reservaExiste == null)
            {
                return NotFound();
            }

            estado_reservaExiste.estado = estado_reservaModificar.estado;

            _equiposContexto.Entry(estado_reservaExiste).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(estado_reservaExiste);
        }

        [HttpDelete]
        [Route("delete/{id}")]

        public IActionResult deleteEsta_reserva(int id)
        {
            estados_reserva? estado_reservaExiste = (from e in _equiposContexto.estados_reserva
                                                 where e.estado_res_id == id
                                                 select e).FirstOrDefault();
            if (estado_reservaExiste == null)
                return NotFound();


            _equiposContexto.Entry(estado_reservaExiste).State = EntityState.Deleted;
            _equiposContexto.SaveChanges();

            return Ok(estado_reservaExiste);
        }
    }
}
