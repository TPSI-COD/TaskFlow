using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskFlow.Data;

namespace TaskFlow.Controllers
{
    [ApiController]
    [Route("api/tarefas")]
    [Authorize]
    public class TarefaApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TarefaApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPut("{id}/status")]
        public IActionResult AlterarStatus(int id, [FromBody] string status)
        {
            var usuarioId = int.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier
                )!.Value
            );

            var tarefa = _context.Tarefas
                .FirstOrDefault(t =>
                    t.Id == id &&
                    t.UsuarioId == usuarioId);

            if (tarefa == null)
            {
                return NotFound();
            }

            if (status != "Pendente" &&
                status != "Em Andamento" &&
                status != "Concluída")
            {
                return BadRequest("Status inválido.");
            }

            tarefa.Status = status;

            if (status == "Concluída")
            {
                tarefa.DataConclusao = DateTime.Now;
            }
            else
            {
                tarefa.DataConclusao = null;
            }

            _context.SaveChanges();

            return Ok(new
            {
                mensagem = "Status atualizado com sucesso.",
                tarefaId = tarefa.Id,
                status = tarefa.Status
            });
        }
    }
}