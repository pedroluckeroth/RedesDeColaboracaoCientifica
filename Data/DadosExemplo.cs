using AnaliseRedesColaboracao.Graph;

namespace AnaliseRedesColaboracao.Data;

public static class DadosExemplo
{
    public static RedeCoautoria CriarRede()
    {
        var rede = new RedeCoautoria();

        Adicionar("Ana Martins", "Bruno Costa", "Carla Souza");
        Adicionar("Ana Martins", "Bruno Costa", "Carla Souza");
        Adicionar("Bruno Costa", "Carla Souza", "Daniel Lima");
        Adicionar("Elisa Rocha", "Felipe Alves", "Gabriela Nunes");
        Adicionar("Elisa Rocha", "Felipe Alves", "Gabriela Nunes");
        Adicionar("Daniel Lima", "Gabriela Nunes", "Hugo Reis");
        Adicionar("Ana Martins", "Daniel Lima");
        Adicionar("Felipe Alves", "Hugo Reis");

        return rede;

        void Adicionar(params string[] nomesAutores)
        {
            var (titulo, ano, area) = ObterMetadados(rede.Publicacoes.Count + 1);
            var autoresIds = nomesAutores
                .Select(rede.ObterOuCriarAutor)
                .Select(autor => autor.Id);

            rede.AdicionarPublicacao(titulo, ano, area, autoresIds);
        }
    }

    private static (string Titulo, int Ano, string Area) ObterMetadados(int indice)
    {
        return indice switch
        {
            1 => ("Mineracao de dados aplicada a saude", 2021, "Ciencia de Dados"),
            2 => ("Modelos preditivos em sistemas de saude", 2022, "Inteligencia Artificial"),
            3 => ("Aprendizado de maquina explicavel", 2023, "Inteligencia Artificial"),
            4 => ("Arquiteturas para sistemas distribuidos", 2021, "Sistemas Distribuidos"),
            5 => ("Seguranca e comunicacao em IoT", 2022, "Internet das Coisas"),
            6 => ("Analise de dados em redes inteligentes", 2024, "Ciencia de Dados"),
            7 => ("Predicao e classificacao de dados", 2024, "Inteligencia Artificial"),
            8 => ("Dispositivos conectados e processamento", 2025, "Internet das Coisas"),
            _ => throw new ArgumentOutOfRangeException(nameof(indice))
        };
    }
}
