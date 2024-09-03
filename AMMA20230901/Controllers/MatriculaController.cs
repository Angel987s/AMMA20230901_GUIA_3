using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMMA20230901.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MatriculaController : Controller
    {
        private static List<Matricula> matriculas = new List<Matricula>();

        [HttpPost]
        public IActionResult CrearMatricula(Matricula matricula)
        {
            // Validación básica (debería ser más robusta en un entorno real)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            matriculas.Add(matricula);
            return CreatedAtAction("ObtenerMatriculaPorId", new { id = matriculas.Count - 1 }, matricula);
        }

        [HttpPut("{id}")]
        public IActionResult ModificarMatricula(int id, Matricula matricula)
        {
            var matriculaExistente = matriculas.FirstOrDefault(m => m.Id == id);
            if (matriculaExistente == null)
            {
                return NotFound();
            }

            // Actualizar los datos de la matrícula
            matriculaExistente.Alumno = matricula.Alumno;
            matriculaExistente.Curso = matricula.Curso;
            // ... actualizar otros campos según sea necesario

            return NoContent();
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerMatriculaPorId(int id)
        {
            var matricula = matriculas.FirstOrDefault(m => m.Id == id);
            if (matricula == null)
            {
                return NotFound();
            }

            return Ok(matricula);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Matricula>> ObtenerTodasLasMatriculas()
        {
            return Ok(matriculas);
        }
    }

    public class Matricula
    {
        public int Id { get; set; }
        public string Alumno { get; set; }
        public string Curso { get; set; }
    }
}
