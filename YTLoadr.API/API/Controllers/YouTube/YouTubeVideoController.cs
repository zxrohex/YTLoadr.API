using Microsoft.AspNetCore.Mvc;

using System.Security.Cryptography;

using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace YTLoadr.API.API.Controllers.YouTube
{
    [Route("api/YouTube/Video/{id}")]
    [Controller]
    public class YouTubeVideoController : Controller
    {
        private YoutubeClient client;

        public YouTubeVideoController(YoutubeClient client)
        {
            this.client = client;
        }

        [HttpGet]
        public async Task<IActionResult> GetVideo([FromRoute] string id)
        {
            return Json(await client.Videos.GetAsync(VideoId.Parse(id)));
        }

        [HttpGet("GetStream")]
        public async Task<IActionResult> GetStream([FromRoute] string id)
        {
            var manifest = await client.Videos.Streams.GetManifestAsync(VideoId.Parse(id));

            var streamInfo = manifest.GetMuxedStreams().GetWithHighestVideoQuality();

            return File(await client.Videos.Streams.GetAsync(streamInfo), $"video/{streamInfo.Container.Name}", $"video_mx_{DateTime.Now:ddMMyyyyHHmmss}.{streamInfo.Container.Name}", true);
        }
    }
}
