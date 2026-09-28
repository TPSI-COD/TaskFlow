using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Models
{
    public class TarefaViewModel
    {
        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(150, ErrorMessage = "O título deve ter no máximo 150 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        [Required(ErrorMessage = "A prioridade é obrigatória.")]
        public string Prioridade { get; set; } = "Média";
    }
}