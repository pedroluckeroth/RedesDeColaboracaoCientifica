namespace AnaliseRedesColaboracao.Graph;

public sealed class ArestaCoautoria
{
    private readonly Dictionary<int, int> _publicacoesPorAno = [];
    private readonly Dictionary<string, int> _publicacoesPorArea = new(StringComparer.OrdinalIgnoreCase);

    public string AutorAId { get; }
    public string AutorBId { get; }
    public int Peso => _publicacoesPorAno.Values.Sum();
    public IReadOnlyDictionary<int, int> PublicacoesPorAno => _publicacoesPorAno;
    public IReadOnlyDictionary<string, int> PublicacoesPorArea => _publicacoesPorArea;

    public ArestaCoautoria(string autorAId, string autorBId)
    {
        AutorAId = autorAId;
        AutorBId = autorBId;
    }

    public void RegistrarPublicacao(int ano, string area)
    {
        _publicacoesPorAno[ano] = _publicacoesPorAno.GetValueOrDefault(ano) + 1;
        _publicacoesPorArea[area] = _publicacoesPorArea.GetValueOrDefault(area) + 1;
    }

    public bool Conecta(string autorId)
    {
        return string.Equals(AutorAId, autorId, StringComparison.OrdinalIgnoreCase)
            || string.Equals(AutorBId, autorId, StringComparison.OrdinalIgnoreCase);
    }

    public string ObterOutroAutor(string autorId)
    {
        if (string.Equals(AutorAId, autorId, StringComparison.OrdinalIgnoreCase))
            return AutorBId;

        if (string.Equals(AutorBId, autorId, StringComparison.OrdinalIgnoreCase))
            return AutorAId;

        throw new ArgumentException("O autor informado não pertence a esta aresta.", nameof(autorId));
    }

    public int ObterPeso(int? anoInicial = null, int? anoFinal = null)
    {
        return _publicacoesPorAno
            .Where(item => (!anoInicial.HasValue || item.Key >= anoInicial.Value)
                && (!anoFinal.HasValue || item.Key <= anoFinal.Value))
            .Sum(item => item.Value);
    }
}
