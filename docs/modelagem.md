# Etapa 1 - Modelagem e planejamento da solucao

## 1. Tema e problema

**Tema 08 - Analise de Redes de Colaboracao Cientifica.** O projeto pretende analisar redes de coautoria academica para identificar autores centrais, grupos de colaboracao, conexoes entre areas de conhecimento e mudancas estruturais ao longo do tempo.

A pergunta central e: **como a estrutura de coautoria permite identificar pesquisadores com maior participacao na rede, comunidades de colaboracao e pontes entre areas de conhecimento, e como essas caracteristicas mudam com o passar dos anos?**

## 2. Justificativa da modelagem

A unidade principal da analise e o autor. Por isso, a rede projetada e um grafo **nao direcionado e ponderado**:

- cada autor e representado por um vertice;
- existe uma aresta entre dois autores quando eles aparecem juntos em pelo menos uma publicacao;
- o peso da aresta corresponde ao numero de publicacoes compartilhadas;
- cada publicacao preserva ano e area de conhecimento, permitindo reconstruir recortes temporais e examinar conexoes entre areas.

Uma publicacao com `k` autores gera relacoes entre todos os pares distintos de autores, ou seja, ate `k(k-1)/2` arestas (ou atualizacoes de peso). Publicacoes com um unico autor adicionam o vertice, mas nao criam arestas.

A modelagem e uma projecao da relacao autor-publicacao para uma rede autor-autor. Essa escolha facilita centralidade e comunidades, mas perde parte da estrutura bipartida original. O relatorio final deve registrar essa decisao e discutir seus efeitos.

## 3. Estruturas de dados

- `Autor`: identificador e nome.
- `Publicacao`: identificador, titulo, ano, area e lista de identificadores de autores.
- `ArestaCoautoria`: dois identificadores de autores, quantidade de colaboracoes e contagens por ano e area.
- `RedeCoautoria`: dicionario de autores, dicionario de arestas e lista de publicacoes.

As arestas sao armazenadas uma unica vez por par de autores. A chave e normalizada para evitar duplicar a relacao `(A, B)` como `(B, A)`.

## 4. Algoritmos do prototipo

### 4.1 Centralidade de grau

Para cada autor `v`, calcula-se `grau(v)`, a quantidade de vizinhos distintos. A centralidade de grau normalizada e:

`C_D(v) = grau(v) / (|V| - 1)`, para redes com mais de um vertice.

O prototipo tambem exibe o grau ponderado, soma dos pesos das arestas incidentes, como indicador complementar de volume de coautorias.

Complexidade: `O(V + E)` para calcular o ranking, considerando a visita das listas de adjacencia/arestas de todos os vertices.

### 4.2 Componentes conexas

Uma busca em largura (BFS) identifica os conjuntos de vertices alcancaveis entre si. Ela e usada no resumo estrutural da rede.

Complexidade: `O(V + E)`.

### 4.3 Comunidades por propagacao de rotulos

Cada autor inicia com um rotulo proprio. Em iteracoes, o autor atualiza seu rotulo para aquele com maior soma de pesos entre seus vizinhos. Empates sao resolvidos pela ordem lexicografica do identificador, tornando a execucao deterministica para a mesma entrada e ordem de processamento.

Complexidade aproximada: `O(I * (V + E))`, em que `I` e o limite de iteracoes.

A propagacao de rotulos e uma heuristica: pode convergir para agrupamentos diferentes conforme a rede, pode produzir uma comunidade muito grande em certas estruturas e nao substitui uma avaliacao de qualidade (por exemplo, modularidade) ou comparacao com outros metodos.

### 4.4 Conexao entre areas

Para cada autor, identificam-se as areas nas quais possui publicacoes. Cada par de areas associado ao mesmo autor recebe uma contribuicao. O resultado indica quantos autores atuam em cada par de areas.

Se um autor tiver `a` areas distintas, a etapa de combinacao gera `a(a-1)/2` pares.

### 4.5 Evolucao temporal

A rede e reconstruida usando apenas publicacoes dentro de um ano ou intervalo. Em cada recorte, recalculam-se quantidade de autores ativos, publicacoes, arestas e ranking de centralidade.

A versao inicial apresenta contagens anuais e permite analisar um intervalo. Evolucoes futuras podem incluir series historicas de centralidade, comunidades e densidade.

## 5. Fluxo de processamento

1. Obter publicacoes com identificador, titulo, ano, area e autores.
2. Resolver cada autor para um identificador consistente.
3. Criar os vertices correspondentes aos autores.
4. Para cada publicacao, criar ou incrementar as arestas de todos os pares de coautores.
5. Executar os algoritmos sobre a rede completa ou sobre recortes temporais.
6. Apresentar resultados e registrar os parametros utilizados.

## 6. Escopo do primeiro prototipo

O primeiro deploy contem uma aplicacao de console com dados sinteticos e as seguintes operacoes: resumo da rede, cadastro manual de publicacoes, ranking de centralidade de grau, propagacao de rotulos, conexoes entre areas, evolucao anual e filtro por periodo.

Os dados de demonstracao sao ficticios. Antes da avaliacao experimental, o grupo devera escolher uma base real, documentar sua origem e preparar a importacao/limpeza dos dados.

## 7. Limitacoes e riscos

- Nomes iguais podem representar pessoas diferentes e nomes diferentes podem representar a mesma pessoa. A versao real precisa de identificadores persistentes e regras de desambiguacao.
- Uma aresta agregada pode ocultar a ordem, o contexto e o tipo de colaboracao.
- Areas de conhecimento precisam de uma taxonomia consistente; sinonimos e classificacoes diferentes podem distorcer as conexoes.
- A propagacao de rotulos e heuristica e precisa ser avaliada no conjunto de dados escolhido.
- A projecao pode favorecer autores de equipes grandes, pois uma publicacao com muitos autores gera muitos pares.
- Bases bibliograficas podem ter cobertura, metadados e periodos diferentes; essas limitacoes devem ser descritas.

## 8. Diagrama inicial de classes

https://github.com/pedroluckeroth/RedesDeColaboracaoCientifica/blob/main/docs/UMLs/diagrama%20de%20classes%20v01.png

## 9. Plano de desenvolvimento

1. **Modelagem:** confirmar pergunta de pesquisa, identificadores, estrutura de grafo, algoritmos e diagrama UML.
2. **Prototipo:** validar a implementacao com dados sinteticos e publicar o primeiro deploy no GitHub.
3. **Dados reais:** selecionar fonte, importar, limpar e documentar o conjunto de dados.
4. **Algoritmos:** validar resultados, adicionar metricas e comparar abordagens de comunidades.
5. **Interface:** construir visualizacoes para a rede, rankings, comunidades, areas e linha do tempo.
6. **Experimentos:** analisar casos, tamanhos, complexidade, desempenho e limitacoes.
7. **Relatorio e apresentacao:** redigir artigo SBC, preparar slides, roteiro do video e demonstracao presencial.

## 10. Links uteis

1. projeto no overleaf: https://www.overleaf.com/project/6b0893063685b6d8988e4eb1
2. repositorio no github: https://github.com/pedroluckeroth/RedesDeColaboracaoCientifica
