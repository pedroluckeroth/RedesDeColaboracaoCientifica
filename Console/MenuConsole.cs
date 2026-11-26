using System.Globalization;
using System.Text;
using AnaliseRedesColaboracao.Algorithms;
using AnaliseRedesColaboracao.Graph;

namespace AnaliseRedesColaboracao.ConsoleApp;

public sealed class MenuConsole
{
    private readonly RedeCoautoria _rede;

    public MenuConsole(RedeCoautoria rede)
    {
        _rede = rede;
    }

    public void Executar()
    {
        var opcao = -1;

        do
        {
            Console.WriteLine(CriarMenu());
            Console.Write("Escolha uma opcao: ");
            var entrada = Console.ReadLine();

            if (!int.TryParse(entrada, out opcao))
            {
                Console.WriteLine("Opcao invalida. Digite um numero do menu.");
                opcao = -1;
            }
            else
            {
                switch (opcao)
                {
                    case 1:
                        ExibirResumo();
                        break;
                    case 2:
                        CadastrarPublicacao();
                        break;
                    case 3:
                        ExibirAutoresCentrais();
                        break;
                    case 4:
                        ExibirComunidades();
                        break;
                    case 5:
                        ExibirConexoesEntreAreas();
                        break;
                    case 6:
                        ExibirEvolucaoAnual();
                        break;
                    case 7:
                        AnalisarPeriodo();
                        break;
                    case 0:
                        Console.WriteLine("Encerrando a ferramenta. Ate mais!");
                        break;
                    default:
                        Console.WriteLine("Opcao inexistente. Tente novamente.");
                        break;
                }
            }

            if (opcao != 0)
                Pausar();

        } while (opcao != 0);
    }

    private static string CriarMenu()
    {
        var menu = new StringBuilder();
        menu.AppendLine();
        menu.AppendLine("===============================================");
        menu.AppendLine("  ANALISE DE REDES DE COLABORACAO CIENTIFICA");
        menu.AppendLine("===============================================");
        menu.AppendLine("1 - Resumo da rede");
        menu.AppendLine("2 - Cadastrar publicacao");
        menu.AppendLine("3 - Autores centrais");
        menu.AppendLine("4 - Identificar comunidades");
        menu.AppendLine("5 - Areas de conhecimento conectadas");
        menu.AppendLine("6 - Evolucao anual da rede");
        menu.AppendLine("7 - Analisar um periodo");
        menu.AppendLine("0 - Sair");
        menu.AppendLine("-----------------------------------------------");
        return menu.ToString();
    }

    private void ExibirResumo()
    {
        var componentes = AnaliseRede.ObterComponentesConexas(_rede);
        Console.WriteLine();
        Console.WriteLine("RESUMO DA REDE");
        Console.WriteLine($"Autores: { _rede.Autores.Count }");
        Console.WriteLine($"Publicacoes: {_rede.Publicacoes.Count}");
        Console.WriteLine($"Relacoes de coautoria: {_rede.Arestas.Count}");
        Console.WriteLine($"Componentes conexas: {componentes.Count}");
        Console.WriteLine("Observacao: cada aresta representa pelo menos uma publicacao compartilhada.");
    }

    private void CadastrarPublicacao()
    {
        Console.WriteLine();
        Console.WriteLine("CADASTRO DE PUBLICACAO");
        var titulo = LerTextoObrigatorio("Titulo: ");
        var ano = LerAno("Ano: ");
        var area = LerTextoObrigatorio("Area de conhecimento: ");
        var nomes = LerTextoObrigatorio("Autores (separe os nomes por virgula): ")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(nome => !string.IsNullOrWhiteSpace(nome))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (nomes.Count == 0)
        {
            Console.WriteLine("Nenhum autor foi informado. Publicacao nao cadastrada.");
            return;
        }

        var autoresIds = nomes
            .Select(_rede.ObterOuCriarAutor)
            .Select(autor => autor.Id)
            .ToList();

        _rede.AdicionarPublicacao(titulo, ano, area, autoresIds);
        Console.WriteLine("Publicacao cadastrada e rede atualizada com sucesso.");
    }

    private void ExibirAutoresCentrais()
    {
        Console.WriteLine();
        Console.WriteLine("AUTORES COM MAIOR CENTRALIDADE DE GRAU");

        var ranking = AnaliseRede.ObterRankingCentralidade(_rede).Take(10).ToList();
        if (ranking.Count == 0)
        {
            Console.WriteLine("Ainda nao existem autores cadastrados.");
            return;
        }

        var posicao = 1;
        foreach (var resultado in ranking)
        {
            Console.WriteLine(
                $"{posicao}. {resultado.Autor.Nome} | grau: {resultado.Grau} | " +
                $"colaboracoes acumuladas: {resultado.GrauPonderado} | " +
                $"centralidade: {resultado.CentralidadeDeGrau.ToString("P1", CultureInfo.InvariantCulture)}");
            posicao++;
        }
    }

