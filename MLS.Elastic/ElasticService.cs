using System.Threading.Tasks;
using Nest;

namespace MLS.Elastic
{
    public class ElasticService<TDocument> : IElasticService<TDocument> where TDocument : class
    {
        private readonly ElasticClient _elasticClient;

        public ElasticService(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public virtual async Task<TDocument> GetDocument(string id)
        {
            var response = await _elasticClient.GetAsync<TDocument>(id);
            return response.Source;
        }

        public virtual async Task<IndexResponse> IndexDocument(TDocument document)
        {
            return await _elasticClient.IndexDocumentAsync(document);
        }
    }
}
