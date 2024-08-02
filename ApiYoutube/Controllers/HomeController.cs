using ApiYoutube.Models;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ApiYoutube.Controllers
{
    public class HomeController : Controller
    {

        public async Task<IActionResult> Index()
        {

            var videos = await BuscarVideosCanal();

            return View(videos);
        }


        private async Task<List<VideoDetalhes>> BuscarVideosCanal()
        {

            var youtubeService = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = "AIzaSyB38OpAF6N8PCXE9vqTBg7cH4qaPbtFxz0",
                ApplicationName = "YoutubeApiVideo"
            });

            var searchRequest = youtubeService.Search.List("snippet");
            searchRequest.ChannelId = "UCXpt0pR8Qo5C67Y--xQpJAQ";
            searchRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
            searchRequest.MaxResults = 20;

            var searchResponse = await searchRequest.ExecuteAsync();

            List<VideoDetalhes> videoList = searchResponse.Items.Select(item =>
                                new VideoDetalhes
                                {
                                    Title = item.Snippet.Title,
                                    Description = item.Snippet.Description,
                                    ThumbnailUrl = item.Snippet.Thumbnails.Medium.Url,
                                    Link = $"https://www.youtube.com/watch?v={item.Id.VideoId}",
                                    PublishedAt = item.Snippet.PublishedAt
                                }

            ).OrderByDescending(video => video.PublishedAt)
             .ToList();

            return videoList;

        }

    }
}
