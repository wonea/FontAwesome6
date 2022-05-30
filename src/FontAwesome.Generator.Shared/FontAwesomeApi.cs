﻿using FontAwesome.Generator.Shared.Models.GraphQL;


using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace FontAwesome.Generator.Shared.GraphQl
{
    public class FontAwesomeApi
    {
        public async Task<bool> DoesVersionExistAsync(string version)
        {
            var graphQLClient = new GraphQLHttpClient("https://api.fontawesome.com", new NewtonsoftJsonSerializer());
            var request = new GraphQLRequest
            {
                Query = @"query ReleaseQuery($version: String!) { 
                  release (version: $version)
                  {
                    version                    
                  }
                }",
                Variables = new
                {
                    version
                }
            };

            var response = await graphQLClient.SendQueryAsync<ResponseType>(request);
            return response.Data.Release != null;
        }

        //public async Task<Release[]> GetVersionsAsync()
        //{
        //    var graphQLClient = new GraphQLHttpClient("https://api.fontawesome.com", new NewtonsoftJsonSerializer());
        //    var request = new GraphQLRequest
        //    {
        //        Query = @"{ 
        //          releases
        //          {
        //            version,
        //            isLatest
        //          }
        //        }",
        //    };

        //    var response = await graphQLClient.SendQueryAsync<ResponseType>(request);
        //    return response.Data.Release;
        //}

        public async Task<Icon[]> GetFreeIconsAsync(string version)
        {
            var graphQLClient = new GraphQLHttpClient("https://api.fontawesome.com", new NewtonsoftJsonSerializer());
            var request = new GraphQLRequest
            {
                Query = @"query ReleaseQuery($version: String!) { 
                  release (version: $version)
                  {
                            version,
                            icons(license: ""free"") {
                            id,
                            label,
                            styles,
                            unicode,
                            membership {
                            free,
                            pro
                        }
                    }
                  }
                }",
                Variables = new
                {
                    version
                }
            };

            var response = await graphQLClient.SendQueryAsync<ResponseType>(request);
            return response.Data.Release.Icons;
        }

        public async Task<Icon[]> GetAllIconsAsync(string version)
        {
            var graphQLClient = new GraphQLHttpClient("https://api.fontawesome.com", new NewtonsoftJsonSerializer());
            var request = new GraphQLRequest
            {
                Query = @"query ReleaseQuery($version: String!) { 
                  release (version: $version)
                  {
                            version,
                            icons {
                            id,
                            label,
                            styles,
                            unicode,
                            membership {
                            free,
                            pro
                        }
                    }
                  }
                }",
                Variables = new
                {
                    version
                }
            };

            var response = await graphQLClient.SendQueryAsync<ResponseType>(request);
            return response.Data.Release.Icons;
        }
    }
}
