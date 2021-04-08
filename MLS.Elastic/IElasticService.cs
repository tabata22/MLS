using System.Threading.Tasks;
using Nest;

namespace MLS.Elastic
{
    public interface IElasticService<TDocument> where TDocument : class
    {
        Task<IndexResponse> IndexDocument(TDocument document);
        Task<TDocument> GetDocument(string id);
    }
}
