using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace MeNext
{
    public class TestingScreen : ContentPage
    {
        private static Random rnd = new Random();

        public TestingScreen(MainController mc)
        {
            var musicService = mc.musicService;

            this.Title = "Test";
            NavigationPage.SetHasNavigationBar(this, false);
            var layout = new StackLayout()
            {
                Padding = LayoutConsts.DEFAULT_PADDING,
                Children = {
                    new Label { Text = "Im a Home Screen" }
                }
            };


            var playCommand = new Command(() =>
            {
                musicService.Playing = !musicService.Playing;
            });
            layout.Children.Add(new Button
            {
                Text = "Play/Pause",
                Command = playCommand
            });

            var playSomethingElse = new Command(() =>
            {
                var songs = new List<string>();
                // Long Songs
                //songs.Add("spotify:track:0imYRG0WKxUOOcqBu3VX10");
                //songs.Add("spotify:track:54b8qPFqYqIndfdxiLApea");
                //songs.Add("spotify:track:57hJxdJGm8kZMU0xPGNBAA");
                //songs.Add("spotify:track:5VSAonaAPhhGn0G7hMYwWK");
                //songs.Add("spotify:track:3fkPMWQ6cBNBLuFcPyMS8s");
                //songs.Add("spotify:track:3AI7ca5RpYLyqaVgU7K1AP");
                //songs.Add("spotify:track:22M5LjnTiBLsMoz5OLCOEB");
                //songs.Add("spotify:track:6t1FIJlZWTQfIZhsGjaulM");
                //songs.Add("spotify:track:1DndHckdH9m5rp6gYP086b");
                //songs.Add("spotify:track:2EKI5LB3e3zuK1BStWvOt6");
                //songs.Add("spotify:track:7CPJQ5HQIN2ziarFhRqLrz");
                //songs.Add("spotify:track:3gOsZGaMej7EMVy6VBjxHM");
                //songs.Add("spotify:track:1D1nixOVWOxvNfWi0UD7VX");
                //songs.Add("spotify:track:1v2zyAJrChw5JnfafSkwkJ");


                // Short songs
                songs.Add("spotify:track:5cImdQZJ1jppfGCFa3P1Xm");
                songs.Add("spotify:track:2Fw9d40vTv6jqgYubo0UGC");
                songs.Add("spotify:track:6tusfajR8czPAqYGjxCPqA");
                songs.Add("spotify:track:18hivPvBBimfyWTP5D5OJu");

                int r = rnd.Next(songs.Count);

                musicService.PlaySong(musicService.GetSong(songs[r]));
            });
            layout.Children.Add(new Button
            {
                Text = "Play Something Random",
                Command = playSomethingElse
            });

            var testCommand = new Command(() =>
            {
                //var song = musicService.GetSong("spotify:track:3fkPMWQ6cBNBLuFcPyMS8s");
                //Debug.WriteLine(song.Artists[0].Name);

                Debug.WriteLine("Searching");
                var playlists = musicService.UserLibraryPlaylists;
                foreach (var p in playlists.Items) {
                    Debug.WriteLine("\n== " + p.Name + " ==");
                    foreach (var s in p.Songs.Items) {
                        Debug.WriteLine(s.Name);
                    }
                }



                //var results = musicService.UserLibrarySongs;
                //if (results != null) {
                //    Debug.WriteLine("results was not null");
                //    if (results.Items != null) {
                //        Debug.WriteLine("items was not null. printing them.");
                //        foreach (var result in results.Items) {
                //            Debug.WriteLine(result.Name);
                //        }

                //        if (results.HasNextPage) {
                //            Debug.WriteLine("Has next page. Printing it.");
                //            foreach (var result in results.NextPage.Items) {
                //                Debug.WriteLine(result.Name);
                //            }
                //        } else {
                //            Debug.WriteLine("Does not have next page.");
                //        }
                //    } else {
                //        Debug.WriteLine("items was null");
                //    }

                //} else {
                //    Debug.WriteLine("results was null");
                //}

                Debug.WriteLine("Done");
            });
            layout.Children.Add(new Button
            {
                Text = "Test Button",
                Command = testCommand
            });

            Content = layout;
        }
    }
}
