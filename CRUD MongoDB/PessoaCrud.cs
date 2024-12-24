using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Bson.Serialization.Attributes;

namespace CRUD_MongoDB
{
    public class PessoaCrud:ICrud<PessoaCrud>
    {
        public int Id { get; private set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Cor { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }

        public PessoaCrud(string Nome = null, int idade = 0, string Cor = null,
        string Estado = null, string Cidade = null)
        {
            this.Nome = Nome;
            this.Idade = idade;
            this.Cor = Cor;
            this.Estado = Estado;
            this.Cidade = Cidade;

            var client = new MongoClient("mongodb://localhost:27017/");
            var database = client.GetDatabase("Formularios");
            collection = database.GetCollection<PessoaCrud>("Pessoas");

            Tipo_Propriedade = new List<(string Categoria, string Dado)>();
        }

        private readonly IMongoCollection<PessoaCrud> collection;

        [BsonIgnore]
        public List<(string Categoria, string Dado)> Tipo_Propriedade;


        [BsonIgnore]
        public FilterDefinition<PessoaCrud> filtro;
        int i = 0;
        int novoId = 0;


        public void CleanList()
        {
            Tipo_Propriedade.Clear();
        }

        public bool Verificar(string input, int input_i)
        {
            string[] CategoriasConfirmar = { "Id", "Nome", "Idade", "Cor", "Estado", "Cidade"};
            int contagem = 0, verificarIdade = 0;

            if (input_i == 1)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine();
                    Console.WriteLine("Categoria inválida! O espaço categoria está em branco.");
                    Console.WriteLine("Escreva uma categoria.");
                    return true;
                }
                else if (!CategoriasConfirmar.Contains(input))
                {
                    Console.WriteLine();
                    Console.WriteLine("Categoria inválida! Escreva uma categoria existente.");
                    return true;
                }

                if (CategoriasConfirmar.Contains(input))
                {
                    return false;
                }
            }

            else if (input_i == 2)
            {
                contagem = input.Count();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine();
                    Console.WriteLine("Dado inválido! O espaço está em branco.");
                    return true;
                }

                if (input == "Idade" && contagem <= 3)
                {
                    try
                    {
                        verificarIdade = int.Parse(input);
                    }
                    catch
                    { }
                }

                if (input == "Idade" && (verificarIdade < 12 || verificarIdade > 105))
                {
                    Console.WriteLine("Idade inválida! Escreva uma idade válida para a pesquisa.");
                    return true;
                }
            }

