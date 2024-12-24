using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections;


namespace CRUD_MongoDB
{
    public class Programa
    {
        public static void Main(string[] args)
        {
            string operacao = "", categoria, addFiltro = "", if_quantidade, result_if = "", opcao = "", OnOff_string = "";
            int i = 0, num_retorno = 0, contagem = 0, verificarIdade = 0;
            bool categoriaCorreta = false, filtroTrue = true, verificador, on_off = true;
            var dado_recebido = "";

            var dados = new List<string>();
            var pessoaCrud = new PessoaCrud();
            var lista_filtro = new List<PessoaCrud>();


            Console.WriteLine("Qual operação deseja realizar: Inserir, Pesquisar, Alterar ou Excluir");
            operacao = Console.ReadLine().ToLower();

            do
            {

                // CRUD: -------> Create 
                if (operacao == "inserir")
                {

                    // Loop verificação de variavel
                    do 
                    {                 
                        Console.WriteLine("Deseja preencher por Categorias ou Na Mesma Linha ? ");
                        Console.WriteLine("Para escolher Categoria escreva: C | Para escolher Em Linha escreva: I");

                        opcao = Console.ReadLine().ToLower();

                        verificador = pessoaCrud.Verificar(opcao, 3);

                    } while (verificador == true);                   


                    if (opcao == "i")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Insira as categorias pela ordem e separando-as por virgula (,).");
                        Console.WriteLine("(Nome, Sexo, Idade, Cor, Profissão, Estado, Cidade, Bairro ou Email)");
                        Console.WriteLine();
                        Console.WriteLine("Caso não tenho o dado ou não saiba, coloque uma virgula antes de um espaço e coloque uma virgula depois");
                        Console.WriteLine("Exemplo: Nome, , Sexo");
                        Console.WriteLine();
                        string dadosInline = Console.ReadLine();

                        var listaDados = pessoaCrud.toList(dadosInline);

                        var pessoa = new PessoaCrud(listaDados[0], listaDados[1], int.Parse(listaDados[2]), listaDados[3],
                        listaDados[4], listaDados[5], listaDados[6], listaDados[7], listaDados[8]);

                        try
                        {
                            Console.WriteLine("Documento inserido com sucesso!!");
                            Console.WriteLine();

                            pessoaCrud.Create(pessoa);
                        }
                        catch
                        {
                            Console.WriteLine("Documento não Inserido! Houve algum erro.");
                        }

                        Console.WriteLine();
                        Console.WriteLine("Deseja realizar outra operação? (S/N)");
                        OnOff_string = Console.ReadLine().ToLower();

                        if (OnOff_string != "s")
                        {
                            on_off = false;
                        }

                    }


                    else if (opcao == "c")
                    {
                        Console.WriteLine("Escreva seu Nome:");
                        dados.Add(Console.ReadLine());

                        Console.WriteLine("Escreva sue Sexo:");
                        dados.Add(Console.ReadLine());

                        Console.WriteLine("Escreva sua Idade:");
                        dados.Add(Console.ReadLine());

                        Console.WriteLine("Escreva sua Cor:");
                        dados.Add(Console.ReadLine());

                        Console.WriteLine("Escreva sua Profissão:");
                        dados.Add(Console.ReadLine());

                        Console.WriteLine("Escreva seu Estado:");
                        dados.Add(Console.ReadLine());

                        Console.WriteLine("Escreva sua Cidade:");
                        dados.Add(Console.ReadLine());

                        Console.WriteLine("Escreva sua Bairro:");
                        dados.Add(Console.ReadLine());

                        Console.WriteLine("Escreva seu Email:");
                        dados.Add(Console.ReadLine());

                        var pessoa = new PessoaCrud(dados[0], dados[1], int.Parse(dados[2]), dados[3], dados[4], dados[5], dados[6],
                        dados[7], dados[8]);

                        Console.WriteLine();

                        try
                        {
                            Console.WriteLine("Documento inserido com sucesso!!");
                            Console.WriteLine();

                            pessoaCrud.Create(pessoa);
                        }
                        catch
                        {
                            Console.WriteLine("Documento não Inserido! Houve algum erro.");
                        }

                        Console.WriteLine();
                        Console.WriteLine("Deseja realizar outra operação? (S/N)");
                        OnOff_string = Console.ReadLine().ToLower();

                        if (OnOff_string != "s")
                        {
                            on_off = false;
                        }
                    }

                }

                // CRUD: -------> Read 
                else if (operacao == "pesquisar")
                {


                    // Loop para adicionar mais um filtro em conjunto na pesquisa.
                    do
                    {
                        // Loop Confirma se recebe a categoria certa.
                        do
                        {
                            Console.WriteLine();
                            Console.WriteLine("Deseja pesquisar por qual Categoria:");
                            Console.WriteLine("(Id, Nome, Sexo, Idade, Cor, Profissão, Estado, Cidade, Bairro ou Email)");
                            categoria = Console.ReadLine().ToLower().FirstChar();

                            verificador = pessoaCrud.Verificar(categoria, 1);

                        } while (verificador = true);

                        // Loop Confirma se recebe a idade certa.
                        do
                        {
                            Console.WriteLine();
                            Console.WriteLine("Qual ou Quem deseja buscar: ");
                            dado_recebido = Console.ReadLine();
                            contagem = dado_recebido.Count();

                            verificador = pessoaCrud.Verificar(dado_recebido, 1);

                        } while (verificador = true);


                        var resultados = pessoaCrud.ReadFiltro(categoria, dado_recebido, i);

                        num_retorno = resultados.Count;
                        Console.WriteLine($"Resultados Encontrados: {num_retorno}");

                        Console.WriteLine();
                        pessoaCrud.Exibir(resultados);

                        Console.WriteLine("Deseja adicionar outro filtro? (S/N)");
                        addFiltro = Console.ReadLine().ToLower();
                        ++i;

                    } while (addFiltro == "s");

                    i = 0;
                    pessoaCrud.CleanList();

                    Console.WriteLine();
                    Console.WriteLine("Deseja realizar outra operação? (S/N)");
                    OnOff_string = Console.ReadLine().ToLower();

                    if (OnOff_string != "s")
                    {
                        on_off = false;
                    }

                }

                // CRUD: -------> Update 
                else if (operacao == "alterar")
                {


                    Console.WriteLine("Deseja Alterar Apenas Um ou Vários Documentos? (Um/Mais)");
                    if_quantidade = Console.ReadLine().ToLower();


                    if (if_quantidade == "um")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Primeiro pesquise o documento que deseja alterar.");
                        Console.WriteLine();
                    }
                    else if (if_quantidade == "mais")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Primeiro pesquise os documentos que deseja alterar");
                    }
                    do
                    {
                        // Loop Confirma se recebe a categoria certa.
                        do
                        {
                            Console.WriteLine();
                            Console.WriteLine("Que categoria de dado deseja pesquisar?");
                            Console.WriteLine("(Id, Nome, Sexo, Idade, Cor, Profissão, Estado, Cidade, Bairro ou Email)");
                            categoria = Console.ReadLine().ToLower().FirstChar();

                            verificador = pessoaCrud.Verificar(categoria, 1);

                        } while (verificador == true);


                        // Loop Confirma se recebe a idade certa.
                        do
                        {
                            Console.WriteLine();
                            Console.WriteLine("Qual ou Quem deseja buscar: ");
                            dado_recebido = Console.ReadLine();
                            contagem = dado_recebido.Count();

                            verificador = pessoaCrud.Verificar(dado_recebido, 2);

                        } while (verificador == true);


                        var resultados = pessoaCrud.ReadFiltro(categoria, dado_recebido, i);

                        num_retorno = resultados.Count;
                        Console.WriteLine($"Resultados Encontrados: {num_retorno}");
                        Console.WriteLine();

                        Console.WriteLine();
                        pessoaCrud.Exibir(resultados);

                        Console.WriteLine("Deseja adicionar outro filtro? (S/N)");
                        addFiltro = Console.ReadLine().ToLower();
                        ++i;

                        if (addFiltro == "n" && (if_quantidade == "um" && num_retorno != 1))
                        {
                            Console.WriteLine();
                            Console.WriteLine("Como foi selecionado apenas 'UM' documento para ser modificado");
                            Console.WriteLine("Os resultados encontrados devem ser apenas um (1).");
                            Console.WriteLine();
                            Console.WriteLine("Adicione outra categoria do documento que deseja encontrar:");

                            filtroTrue = true;
                        }
                        else if (addFiltro == "n" && num_retorno == 1)
                        {
                            filtroTrue = false;
                        }

                        if (addFiltro == "n" && if_quantidade == "mais")
                        {
                            filtroTrue = false;
                        }

                    } while (filtroTrue == true);


