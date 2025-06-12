namespace DesafioProjetoHospedagem.Models
{
    public class Reserva
    {
        public List<Pessoa> Hospedes { get; set; }
        public Suite Suite { get; set; }
        public int DiasReservados { get; set; }
        public DateTime DataCheckIn { get; set; }
        public DateTime DataCheckOut { get; set; }

        public Reserva() { }

        public Reserva(int diasReservados, DateTime checkInDate)
        {
            DiasReservados = diasReservados;
            DataCheckIn = checkInDate;
            DataCheckOut = DataCheckIn.AddDays(diasReservados);
        }

        public void CadastrarHospedes(List<Pessoa> hospedes)
        {
            // Verifica se a lista de hóspedes não é nula ou vazia
            if (hospedes == null || !hospedes.Any())
            {
                throw new ArgumentException("A lista de hóspedes não pode ser nula ou vazia.");
            }

            // Verifica se existe uma suíte cadastrada
            if (Suite == null)
            {
                throw new InvalidOperationException("É necessário cadastrar uma suíte antes de cadastrar os hóspedes.");
            }

            // Verifica se a capacidade é maior ou igual ao número de hóspedes
            if (Suite.Capacidade >= hospedes.Count)
            {
                Hospedes = hospedes;
            }
            else
            {
                // Retorna uma exception caso a capacidade seja menor que o número de hóspedes
                throw new InvalidOperationException($"A suíte comporta apenas {Suite.Capacidade} hóspede(s), mas foram informados {hospedes.Count} hóspede(s).");
            }
        }

        public void CadastrarSuite(Suite suite)
        {
            // Verifica se a suíte não é nula
            if (suite == null)
            {
                throw new ArgumentNullException(nameof(suite), "A suíte não pode ser nula.");
            }

            Suite = suite;
        }

        public int ObterQuantidadeHospedes()
        {
            // Retorna a quantidade de hóspedes (propriedade Hospedes)
            return Hospedes?.Count ?? 0;
        }

        public decimal CalcularValorDiaria()
        {
            // Cálculo: DiasReservados X Suite.ValorDiaria
            decimal valor = DiasReservados * Suite.ValorDiaria;

            decimal desconto = DiasReservados >= 10 ? valor * 0.1M : 0;

            return valor - desconto;
        }

        public string EmitirNota()
        {
            if (Suite == null)
                return "Reserva incompleta - Suíte não cadastrada";

            decimal valorOriginal = DiasReservados * Suite.ValorDiaria;
            decimal desconto = DiasReservados >= 10 ? valorOriginal * 0.1M : 0;
            decimal valorFinal = CalcularValorDiaria();
            int quantidadeHospedes = ObterQuantidadeHospedes();

            string resumo =
                "========================================\n" +
                "           COMPROVANTE DE RESERVA        \n" +
                "========================================\n\n" +

                ">> Detalhes da Suíte\n" +
                $"Suíte:           {Suite.TipoSuite}\n" +
                $"Capacidade:      {Suite.Capacidade} pessoa(s)\n" +
                $"Valor da diária: {Suite.ValorDiaria:C}\n\n" +

                ">> Período da Reserva\n" +
                $"Check-in:        {DataCheckIn:dd/MM/yyyy}\n" +
                $"Check-out:       {DataCheckOut:dd/MM/yyyy}\n" +
                $"Total de dias:   {DiasReservados}\n\n" +

                ">> Hóspedes\n" +
                $"Quantidade:      {quantidadeHospedes}\n" +
                $"{string.Join("\n", Hospedes?.Select(h => $" - {h.NomeCompleto}") ?? new[] { " - Nenhum hóspede cadastrado" })}\n\n" +

                ">> Valores\n" +
                $"Valor original:  {valorOriginal:C}\n" +
                (DiasReservados >= 10 ? $"Desconto (10%):   {desconto:C}\n" : "") +
                $"Valor final:     {valorFinal:C}\n\n" +

                "========================================\n" +
                "        Obrigado por reservar conosco!   \n" +
                "========================================\n";

            return resumo;
        }
    }
}