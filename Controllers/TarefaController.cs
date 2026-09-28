using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Data;
using TaskFlow.Models;
using TaskFlow.Models.ViewModels;

namespace TaskFlow.Controllers
{
    [Authorize]
    public class TarefaController : Controller
    {
        private readonly AppDbContext _context;

        public TarefaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(TarefaViewModel dados)
        {
            if (!ModelState.IsValid)
            {
                return View(dados);
            }

            var usuarioId = int.Parse(
                User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier
                )!.Value
            );

            var tarefa = new Tarefa
            {
                Titulo = dados.Titulo,
                Descricao = dados.Descricao,
                Prioridade = dados.Prioridade,
                Status = "Pendente",
                DataCriacao = DateTime.Now,
                UsuarioId = usuarioId
            };

            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");

        }

            [HttpGet]
            public IActionResult Index(string? status)
            {
                var usuarioId = int.Parse(
                    User.FindFirst(
                        System.Security.Claims.ClaimTypes.NameIdentifier
                    )!.Value
                );

                var tarefas = _context.Tarefas
                    .Where(t => t.UsuarioId == usuarioId);

                if (!string.IsNullOrEmpty(status))
                {
                    tarefas = tarefas.Where(t => t.Status == status);
                }

                var resultado = tarefas
                    .OrderByDescending(t => t.DataCriacao)
                    .ToList();

                ViewBag.StatusAtual = status;

                return View(resultado);
            }

        [HttpGet]
            public IActionResult Concluir(int id)
            {
                var usuarioId = int.Parse(
                    User.FindFirst(
                        System.Security.Claims.ClaimTypes.NameIdentifier
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

                tarefa.Status = "Concluída";
                tarefa.DataConclusao = DateTime.Now;

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            [HttpGet]
            public IActionResult AlterarStatus(int id, string status)
            {
                var usuarioId = int.Parse(
                    User.FindFirst(
                        System.Security.Claims.ClaimTypes.NameIdentifier
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

                return RedirectToAction(nameof(Index));
            }


                [HttpGet]
                    public IActionResult Kanban()
                    {
                        var usuarioId = int.Parse(
                            User.FindFirst(
                                System.Security.Claims.ClaimTypes.NameIdentifier
                            )!.Value
                        );

                        var tarefas = _context.Tarefas
                            .Where(t => t.UsuarioId == usuarioId)
                            .OrderByDescending(t => t.DataCriacao)
                            .ToList();

                        return View(tarefas);
                    }



                    [HttpGet]
                public IActionResult Dashboard()
                {
                    var usuarioId = int.Parse(
                        User.FindFirst(
                            System.Security.Claims.ClaimTypes.NameIdentifier
                        )!.Value
                    );

                    var tarefas = _context.Tarefas
                        .Where(t => t.UsuarioId == usuarioId);

                    var model = new DashboardViewModel
                    {
                        TotalTarefas = tarefas.Count(),

                        Pendentes = tarefas.Count(
                            t => t.Status == "Pendente"),

                        EmAndamento = tarefas.Count(
                            t => t.Status == "Em Andamento"),

                        Concluidas = tarefas.Count(
                            t => t.Status == "Concluída")
                    };

                    return View(model);
                }

                [HttpGet]
            public IActionResult Editar(int id)
            {
                var usuarioId = int.Parse(
                    User.FindFirst(
                        System.Security.Claims.ClaimTypes.NameIdentifier
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

                var model = new TarefaViewModel
                {
                    Titulo = tarefa.Titulo,
                    Descricao = tarefa.Descricao,
                    Prioridade = tarefa.Prioridade
                };

                return View(model);
            }
                [HttpPost]
                public IActionResult Editar(int id, TarefaViewModel dados)
                {
                    if (!ModelState.IsValid)
                    {
                        return View(dados);
                    }

                    var usuarioId = int.Parse(
                        User.FindFirst(
                            System.Security.Claims.ClaimTypes.NameIdentifier
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

                    tarefa.Titulo = dados.Titulo;
                    tarefa.Descricao = dados.Descricao;
                    tarefa.Prioridade = dados.Prioridade;

                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                        [HttpPost]
                        [ValidateAntiForgeryToken]
                        public IActionResult Excluir(int id)
                        {
                            var usuarioId = int.Parse(
                                User.FindFirst(
                                    System.Security.Claims.ClaimTypes.NameIdentifier
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

                            _context.Tarefas.Remove(tarefa);
                            _context.SaveChanges();

                            return RedirectToAction(nameof(Index));
                        }

        }
    }
