# Checklist geral de entregas - Trabalho Pratico de Algoritmos em Grafos

Organizacao baseada no enunciado do Trabalho Pratico 02/2026. Confirmar no Canvas a data e eventuais orientacoes complementares.

## Antes de desenvolver

- [ ] Definir pergunta de pesquisa e recorte (area, instituicao, pais, periodo ou outro criterio).
- [ ] Escolher fonte/base de publicacoes e verificar licenca, cobertura e campos disponiveis.
- [ ] Definir identificador de autor e estrategia de desambiguacao.
- [ ] Formalizar a modelagem e justificar grafo, vertices, arestas, pesos e metadados.
- [ ] Produzir diagrama UML da solucao.
- [ ] Escrever plano de desenvolvimento e responsabilidades iniciais.

## Etapa 1 - Modelagem e planejamento da solucao

**Entregavel indicado no enunciado:** documento com descricao do problema, justificativa da modelagem e plano de desenvolvimento.

- [ ] Contextualizacao do problema e motivacao.
- [ ] Objetivos e perguntas que a ferramenta pretende responder.
- [ ] Definicao formal dos vertices, arestas, direcao, pesos e atributos.
- [ ] Justificativa da representacao escolhida e suas limitacoes.
- [ ] Definicao dos algoritmos de grafos que serao implementados.
- [ ] Diagrama UML e descricao das principais classes/estruturas.
- [ ] Plano de desenvolvimento, cronograma e divisao de responsabilidades.

## Etapa 2 - Desenvolvimento da ferramenta

**Entregavel indicado no enunciado:** prototipo funcional com codigo versionado no Git.

- [x] Criar projeto em C# / .NET 10.
- [x] Implementar estrutura propria da rede de coautoria.
- [x] Implementar centralidade de grau.
- [x] Implementar componentes conexas.
- [x] Implementar uma primeira heuristica de comunidades.
- [x] Implementar relacoes entre areas e recortes temporais iniciais.
- [x] Criar menu minimo de console para o primeiro deploy.
- [ ] Escolher e integrar base real de publicacoes.
- [ ] Implementar importacao e validacao dos dados.
- [ ] Criar testes automatizados e validar casos de borda.
- [ ] Comparar algoritmos e validar a qualidade dos resultados.
- [ ] Criar interface grafica com visualizacao dos resultados para a versao final.
- [ ] Registrar evolucao no Git com commits individuais e frequentes.
- [ ] Avaliar desempenho com diferentes tamanhos de entrada.

## Etapa 3 - Apresentacao e relatorio final

### Relatorio tecnico

- [ ] Produzir artigo em LaTeX usando obrigatoriamente o template oficial da SBC.
- [ ] Manter o relatorio entre 12 e 15 paginas.
- [ ] Incluir contextualizacao, problema e justificativa.
- [ ] Incluir modelagem, diagrama UML e algoritmos.
- [ ] Explicar implementacao e interface.
- [ ] Apresentar base de dados, origem, limpeza e criterios de selecao.
- [ ] Demonstrar telas, resultados e experimentos.
- [ ] Analisar complexidade e desempenho.
- [ ] Discutir limitacoes e conclusoes.
- [ ] Descrever a responsabilidade e a contribuicao efetiva de cada integrante.
- [ ] Revisar referencias, figuras, tabelas e formatacao SBC.
- [ ] Exportar o PDF final no Overleaf.

### Apresentacao e demonstracao

- [ ] Preparar slides.
- [ ] Preparar demonstracao funcional da ferramenta.
- [ ] Garantir participacao de todos os integrantes.
- [ ] Planejar apresentacao presencial de 20 minutos.
- [ ] Gravar video de 12 a 18 minutos, focado no funcionamento e nas funcionalidades produzidas por cada aluno.

### Repositorio e pacote final

- [ ] Manter historico de commits no Git; commits individuais serao avaliados.
- [ ] Revisar README e instrucoes para executar o projeto.
- [ ] Conferir codigo-fonte completo.
- [ ] Conferir arquivo-fonte do relatorio (`.tex`).
- [ ] Conferir PDF do relatorio.
- [ ] Criar um unico `.zip` contendo codigo-fonte, `.tex` e PDF.
- [ ] Conferir no Canvas a data e fazer a entrega (apenas um integrante precisa enviar).

## Regras do enunciado para nao esquecer

- O grupo previsto no enunciado e de **5 alunos**.
- A implementacao do problema deve usar **C#**.
- Todos os integrantes devem participar do desenvolvimento relacionado a algoritmos de grafos; nao basta ficar somente com relatorio, interface ou modelagem.
- Nao usar bibliotecas prontas para criacao/processamento do grafo (o enunciado cita NetworkX como exemplo proibido, exceto para comparacao de desempenho).
- O trabalho final deve apresentar interface grafica com resultados; o menu de console deste primeiro prototipo nao substitui automaticamente essa exigencia.
- A ausencia de qualquer item exigido pode resultar em nota zero, conforme observacao do enunciado.
