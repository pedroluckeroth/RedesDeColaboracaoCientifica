using AnaliseRedesColaboracao.ConsoleApp;
using AnaliseRedesColaboracao.Data;

var rede = DadosExemplo.CriarRede();
var menu = new MenuConsole(rede);
menu.Executar();
