using BLTools;

using MediaInfoLib;

namespace BuildMediaInfo;

internal class Program {
  static async Task Main(string[] args) {
    SplitArgs Args = new();
    Args.Parse(args);

    string RootFolder = Args.GetValue("folder", ".");
    if (!Directory.Exists(RootFolder)) {
      Usage("Missing folder");
    }

    Console.WriteLine($"Scanning {RootFolder} ...");

    IEnumerable<string> Movies = Directory.EnumerateFiles(RootFolder, "*.mkv", SearchOption.AllDirectories);

    TListCounter Counter = new();

    foreach (string FileItem in Movies) {
      //Console.WriteLine($"Processing {FileItem}...");
      TMediaInfo MediaInfo = new(FileItem);
      await MediaInfo.GetTracks();
      foreach (AudioTrackInfo TrackItem in MediaInfo.GetAudioTracks()) {
        Counter.Add(TrackItem.Language);
      }
    }

    Console.WriteLine(Counter.ToString());

    Environment.Exit(0);
  }

  static void Usage(string message = "") {
    if (!string.IsNullOrWhiteSpace(message)) {
      Console.WriteLine(message);
    }
    Environment.Exit(1);
  }
}
