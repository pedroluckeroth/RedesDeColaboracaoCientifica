using AnaliseRedesColaboracao.Domain;
using AnaliseRedesColaboracao.Graph;

namespace AnaliseRedesColaboracao.Algorithms;

public sealed record ResultadoCentralidade(
    Autor Autor,
    int Grau,
    int GrauPonderado,
    double CentralidadeDeGrau);

public sealed record ResultadoConexaoAreas(
    string AreaA,
    string AreaB,
    int AutoresCompartilhados);

public static class AnaliseRede
{
    public static IReadOnlyList<ResultadoCentralidade> ObterRankingCentralidade(RedeCoautoria rede)
    {
        var quantidadeAutores = rede.Autores.Count;

        return rede.Autores
            .Select(autor =>
            {
                var vizinhos = rede.ObterVizinhos(autor.Id);
                var grau = vizinhos.Count;
                var grauPonderado = vizinhos.Sum(item => item.Aresta.Peso);
                var centralidade = quantidadeAutores <= 1
                    ? 0
                    : grau / (double)(quantidadeAutores - 1);

                return new ResultadoCentralidade(autor, grau, grauPonderado, centralidade);
            })
            .OrderByDescending(resultado => resultado.CentralidadeDeGrau)
            .ThenByDescending(resultado => resultado.GrauPonderado)
            .ThenBy(resultado => resultado.Autor.Nome, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public static IReadOnlyList<IReadOnlyList<Autor>> ObterComponentesConexas(RedeCoautoria rede)
    {
        var visitados = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var componentes = new List<IReadOnlyList<Autor>>();

        foreach (var autorInicial in rede.Autores.OrderBy(autor => autor.Id, StringComparer.OrdinalIgnoreCase))
        {
            if (visitados.Contains(autorInicial.Id))
                continue;

            var componente = new List<Autor>();
            var fila = new Queue<string>();
            fila.Enqueue(autorInicial.Id);
            visitados.Add(autorInicial.Id);

            while (fila.Count > 0)
            {
                var autorAtualId = fila.Dequeue();
                var autorAtual = rede.ObterAutor(autorAtualId);

                if (autorAtual is not null)
                    componente.Add(autorAtual);

                foreach (var vizinho in rede.ObterVizinhos(autorAtualId))
                {
                    if (visitados.Add(vizinho.Vizinho.Id))
                        fila.Enqueue(vizinho.Vizinho.Id);
                }
            }

            componentes.Add(componente
                .OrderBy(autor => autor.Nome, StringComparer.OrdinalIgnoreCase)
                .ToList());
        }

        return componentes
            .OrderByDescending(componente => componente.Count)
            .ToList();
    }

    public static IReadOnlyList<IReadOnlyList<Autor>> DetectarComunidadesPorPropagacaoDeRotulos(
        RedeCoautoria rede,
        int maximoIteracoes = 30)
    {
        if (maximoIteracoes < 1)
            throw new ArgumentOutOfRangeException(nameof(maximoIteracoes));

        var autores = rede.Autores
            .OrderByDescending(autor => rede.ObterVizinhos(autor.Id).Count)
            .ThenBy(autor => autor.Id, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var rotulos = autores.ToDictionary(
            autor => autor.Id,
            autor => autor.Id,
            StringComparer.OrdinalIgnoreCase);

        for (var iteracao = 0; iteracao < maximoIteracoes; iteracao++)
        {
            var houveAlteracao = false;

            foreach (var autor in autores)
            {
                var vizinhos = rede.ObterVizinhos(autor.Id);
                if (vizinhos.Count == 0)
                    continue;

                var votos = vizinhos
                    .GroupBy(item => rotulos[item.Vizinho.Id], StringComparer.OrdinalIgnoreCase)
                    .Select(grupo => new
                    {
                        Rotulo = grupo.Key,
                        Peso = grupo.Sum(item => item.Aresta.Peso)
                    })
                    .ToList();

                var maiorPeso = votos.Max(voto => voto.Peso);
                var novoRotulo = votos
                    .Where(voto => voto.Peso == maiorPeso)
                    .OrderBy(voto => voto.Rotulo, StringComparer.OrdinalIgnoreCase)
                    .First()
                    .Rotulo;

                if (!string.Equals(rotulos[autor.Id], novoRotulo, StringComparison.OrdinalIgnoreCase))
                {
                    rotulos[autor.Id] = novoRotulo;
                    houveAlteracao = true;
                }
            }

            if (!houveAlteracao)
                break;
        }

        return autores
            .GroupBy(autor => rotulos[autor.Id], StringComparer.OrdinalIgnoreCase)
            .Select(grupo => (IReadOnlyList<Autor>)grupo
                .OrderBy(autor => autor.Nome, StringComparer.OrdinalIgnoreCase)
                .ToList())
            .OrderByDescending(comunidade => comunidade.Count)
            .ToList();
    }

    public static IReadOnlyList<ResultadoConexaoAreas> ObterConexoesEntreAreas(RedeCoautoria rede)
    {
        var areasPorAutor = rede.Publicacoes
            .SelectMany(publicacao => publicacao.AutoresIds.Select(autorId => new
            {
                AutorId = autorId,
                publicacao.Area
            }))
            .GroupBy(item => item.AutorId, StringComparer.OrdinalIgnoreCase)
            .Select(grupo => grupo
                .Select(item => item.Area)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(area => area, StringComparer.OrdinalIgnoreCase)
                .ToList())
            .ToList();

        var conexoes = new Dictionary<string, ResultadoConexaoAreas>(StringComparer.OrdinalIgnoreCase);

        foreach (var areas in areasPorAutor)
        {
            for (var i = 0; i < areas.Count; i++)
            {
                for (var j = i + 1; j < areas.Count; j++)
                {
                    var areaA = areas[i];
                    var areaB = areas[j];
                    var chave = $"{areaA}\u001F{areaB}";

                    if (conexoes.TryGetValue(chave, out var existente))
                    {
                        conexoes[chave] = existente with
                        {
                            AutoresCompartilhados = existente.AutoresCompartilhados + 1
                        };
                    }
                    else
                    {
                        conexoes[chave] = new ResultadoConexaoAreas(areaA, areaB, 1);
                    }
                }
            }
        }

        return conexoes.Values
            .OrderByDescending(conexao => conexao.AutoresCompartilhados)
            .ThenBy(conexao => conexao.AreaA, StringComparer.OrdinalIgnoreCase)
            .ThenBy(conexao => conexao.AreaB, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}
