# Analise de Redes de Colaboracao Cientifica

Projeto academico da disciplina **Algoritmos em Grafos** (C# / .NET 10), referente ao Tema 08.

## Objetivo do prototipo

Representar uma rede de coautoria academica e executar analises iniciais sobre seus autores, publicacoes, comunidades, areas de conhecimento e comportamento ao longo do tempo.

A modelagem adotada e uma projecao da rede de publicacoes para uma rede de autores:

- **Vertice:** um autor.
- **Aresta nao direcionada:** dois autores publicaram pelo menos um trabalho juntos.
- **Peso da aresta:** quantidade de publicacoes compartilhadas.
- **Metadados:** ano e area de conhecimento de cada publicacao compartilhada.

O cadastro de uma publicacao com `k` autores cria ou atualiza as relacoes entre todos os pares de coautores daquela publicacao.

## Funcionalidades ja presentes

1. Resumo da rede: autores, publicacoes, arestas e componentes conexas.
2. Cadastro manual de publicacoes e autores.
3. Ranking por centralidade de grau, com grau ponderado complementar.
4. Identificacao exploratoria de comunidades por propagacao de rotulos (*Label Propagation*), implementada no proprio projeto.
5. Relacao entre areas de conhecimento a partir de autores que publicaram em mais de uma area.
6. Evolucao anual da quantidade de autores ativos, publicacoes e relacoes de coautoria.
7. Recorte temporal para comparar a rede em um intervalo de anos.

## Como executar

Com o SDK do .NET 10 instalado:

```bash
dotnet restore
dotnet run
```

Para compilar em modo Release:

```bash
dotnet build --configuration Release
```

## Dados de demonstracao

O prototipo inicia com oito publicacoes e um pequeno conjunto de autores **ficticios**, criado apenas para demonstrar as funcionalidades. Esses dados nao devem ser apresentados no relatorio como producao cientifica real.

O proximo passo de dados e substituir ou complementar a amostra por uma base real, documentando fonte, criterios de selecao, campos utilizados, periodo e eventuais tratamentos.

## Restricoes e cuidados metodologicos

- As estruturas de grafo e os algoritmos estao implementados no projeto, usando colecoes basicas do .NET. Nenhuma biblioteca pronta de processamento de grafos e utilizada.
- A propagacao de rotulos e uma heuristica de deteccao de comunidades. Os resultados podem variar conforme a estrutura da rede e nao devem ser tratados como uma particao definitiva ou prova de comunidades sociais reais.
- O ranking utiliza centralidade de grau. Outras metricas (por exemplo, intermedialidade e proximidade) podem ser avaliadas como extensao.
- A identificacao de autores pelo nome e provisoria. Em uma base real, deve-se priorizar um identificador persistente (por exemplo, ORCID ou identificador da propria fonte) para evitar confundir homonimos ou duplicar autores.
- O recorte temporal reconstrói a rede a partir das publicacoes dentro do intervalo informado. Assim, o grau e as demais metricas sao recalculados para aquele periodo.
- A interface deste primeiro deploy e de console. O enunciado solicita apresentar uma interface grafica com os resultados no trabalho desenvolvido; portanto, tratem este menu como prototipo funcional inicial e planejem uma interface visual para a versao final, conforme alinhamento com o professor.

## Estrutura do projeto

```text
AnaliseRedesColaboracao/
|-- Algorithms/
|   `-- AnaliseRede.cs
|-- Console/
|   `-- MenuConsole.cs
|-- Data/
|   `-- DadosExemplo.cs
|-- Domain/
|   |-- Autor.cs
|   `-- Publicacao.cs
|-- Graph/
|   |-- ArestaCoautoria.cs
|   `-- RedeCoautoria.cs
|-- docs/
|   |-- checklist-entregas.md
|   `-- modelagem.md
|-- .github/workflows/dotnet.yml
|-- AnaliseRedesColaboracao.csproj
|-- Program.cs
`-- README.md
```

## Publicacao no GitHub

Depois de criar um repositorio vazio no GitHub, execute na pasta do projeto:

```bash
git init
git add .
git commit -m "chore: cria prototipo inicial da rede de coautoria"
git branch -M main
git remote add origin https://github.com/SEU-USUARIO/SEU-REPOSITORIO.git
git push -u origin main
```

Substitua a URL de exemplo pela URL real do repositorio. O workflow em `.github/workflows/dotnet.yml` executa restore e build com .NET 10 a cada push ou pull request para `main`.

## Proximas evolucoes sugeridas

- Importacao de CSV e validacao de identificadores de autores.
- Escolha e documentacao de uma base real de publicacoes.
- Testes automatizados para insercao de publicacoes, pesos, centralidade, comunidades e filtros temporais.
- Comparacao de comunidades com outra abordagem e discussao das limitacoes.
- Metricas adicionais de centralidade e analises por area/instituicao.
- Interface grafica para apresentar a rede e os resultados.
- Avaliacao de desempenho com conjuntos de dados de tamanhos distintos.
