using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ListarMembros
{
    internal class ListMembers
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            try
            {
                //new ListMembers().Search().Wait();
                new ListMembers().GetMembers().Wait();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void TestesInicial()
        {
            ////connect youtube
            //DiscoveryService discoveryService = new DiscoveryService(new BaseClientService.Initializer
            //{
            //    ApplicationName = "ListarmembrosAeM",
            //    ApiKey = "AIzaSyDE89FaDNcOr282GagA337lTsOT42T8akM"
            //});

            //DirectoryList result = await discoveryService.Apis.List().ExecuteAsync();

            //if (result.Items != null)
            //{
            //    foreach (DirectoryList.ItemsData api in result.Items)
            //    {
            //        Console.WriteLine(api.Id + "" + api.Title);
            //    }
            //}
            //YouTubeService youTubeService = new YouTubeService(new BaseClientService.Initializer
            //{
            //    ApplicationName = this.GetType().ToString(),
            //    ApiKey = "AIzaSyDE89FaDNcOr282GagA337lTsOT42T8akM"
            //});


            //var membros = youTubeService.Members.List(null);

            //IEnumerable<string> m_oEnum = new string[] { };

            //MembersResource members = youTubeService.Members;

            //Repeatable<string> part = new string[] { };
            //MembershipsLevelsResource.ListRequest memberships = youTubeService.MembershipsLevels.List(part);

            //MembershipsLevelsResource.ListRequest memberships = youTubeService.MembershipsLevels.List("all");
            //var responseMembers = memberships.Execute();

            //foreach (var item in responseMembers.Items)
            //{
            //    Console.WriteLine(item.Kind);
            //    Console.WriteLine(item.ETag);
            //    Console.WriteLine(item.Id);

            //    Console.WriteLine(item.Snippet.LevelDetails.DisplayName);
            //    Console.WriteLine();
            //}

            //Google.Apis.Requests.ClientServiceRequest clientServiceRequest = new ClientServiceRequest();

            //RequestBuilder requestBuilder = new RequestBuilder().


            //members.List(part);

            //MembersResource.ListRequest.

            //MembersResource.ListRequest.ModeEnum.AllCurrent;


            //list members

            //save to csv
        }

        private void ConnectGoogle()
        {
            //
        }

        private async Task GetMembers()
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows an application to upload files to the
                    // authenticated user's YouTube channel, but doesn't allow other types of access.
                    new[] { YouTubeService.Scope.Youtube },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                //ApiKey = "AIzaSyDE89FaDNcOr282GagA337lTsOT42T8akM", //"REPLACE_ME",
                ApplicationName = this.GetType().ToString(),
                HttpClientInitializer = credential
            });

            var searchListRequest = youtubeService.Members.List("snippet");

            //var searchListRequest = youtubeService.Sponsors.List("snippet");

            var searchListResponse = await searchListRequest.ExecuteAsync();

            foreach (var item in searchListResponse.Items)
            {


                //string diplayName = item.Snippet.MemberDetails.DisplayName;
                //Console.WriteLine(diplayName);
            }



            // 
            // https://www.googleapis.com/youtube/v3/members
            // https://www.googleapis.com/auth/youtube.channel-memberships.creator
        }

        private async Task Search()
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyDE89FaDNcOr282GagA337lTsOT42T8akM", //"REPLACE_ME",
                ApplicationName = this.GetType().ToString()
            }); ;

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = "M4Law"; // Replace with your search term.
            searchListRequest.MaxResults = 50;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            List<string> videos = new List<string>();
            List<string> channels = new List<string>();
            List<string> playlists = new List<string>();

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.VideoId));
                        break;

                    case "youtube#channel":
                        channels.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.ChannelId));
                        break;

                    case "youtube#playlist":
                        playlists.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.PlaylistId));
                        break;
                }
            }

            Console.WriteLine(String.Format("Videos:\n{0}\n", string.Join("\n", videos)));
            Console.WriteLine(String.Format("Channels:\n{0}\n", string.Join("\n", channels)));
            Console.WriteLine(String.Format("Playlists:\n{0}\n", string.Join("\n", playlists)));
        }
    }
}