    private void ExibirComunidades()
    {
        Console.WriteLine();
        Console.WriteLine("COMUNIDADES - PROPAGACAO DE ROTULOS");
        var comunidades = AnaliseRede.DetectarComunidadesPorPropagacaoDeRotulos(_rede);

        if (comunidades.Count == 0)
        {
            Console.WriteLine("Ainda nao existem autores cadastrados.");
            return;
        }

        for (var i = 0; i < comunidades.Count; i++)
        {
            var nomes = string.Join(", ", comunidades[i].Select(autor => autor.Nome));
            Console.WriteLine($"Comunidade {i + 1} ({comunidades[i].Count} autores): {nomes}");
        }

        Console.WriteLine("Nota: propagacao de rotulos e uma heuristica; os grupos dependem da estrutura da rede.");
    }

    private void ExibirConexoesEntreAreas()
    {
        Console.WriteLine();
        Console.WriteLine("AREAS CONECTADAS POR AUTORES MULTIDISCIPLINARES");
        var conexoes = AnaliseRede.ObterConexoesEntreAreas(_rede);

        if (conexoes.Count == 0)
        {
            Console.WriteLine("Nao ha autores publicados em mais de uma area.");
            return;
        }

        foreach (var conexao in conexoes)
        {
            Console.WriteLine(
                $"{conexao.AreaA} <-> {conexao.AreaB} | " +
                $"autores em comum: {conexao.AutoresCompartilhados}");
        }
    }

    private void ExibirEvolucaoAnual()
    {
        Console.WriteLine();
        Console.WriteLine("EVOLUCAO ANUAL");
        var anos = _rede.Publicacoes
            .Select(publicacao => publicacao.Ano)
            .Distinct()
            .OrderBy(ano => ano)
            .ToList();

        if (anos.Count == 0)
        {
            Console.WriteLine("Nao existem publicacoes cadastradas.");
            return;
        }

        Console.WriteLine("Ano | Autores ativos | Publicacoes | Relacoes de coautoria");
        foreach (var ano in anos)
        {
            var recorte = _rede.CriarVisaoTemporal(ano, ano);
            Console.WriteLine(
                $"{ano} | {recorte.Autores.Count} | {recorte.Publicacoes.Count} | {recorte.Arestas.Count}");
        }
    }

    private void AnalisarPeriodo()
    {
        Console.WriteLine();
        Console.WriteLine("ANALISE POR PERIODO");
        var anoInicial = LerAno("Ano inicial: ");
        var anoFinal = LerAno("Ano final: ");

        if (anoInicial > anoFinal)
        {
            Console.WriteLine("Periodo invalido: o ano inicial deve ser menor ou igual ao ano final.");
            return;
        }

        var recorte = _rede.CriarVisaoTemporal(anoInicial, anoFinal);
        Console.WriteLine($"Periodo: {anoInicial} a {anoFinal}");
        Console.WriteLine($"Autores ativos: {recorte.Autores.Count}");
        Console.WriteLine($"Publicacoes: {recorte.Publicacoes.Count}");
        Console.WriteLine($"Relacoes de coautoria: {recorte.Arestas.Count}");

        var centrais = AnaliseRede.ObterRankingCentralidade(recorte).Take(3).ToList();
        if (centrais.Count > 0)
        {
            Console.WriteLine("Autores com maior centralidade no periodo:");
            foreach (var resultado in centrais)
                Console.WriteLine($"- {resultado.Autor.Nome} (grau {resultado.Grau})");
        }
    }

    private static string LerTextoObrigatorio(string mensagem)
    {
        string? valor;
        do
        {
            Console.Write(mensagem);
            valor = Console.ReadLine();
        }
        while (string.IsNullOrWhiteSpace(valor));

        return valor.Trim();
    }

    private static int LerAno(string mensagem)
    {
        int ano;
        string? entrada;
        do
        {
            Console.Write(mensagem);
            entrada = Console.ReadLine();
        }
        while (!int.TryParse(entrada, out ano) || ano < 1900);

        return ano;
    }

    private static void Pausar()
    {
        Console.WriteLine();
        Console.Write("Pressione ENTER para voltar ao menu...");
        Console.ReadLine();
    }
}
