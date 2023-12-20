using ExamCenterFinder.Api.Domain;

namespace ExamCenterFinder.Api.Application
{
    public interface IZipCodeCenterPointRepository: IRepository<ZipCodeCenterPoint>
    {
        /// <summary>
        /// Get zipcode cener point by zipcode
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        Task<ZipCodeCenterPoint> GetZipCodeCenterPointsByZipCode(string zipCode);
    }
}
