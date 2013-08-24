using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Microsoft.Phone.BackgroundAudio;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Shell;
using SongzaClasses;

namespace Songza_WP8
{
    public static class API
    {
        static string base_url = "http://songza.com/api/1/";
        static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        public static async Task<List<Scenario>> ConciergeCategories(int day, int period)
        {
            var request = new RestRequest("situation/targeted");
            string current_date = DateTime.Now.ToUniversalTime().ToString("o");

            request.AddParameter("current_date", current_date);
            request.AddParameter("day", day);
            request.AddParameter("period", period);
            request.AddParameter("device", "web");
            request.AddParameter("site", "songza");
            request.AddParameter("optimizer", "default");
            request.AddParameter("max_situations", 5);
            request.AddParameter("max_stations", 3);

            return await Execute<List<Scenario>>(request);
        }

        public static void SetCurrentStation(Station s)
        {
            if (settings.Contains("station"))
                settings["station"] = s;
            else
                settings.Add("station", s);
        }

        public static async Task<List<Station>> SimilarStations(Station s)
        {
            var request = new RestRequest(string.Format("station/{0}/similar",s.Id));

            return await Execute<List<Station>>(request);
        }

        public static async Task<List<Favorite>> Favorites()
        {
            var request = new RestRequest(string.Format("collection/user/{0}", settings["userid"]));

            return await Execute<List<Favorite>>(request);
        }

        public static async void AddToFavorite(string station, string favorite)
        {
            var request = new RestRequest(string.Format("collection/{0}/add-station", favorite));
            request.AddParameter("station", station);
            await Execute<object>(request);
        }

        public static void DayAndPeriod(out int day, out int period)
        {
            day = (int)DateTime.Now.DayOfWeek;

            if (DateTime.Now.Hour < 4)
            {
                period = 5;
                day--;
                if (day < 0)
                    day = 6;
            }
            else if (DateTime.Now.Hour < 10)
                period = 0;
            else if (DateTime.Now.Hour < 12)
                period = 1;
            else if (DateTime.Now.Hour < 18)
                period = 2;
            else if (DateTime.Now.Hour < 21)
                period = 3;
            else if (DateTime.Now.Hour < 23)
                period = 4;
            else
                period = 5;
        }

        private static async void NotifyTrackPlay(string station, string track)
        { 
            var request = new RestRequest(string.Format("station/{0}/song/{1}/notify-play",station,track));
            request.Method = Method.POST;
            await Execute<object>(request);
        }

        public static async Task<List<Station>> ListStations(string[] stationids)
        {
            var request = new RestRequest("station/multi");

            foreach (var item in stationids)
            {
                request.AddParameter("id", item);
            }

            return await Execute<List<Station>>(request);
        }

        public static async Task<List<Station>> Recent()
        {
            var request = new RestRequest(string.Format("user/{0}/stations?limit=40&recent=1",settings["userid"]));
            request.RootElement = "recent";

            var recent = await Execute<Recent>(request);

            return recent.Stations;
        }

        public static async Task<User> Login(string username, string password)
        {
            var request = new RestRequest("login/pw");
            request.AddParameter("username", username);
            request.AddParameter("password", password);
            request.Method = Method.POST;
            request.UserState = "login";

            User resp = await Execute<User>(request);

            if (resp.Id == 0)
                throw new Exception("Login failed");

            SaveUser(username,password,resp.Id.ToString());

            return resp;
        }

        private static void SaveUser(string username, string password, string userid)
        {
            if (settings.Contains("login"))
            {
                settings["login"] = username;
                settings["password"] = password;
                settings["userid"] = userid;
            }
            else
            {
                settings.Add("login", username);
                settings.Add("password", password);
                settings.Add("userid", userid);
            }

            settings.Save();
        }

