using AnaliseRedesColaboracao.Domain;

namespace AnaliseRedesColaboracao.Graph;

public sealed class RedeCoautoria
{
    private readonly Dictionary<string, Autor> _autores = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, ArestaCoautoria> _arestas = new(StringComparer.Ordinal);
    private readonly Dictionary<string, HashSet<string>> _adjacencias = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<Publicacao> _publicacoes = [];
    private int _proximoAutorId = 1;
    private int _proximaPublicacaoId = 1;

    public IReadOnlyCollection<Autor> Autores => _autores.Values.ToList();
    public IReadOnlyCollection<ArestaCoautoria> Arestas => _arestas.Values.ToList();
    public IReadOnlyCollection<Publicacao> Publicacoes => _publicacoes.ToList();

    public Autor ObterOuCriarAutor(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do autor é obrigatório.", nameof(nome));

        var nomeNormalizado = nome.Trim();
        var existente = _autores.Values.FirstOrDefault(autor =>
            string.Equals(autor.Nome, nomeNormalizado, StringComparison.OrdinalIgnoreCase));

        if (existente is not null)
            return existente;

        string id;
        do
        {
            id = $"AUT{_proximoAutorId:D3}";
            _proximoAutorId++;
        }
        while (_autores.ContainsKey(id));

        var novoAutor = new Autor(id, nomeNormalizado);
        _autores.Add(novoAutor.Id, novoAutor);
        _adjacencias.Add(novoAutor.Id, new HashSet<string>(StringComparer.OrdinalIgnoreCase));
        return novoAutor;
    }

    public void AdicionarPublicacao(string titulo, int ano, string area, IEnumerable<string> autoresIds)
    {
        var id = $"PUB{_proximaPublicacaoId:D3}";
        _proximaPublicacaoId++;

        var publicacao = new Publicacao(id, titulo, ano, area, autoresIds);
        AdicionarPublicacao(publicacao);
    }

    public void AdicionarPublicacao(Publicacao publicacao)
    {
        ArgumentNullException.ThrowIfNull(publicacao);

        if (_publicacoes.Any(item => string.Equals(item.Id, publicacao.Id, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Já existe uma publicação com o identificador {publicacao.Id}.");

        foreach (var autorId in publicacao.AutoresIds)
        {
            if (!_autores.ContainsKey(autorId))
                throw new InvalidOperationException($"O autor {autorId} não está cadastrado na rede.");
        }

        _publicacoes.Add(publicacao);

        for (var i = 0; i < publicacao.AutoresIds.Count; i++)
        {
            for (var j = i + 1; j < publicacao.AutoresIds.Count; j++)
            {
                var autorA = publicacao.AutoresIds[i];
                var autorB = publicacao.AutoresIds[j];
                var chave = CriarChaveAresta(autorA, autorB);

                if (!_arestas.TryGetValue(chave, out var aresta))
                {
                    var idsOrdenados = OrdenarIds(autorA, autorB);
                    aresta = new ArestaCoautoria(idsOrdenados.Primeiro, idsOrdenados.Segundo);
                    _arestas.Add(chave, aresta);
                    _adjacencias[autorA].Add(autorB);
                    _adjacencias[autorB].Add(autorA);
                }

                aresta.RegistrarPublicacao(publicacao.Ano, publicacao.Area);
            }
        }
    }

    public Autor? ObterAutor(string autorId)
    {
        return _autores.GetValueOrDefault(autorId);
    }

    public IReadOnlyList<(Autor Vizinho, ArestaCoautoria Aresta)> ObterVizinhos(string autorId)
    {
        if (!_adjacencias.TryGetValue(autorId, out var idsVizinhos))
            return [];

        return idsVizinhos
            .Select(idVizinho =>
            {
                var chave = CriarChaveAresta(autorId, idVizinho);
                return (Vizinho: _autores[idVizinho], Aresta: _arestas[chave]);
            })
            .ToList();
    }

    public RedeCoautoria CriarVisaoTemporal(int? anoInicial, int? anoFinal)
    {
        if (anoInicial.HasValue && anoFinal.HasValue && anoInicial.Value > anoFinal.Value)
            throw new ArgumentException("O ano inicial não pode ser maior que o ano final.");

        var visao = new RedeCoautoria();
        var publicacoesFiltradas = _publicacoes.Where(publicacao =>
            (!anoInicial.HasValue || publicacao.Ano >= anoInicial.Value)
            && (!anoFinal.HasValue || publicacao.Ano <= anoFinal.Value));

        foreach (var publicacao in publicacoesFiltradas)
        {
            foreach (var autorId in publicacao.AutoresIds)
            {
                if (!_autores.TryGetValue(autorId, out var autor))
                    continue;

                visao.AdicionarAutorComId(autor);
            }

            visao.AdicionarPublicacao(publicacao);
        }

        return visao;
    }

    private void AdicionarAutorComId(Autor autor)
    {
        if (_autores.ContainsKey(autor.Id))
            return;

        _autores.Add(autor.Id, autor);
        _adjacencias.Add(autor.Id, new HashSet<string>(StringComparer.OrdinalIgnoreCase));
    }

    private static string CriarChaveAresta(string autorA, string autorB)
    {
        var (primeiro, segundo) = OrdenarIds(autorA, autorB);
        return $"{primeiro}\u001F{segundo}";
    }

    private static (string Primeiro, string Segundo) OrdenarIds(string autorA, string autorB)
    {
        return string.Compare(autorA, autorB, StringComparison.OrdinalIgnoreCase) <= 0
            ? (autorA, autorB)
            : (autorB, autorA);
    }
}