                    Console.WriteLine();
                    if (if_quantidade == "um" && num_retorno == 1)
                    {
                        Console.WriteLine("Esse é o documento que deseja alterar? (sim/não)");
                    }
                    else if (if_quantidade == "mais")
                    {
                        Console.WriteLine("Esses são os documentos que deseja alterar? (sim/não)");
                    }

                    result_if = Console.ReadLine().ToLower();

                    if (result_if == "sim")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Deseja alterar que categoria?");
                        Console.WriteLine("(Nome, Sexo, Idade, Cor, Profissão, Estado, Cidade, Bairro ou Email)");
                        string update_categoria = Console.ReadLine().ToLower().FirstChar();

                        Console.WriteLine();
                        Console.WriteLine("Escreva a alteração:");
                        var update_dado = Console.ReadLine();

                        var toUpdate = (update_categoria, update_dado);

                        if (if_quantidade == "um")
                        {
                            i = 0;
                            pessoaCrud.Update(toUpdate, i);
                        }
                        else if (if_quantidade == "mais")
                        {
                            i = -1;
                            pessoaCrud.Update(toUpdate, i);
                        }

                        i = 0;
                        pessoaCrud.CleanList();

                        Console.WriteLine();
                        Console.WriteLine("Deseja realizar outra operação? (S/N)");
                        OnOff_string = Console.ReadLine().ToLower();

