# menext-app
An Android/iOS app for collaborative Spotify DJing
## Development setup
1. Download Xamarin (now Visual Studio for Mac). Windows development might work. I'm not going to help you with it.
2. Clone this repo
3. Preferences --> Project --> Build --> Uncheck "Enable parallel build of projects"
4. Build all. Look in your error messages. Install any needed android sdks. Repeat.
##
1. Notes: You must always built android in debug so it gets signed with the debug key certificate. O/w Spotify API will reject you.
## Troubleshooting
Xamarin is finicky. Try these things. Also try with one of the Xamarin sample projects.
1. Clean the project
2. Update nuget packages
3. Re-clone
4. Try cleaning again
