using System.Threading.Tasks;
using Nest;

namespace MLS.Elastic
{
    public class ElasticService : IElasticService
    { 
        private readonly ElasticClient _elasticClient;

        public ElasticService(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public virtual async Task<TDocument> GetDocument<TDocument>(string id) where TDocument : class
        {
            var response = await _elasticClient.GetAsync<TDocument>(id);
            return response.Source;
        }

        public virtual async Task<DeleteResponse> DeleteAsync<TDocument>(string id) where TDocument : class
        {
            var response = await _elasticClient.DeleteAsync<TDocument>(id);
            return response;
        }

        public async Task<IndexResponse> Index<TDocument>(TDocument document) where TDocument : class
        {
            return await _elasticClient.IndexAsync(document, i => i.Index("loans"));
        }

        public virtual async Task<IndexResponse> IndexDocument<TDocument>(TDocument document) where TDocument : class
        {
            return await _elasticClient.IndexDocumentAsync(document);
        }
    }
}