                        if (OnOff_string != "s")
                        {
                            on_off = false;
                        }
                    }
                }

                // CRUD: -------> Delete 
                else if (operacao == "excluir")
                {


                    Console.WriteLine("Deseja excluir apenas Um ou Mais documentos? (Um/Mais)");
                    if_quantidade = Console.ReadLine().ToLower();

                    if (if_quantidade == "um")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Primeiro pesquise o documento que deseja excluir.");
                        Console.WriteLine();
                    }
                    else if (if_quantidade == "mais")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Primeiro pesquise os documentos que deseja excluir");
                    }

                    do
                    {

                        // Loop Confirma se recebe a categoria certa.
                        do
                        {
                            Console.WriteLine();
                            Console.WriteLine("Que categoria de dado deseja pesquisar?");
                            Console.WriteLine("(Id, Nome, Sexo, Idade, Cor, Profissão, Estado, Cidade, Bairro ou Email)");
                            categoria = Console.ReadLine().ToLower().FirstChar();

                            verificador = pessoaCrud.Verificar(categoria, 1);

                        } while (verificador == true);


                        // Loop Confirma se recebe a idade certa.
                        do
                        {
                            Console.WriteLine();
                            Console.WriteLine("Qual ou Quem deseja buscar: ");
                            dado_recebido = Console.ReadLine();
                            contagem = dado_recebido.Count();

                            verificador = pessoaCrud.Verificar(dado_recebido, 2);

                        } while (verificador == true);

                        var resultados = pessoaCrud.ReadFiltro(categoria, dado_recebido, i);

                        num_retorno = resultados.Count;
                        Console.WriteLine($"Resultados Encontrados: {num_retorno}");

                        Console.WriteLine();
                        pessoaCrud.Exibir(resultados);

                        Console.WriteLine("Deseja adicionar outro filtro? (S/N)");
                        addFiltro = Console.ReadLine().ToLower();
                        ++i;

                        if (addFiltro == "n" && (if_quantidade == "um" && num_retorno != 1))
                        {
                            Console.WriteLine();
                            Console.WriteLine("Como foi selecionado apenas 'UM' documento para ser modificado");
                            Console.WriteLine("Os resultados encontrados devem ser apenas um (1).");
                            Console.WriteLine();
                            Console.WriteLine("Adicione outra categoria do documento que deseja encontrar:");

                            filtroTrue = true;
                        }
                        else if (addFiltro == "n" && num_retorno == 1)
                        {
                            filtroTrue = false;
                        }

                        if (addFiltro == "n" && if_quantidade == "mais")
                        {
                            filtroTrue = false;
                        }

                    } while (filtroTrue == true);


                    Console.WriteLine();
                    if (if_quantidade == "um" && num_retorno == 1)
                    {
                        Console.WriteLine("Esse é o documento que deseja excluir? (sim/não)");
                    }
                    else if (if_quantidade == "mais")
                    {
                        Console.WriteLine("Esses são os documentos que deseja excluir? (sim/não)");
                    }

                    result_if = Console.ReadLine().ToLower();

                    if (result_if == "sim")
                    {
                        Console.WriteLine("Tem certeza que deseja excluir esse arquivo? (S/N)");
                        string confirmacao = Console.ReadLine().ToLower();

                        if (confirmacao == "s")
                        {
                            if (if_quantidade == "um")
                            {
                                i = 0;
                                pessoaCrud.Delete(i);
                            }
                            else if (if_quantidade == "mais")
                            {
                                i = -1;
                                pessoaCrud.Delete(i);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Operação Cancelada.");
                        }

                        i = 0;
                        pessoaCrud.CleanList();

                        Console.WriteLine();
                        Console.WriteLine("Deseja realizar outra operação? (S/N)");
                        OnOff_string = Console.ReadLine().ToLower();

                        if (OnOff_string != "s")
                        {
                            on_off = false;
                        }
                    }
                }

            } while (on_off = true);
            
        }
    }

}
