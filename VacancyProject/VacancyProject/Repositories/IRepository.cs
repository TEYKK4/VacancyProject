namespace VacancyProject.Repositories;

public interface IRepository<T>
{
    Task<IEnumerable<T>> Get();
    Task<bool> Post(T item);
    Task<bool> Update(T item);
    Task<bool> Delete(T item);
}