            else if (input_i == 3)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine();
                    Console.WriteLine("Dado inválido! O espaço está em branco.");
                    return true;
                }
                else if (input != "i" || input != "c")
                {
                    Console.WriteLine("Dado inválido! Insira a letra C ou a I.");
                }
            }

            return false;
        }

        public void Exibir(List<PessoaCrud> lista)
        {
            foreach (var item in lista)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine($"Id: {item.Id}");
                Console.WriteLine($"Nome: {item.Nome}");
                Console.WriteLine($"Idade: {item.Idade}");
                Console.WriteLine($"Cor: {item.Cor}");
                Console.WriteLine($"Estado: {item.Estado}");
                Console.WriteLine($"Cidade: {item.Cidade}");
                Console.WriteLine("-------------------------------------------");
            }
        }

        private void pegarFiltro(FilterDefinition<PessoaCrud> filtroOperacao)
        {
            filtro = filtroOperacao;
        }

        public List<string> toList(string texto)
        {
            List<string> lista = new List<string>(texto.Split(','));

            

            for (int i = 0; i < lista.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(lista[i]))
                {
                    lista[i] = "Vazio";
                }

                lista[i] = lista[i].Trim();
          
            }

            return lista;
        }

        public void Create(PessoaCrud pessoa)
        {
            var novoId = collection.Find(Builders<PessoaCrud>.Filter.Empty)
            .Sort(Builders<PessoaCrud>.Sort.Descending("Id")).Limit(1).FirstOrDefault();

            var nova_Pessoa = new PessoaCrud()
            {
                Id = (novoId?.Id ?? 0) + 1,
                Nome = pessoa.Nome,
                Idade = pessoa.Idade,
                Cor = pessoa.Cor,
                Estado = pessoa.Estado,
                Cidade = pessoa.Cidade
            };

            collection.InsertOne(nova_Pessoa);

            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Pessoa Adiconada:");
            Console.WriteLine($"Id: {nova_Pessoa.Id}");
            Console.WriteLine($"Nome: {nova_Pessoa.Nome}");
            Console.WriteLine($"Idade: {nova_Pessoa.Idade}");
            Console.WriteLine($"Cor: {nova_Pessoa.Cor}");
            Console.WriteLine($"Estado: {nova_Pessoa.Estado}");
            Console.WriteLine($"Cidade: {nova_Pessoa.Cidade}");
            Console.WriteLine("-------------------------------------------");
        }

        public List<PessoaCrud> ReadFiltro(string tipo, string propiedade, int i)
        {

            Tipo_Propriedade.Add((tipo, propiedade));

            if (i == 0)
            {
                var builder = Builders<PessoaCrud>.Filter;
                var filtro1 = builder.Eq(Tipo_Propriedade[0].Categoria, Tipo_Propriedade[0].Dado);
                pegarFiltro(filtro1);

                var resultado = collection.Find(filtro1).ToList();


                return resultado;
            }
            else if (i == 1)
            {

                var builder = Builders<PessoaCrud>.Filter;
                var filtro1 = builder.Eq(Tipo_Propriedade[0].Categoria, Tipo_Propriedade[0].Dado);
                var filtro2 = builder.Eq(Tipo_Propriedade[1].Categoria, Tipo_Propriedade[1].Dado);
                var filtro_combinado = builder.And(filtro1, filtro2);
                pegarFiltro(filtro_combinado);

                var resultado = collection.Find(filtro_combinado).ToList();

                return resultado;
            }
            else if (i == 2)
            {
                var builder = Builders<PessoaCrud>.Filter;
                var filtro1 = builder.Eq(Tipo_Propriedade[0].Categoria, Tipo_Propriedade[0].Dado);
                var filtro2 = builder.Eq(Tipo_Propriedade[1].Categoria, Tipo_Propriedade[1].Dado);
                var filtro3 = builder.Eq(Tipo_Propriedade[2].Categoria, Tipo_Propriedade[2].Dado);
                var filtro_combinado = builder.And(filtro1, filtro2, filtro3);
                pegarFiltro(filtro_combinado);

                var resultado = collection.Find(filtro_combinado).ToList();

                return resultado;
            }
            else if (i == 3)
            {
                var builder = Builders<PessoaCrud>.Filter;
                var filtro1 = builder.Eq(Tipo_Propriedade[0].Categoria, Tipo_Propriedade[0].Dado);
                var filtro2 = builder.Eq(Tipo_Propriedade[1].Categoria, Tipo_Propriedade[1].Dado);
                var filtro3 = builder.Eq(Tipo_Propriedade[2].Categoria, Tipo_Propriedade[2].Dado);
                var filtro4 = builder.Eq(Tipo_Propriedade[3].Categoria, Tipo_Propriedade[3].Dado);
                var filtro_combinado = builder.And(filtro1, filtro2, filtro3, filtro4);
                pegarFiltro(filtro_combinado);

                var resultado = collection.Find(filtro_combinado).ToList();

                return resultado;
            }
            else if (i == 4)
            {
                var builder = Builders<PessoaCrud>.Filter;
                var filtro1 = builder.Eq(Tipo_Propriedade[0].Categoria, Tipo_Propriedade[0].Dado);
                var filtro2 = builder.Eq(Tipo_Propriedade[1].Categoria, Tipo_Propriedade[1].Dado);
                var filtro3 = builder.Eq(Tipo_Propriedade[2].Categoria, Tipo_Propriedade[2].Dado);
                var filtro4 = builder.Eq(Tipo_Propriedade[3].Categoria, Tipo_Propriedade[3].Dado);
                var filtro5 = builder.Eq(Tipo_Propriedade[4].Categoria, Tipo_Propriedade[4].Dado);
                var filtro_combinado = builder.And(filtro1, filtro2, filtro3, filtro4, filtro5);
                pegarFiltro(filtro_combinado);

                var resultado = collection.Find(filtro_combinado).ToList();

                return resultado;
            }
            else if (i == 5)
            {
                var builder = Builders<PessoaCrud>.Filter;
                var filtro1 = builder.Eq(Tipo_Propriedade[0].Categoria, Tipo_Propriedade[0].Dado);
                var filtro2 = builder.Eq(Tipo_Propriedade[1].Categoria, Tipo_Propriedade[1].Dado);
                var filtro3 = builder.Eq(Tipo_Propriedade[2].Categoria, Tipo_Propriedade[2].Dado);
                var filtro4 = builder.Eq(Tipo_Propriedade[3].Categoria, Tipo_Propriedade[3].Dado);
                var filtro5 = builder.Eq(Tipo_Propriedade[4].Categoria, Tipo_Propriedade[4].Dado);
                var filtro6 = builder.Eq(Tipo_Propriedade[5].Categoria, Tipo_Propriedade[5].Dado);
                var filtro_combinado = builder.And(filtro1, filtro2, filtro3, filtro4, filtro5, filtro6);
                pegarFiltro(filtro_combinado);

                var resultado = collection.Find(filtro_combinado).ToList();

                return resultado;
            }

            else
            {
                return null;
            }
        }

        public void Update((string categoria, string dado) toUpdate, int i)
        {
            if (i == 0)
            {
                var update = Builders<PessoaCrud>.Update.Set(toUpdate.categoria, toUpdate.dado);

                collection.UpdateOne(filtro, update);

                var builder = Builders<PessoaCrud>.Filter;
                var resultado = collection.Find(filtro).ToList();

                Console.WriteLine("Documento Atualizado!");

                Exibir(resultado);
            }
            else if (i == -1)
            {
                var update = Builders<PessoaCrud>.Update.Set(toUpdate.categoria, toUpdate.dado);

                collection.UpdateMany(filtro, update);

                var builder = Builders<PessoaCrud>.Filter;
                var resultado = collection.Find(filtro).ToList();

                Console.WriteLine("Documentos Atualizados!");
                Console.WriteLine();

                Exibir(resultado);
            }
        }

        public void Delete(int i)
        {
            if (i == 0)
            {
                collection.DeleteOne(filtro);
            }
            else if (i == -1)
            {
                collection.DeleteMany(filtro);
            }

        }

        
    }
}


