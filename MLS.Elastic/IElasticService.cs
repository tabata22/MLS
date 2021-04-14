using System;
using System.Threading.Tasks;
using Nest;

namespace MLS.Elastic
{
    public interface IElasticService
    {
        Task<IndexResponse> Index<TDocument>(TDocument document, Func<IndexDescriptor<TDocument>, IIndexRequest<TDocument>> request) 
            where TDocument : class;
        Task<IndexResponse> IndexDocument<TDocument>(TDocument document) where TDocument : class;
        Task<TDocument> GetDocument<TDocument>(string id) where TDocument : class;
        Task<DeleteResponse> DeleteAsync<TDocument>(string id) where TDocument : class;
    }
}
