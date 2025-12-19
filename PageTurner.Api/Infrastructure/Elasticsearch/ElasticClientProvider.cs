using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;

namespace PageTurner.Api.Infrastructure.Elasticsearch;

public class ElasticClientProvider
{
    public ElasticsearchClient Client { get; }

    public ElasticClientProvider(IConfiguration configuration)
    {
        var uri =
            configuration["Elasticsearch:Uri"]
            ?? throw new ArgumentNullException("Elasticsearch:Uri");
        var indexName =
            configuration["Elasticsearch:IndexName"]
            ?? throw new ArgumentNullException("Elasticsearch:IndexName");

        var settings = new ElasticsearchClientSettings(new Uri(uri)).DefaultIndex(indexName);

        Client = new ElasticsearchClient(settings);
    }
}