        public static async Task<Favorite> CreateFavorite(string title)
        {
            var request = new RestRequest(string.Format("collection/user/{0}/create", settings["userid"]));
            request.AddParameter("title", title);
            request.Method = Method.POST;
            return await Execute<Favorite>(request);
        }

        public static async Task<Track> NextTrack(long stationid)
        {
            var request = new RestRequest(string.Format("station/{0}/next", stationid));

            var track = await Execute<Track>(request);

            if(track != null)
                NotifyTrackPlay(stationid.ToString(), track.Song.Id);

            return track;
        }

        public static AudioTrack CreateTrack(Track t, string station)
        {
            return new AudioTrack(new Uri(t.ListenUrl), t.Song.Title, t.Song.Artist.Name, t.Song.Album, new Uri(t.Song.CoverUrl), station, EnabledPlayerControls.Pause | EnabledPlayerControls.SkipNext);
        }

        public async static Task<List<Station>> QueryStations(string queryString)
        {
            var request = new RestRequest(string.Format("search/station?query={0}", HttpUtility.UrlEncode(queryString)));

            return await Execute<List<Station>>(request);
        }

        public async static Task<List<Track.Artist>> QueryArtists(string queryString)
        {
            var request = new RestRequest(string.Format("search/artist?query={0}", HttpUtility.UrlEncode(queryString)));

            return await Execute<List<Track.Artist>>(request);
        }

        public async static Task<List<Station>> StationsForArtist(string artistId)
        {
            var request = new RestRequest(string.Format("artist/{0}/stations", artistId));

            return await Execute<List<Station>>(request);
        }

        public async static Task<List<Station>> PopularStations(string tag)
        {
            var request = new RestRequest(string.Format("chart/name/songza/{0}", tag));

            return await Execute<List<Station>>(request);
        }

        public async static Task<List<Category>> Categories()
        {
            var request = new RestRequest("tags");

            return await Execute<List<Category>>(request);
        }

        public async static Task<List<SubCategory>> SubCategories(string id)
        {
            var request = new RestRequest(string.Format("gallery/tag/{0}", id));

            return await Execute<List<SubCategory>>(request);
        }

        private static async Task<T> Execute<T>(RestRequest request) where T : new()
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            var client = new RestClient(base_url);

            if (settings.Contains("sessionid") && DateTime.Now.Subtract((DateTime)settings["created"]).TotalHours < 24)
                request.AddParameter("sessionid", settings["sessionid"], ParameterType.Cookie);

            client.ExecuteAsync<T>(request, (response) =>
                {
                    if (response == null)
                    {
                        tcs.SetResult(default(T));
                        return;
                    }

                    if (response.ErrorException != null)
                    {
                        if (response.Content == "rate limit exceeded")
                        {
                            tcs.SetResult(default(T));
                            return;
                        }
                        tcs.SetException(response.ErrorException);
                        return;
                    }

                    Parameter p = response.Headers.SingleOrDefault(x => x.Name == "Set-Cookie");

                    if(p != null)
                    {
                        string cookies = (string)p.Value;
                        int index = cookies.IndexOf("sessionid=");
                        if (index >= 0)
                        {
                            string temp = cookies.Substring(index + 10);
                            index = temp.IndexOf(';');
                            string sessionid;
                            if (index >= 0)
                                sessionid = temp.Substring(0, index);
                            else
                                sessionid = temp;

                            if ((request.UserState != null && ((string)request.UserState == "login")) || !settings.Contains("sessionid") || DateTime.Now.Subtract((DateTime)settings["created"]).TotalHours >= 24)
                            {
                                if (settings.Contains("sessionid"))
                                {
                                    settings["sessionid"] = sessionid;
                                    settings["created"] = DateTime.Now;
                                }
                                else
                                {
                                    settings.Add("sessionid", sessionid);
                                    settings.Add("created", DateTime.Now);
                                }

                                settings.Save();
                            }
                        }
                        
                    }

                    tcs.SetResult(response.Data);
                });

            return await tcs.Task;
        }
    }
}