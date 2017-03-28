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
                songs.Add("spotify:track:0imYRG0WKxUOOcqBu3VX10");
                songs.Add("spotify:track:54b8qPFqYqIndfdxiLApea");
                songs.Add("spotify:track:57hJxdJGm8kZMU0xPGNBAA");
                songs.Add("spotify:track:5VSAonaAPhhGn0G7hMYwWK");
                songs.Add("spotify:track:3fkPMWQ6cBNBLuFcPyMS8s");

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
                var song = musicService.GetSong("spotify:track:3fkPMWQ6cBNBLuFcPyMS8s");
                Debug.WriteLine(song.Name);
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
