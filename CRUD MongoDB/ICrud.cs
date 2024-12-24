using System;

namespace CRUD_MongoDB
{
    public interface ICrud<T>
    {
       void Create(T pessoa);
       List<T> ReadFiltro(string tipo, string propiedade, int i);
       void Update((string categoria, string dado) toUpdate, int i);
       void Delete(int id);
    }
}
