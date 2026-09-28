namespace TaskFlow.Models
{
    public class Tarefa
    {
        public int Id { get; set; }

        public required string Titulo { get; set; }

        public string? Descricao { get; set; }

        public string Status { get; set; } = "Pendente";

        public string Prioridade { get; set; } = "Média";

        public DateTime DataCriacao { get; set; }

        public DateTime? DataConclusao { get; set; }

        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }
    }
}