namespace ExamCenterFinder.Api.Application
{
    public interface IRepository<T>
    {
        Task<IList<T>> GetAllAsync();
    }
}
