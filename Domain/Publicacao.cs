namespace AnaliseRedesColaboracao.Domain;

public sealed class Publicacao
{
    public string Id { get; }
    public string Titulo { get; }
    public int Ano { get; }
    public string Area { get; }
    public IReadOnlyList<string> AutoresIds { get; }

    public Publicacao(
        string id,
        string titulo,
        int ano,
        string area,
        IEnumerable<string> autoresIds)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("O identificador da publicação é obrigatório.", nameof(id));

        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("O título da publicação é obrigatório.", nameof(titulo));

        if (ano < 1900)
            throw new ArgumentOutOfRangeException(nameof(ano), "Informe um ano válido (a partir de 1900).");

        if (string.IsNullOrWhiteSpace(area))
            throw new ArgumentException("A área de conhecimento é obrigatória.", nameof(area));

        var ids = autoresIds
            .Where(idAutor => !string.IsNullOrWhiteSpace(idAutor))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (ids.Count == 0)
            throw new ArgumentException("A publicação precisa ter pelo menos um autor.", nameof(autoresIds));

        Id = id.Trim();
        Titulo = titulo.Trim();
        Ano = ano;
        Area = area.Trim();
        AutoresIds = ids;
    }
}
