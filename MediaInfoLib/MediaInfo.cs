using BLTools.Processes;

namespace MediaInfoLib;

public class TMediaInfo {

  public static string AppLocation = @"c:\Portable applications\MediaInfo\MediaInfo.exe";

  public ILogger Logger { get; set; } = new TConsoleLogger();
  public string Source { get; set; } = "";

  #region --- JSON constants --------------------------------------------
  public const string JSON_PROPERTY_MEDIA = "Media";
  public const string JSON_PROPERTY_MEDIA_TRACK = "Track";
  public const string JSON_PROPERTY_MEDIA_TRACK_TYPE = "@type";

  public const string JSON_PROPERTY_TRACK_GENERAL = "General";

  public const string JSON_PROPERTY_TRACK_VIDEO = "Video";
  public const string JSON_PROPERTY_VIDEO_TITLE = "Title";
  public const string JSON_PROPERTY_VIDEO_FORMAT = "Format";
  public const string JSON_PROPERTY_VIDEO_WIDTH = "Width";
  public const string JSON_PROPERTY_VIDEO_HEIGH = "Height";
  public const string JSON_PROPERTY_VIDEO_IS_DEFAULT = "Default";
  public const string JSON_PROPERTY_VIDEO_IS_FORCED = "Forced";
  public const string JSON_PROPERTY_VIDEO_FRAME_RATE = "FrameRate";
  public const string JSON_PROPERTY_VIDEO_BIT_DEPTH = "BitDepth";
  public const string JSON_PROPERTY_VIDEO_CODEC = "Codec";

  public const string JSON_PROPERTY_TRACK_AUDIO = "Audio";
  public const string JSON_PROPERTY_AUDIO_LANGUAGE = "Language";
  public const string JSON_PROPERTY_AUDIO_FORMAT = "Format";
  public const string JSON_PROPERTY_AUDIO_FORMAT_DESCRIPTION = "Format_Commercial_IfAny";
  public const string JSON_PROPERTY_AUDIO_TITLE = "Title";
  public const string JSON_PROPERTY_AUDIO_IS_DEFAULT = "Default";
  public const string JSON_PROPERTY_AUDIO_IS_FORCED = "Forced";

  public const string JSON_PROPERTY_TRACK_TEXT = "Text";
  public const string JSON_PROPERTY_TEXT_LANGUAGE = "Language";
  public const string JSON_PROPERTY_TEXT_FORMAT = "Format";
  public const string JSON_PROPERTY_TEXT_TITLE = "Title";
  public const string JSON_PROPERTY_TEXT_IS_DEFAULT = "Default";
  public const string JSON_PROPERTY_TEXT_IS_FORCED = "Forced";
  #endregion --- JSON constants --------------------------------------------

  public static bool IsAvailable => File.Exists(AppLocation);

  public List<ITrackInfo> Tracks { get; } = new();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaInfo() { }
  public TMediaInfo(string source) {
    Source = source;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public async Task<JsonDocument?> ParseMediaInfo() {

    if (!File.Exists(Source)) {
      throw new FileNotFoundException($"Missing target file {Source.WithQuotes()}");
    }

    string DataFromProcess = await ProcessHelper.ExecuteProcessAsync(AppLocation, $"--output=JSON {Source.WithQuotes()}", Path.GetDirectoryName(AppLocation) ?? "").ConfigureAwait(false);

    try {
      return JsonDocument.Parse(DataFromProcess);
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to get tracks", ex);
      return null;
    }
  }

  public async Task<string> GetHelp() {
    return await ProcessHelper.ExecuteProcessAsync(AppLocation, "--help");
  }

  public async Task GetTracks() {

    Tracks.Clear();

    JsonDocument? JsonData = await ParseMediaInfo().ConfigureAwait(false);
    if (JsonData is null) {
      return;
    }

    foreach (JsonElement TrackItem in JsonData.RootElement.SafeGetProperty(JSON_PROPERTY_MEDIA).SafeGetProperty(JSON_PROPERTY_MEDIA_TRACK).EnumerateArray()) {
      string TrackTypeValue = TrackItem.GetProperty(JSON_PROPERTY_MEDIA_TRACK_TYPE).GetString() ?? "";

      switch (TrackTypeValue) {
        case JSON_PROPERTY_TRACK_AUDIO: {
            AudioTrackInfo RetVal = new() {
              Language = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_AUDIO_LANGUAGE, ""),
              Format = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_AUDIO_FORMAT, ""),
              FormatDescription = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_AUDIO_FORMAT_DESCRIPTION, ""),
              Title = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_AUDIO_TITLE, ""),
              IsDefault = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_AUDIO_IS_DEFAULT, "No").ToBool(),
              IsForced = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_AUDIO_IS_FORCED, "No").ToBool()
            };
            Tracks.Add(RetVal);
            break;
          }

        case JSON_PROPERTY_TRACK_VIDEO: {
            VideoTrackInfo RetVal = new VideoTrackInfo() {
              Format = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_VIDEO_FORMAT, ""),
              Title = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_VIDEO_TITLE, ""),
              IsDefault = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_VIDEO_IS_DEFAULT, "No").ToBool(),
              IsForced = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_VIDEO_IS_FORCED, "No").ToBool(),
              Width = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_VIDEO_WIDTH, 0),
              Height = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_VIDEO_HEIGH, 0),
              FrameRate = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_VIDEO_FRAME_RATE, 0f),
              BitDepth = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_VIDEO_BIT_DEPTH, 0),
              Codec = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_VIDEO_CODEC, ""),
            };
            Tracks.Add(RetVal);
            break;
          }

        case JSON_PROPERTY_TRACK_TEXT: {
            TextTrackInfo RetVal = new() {
              Language = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_TEXT_LANGUAGE, ""),
              Format = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_TEXT_FORMAT, ""),
              Title = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_TEXT_TITLE, ""),
              IsDefault = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_TEXT_IS_DEFAULT, "No").ToBool(),
              IsForced = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_TEXT_IS_FORCED, "No").ToBool()
            };
            Tracks.Add(RetVal);
            break;
          }

        case JSON_PROPERTY_TRACK_GENERAL: {
            GeneralTrackInfo RetVal = new();
            Tracks.Add(RetVal);
            break;
          }

        default: {
            //Logger.LogWarning($"Invalid track type : {TrackTypeValue}");
            break;
          }
      }
    }

  }

  public IEnumerable<GeneralTrackInfo> GetGeneralTracks() => Tracks.OfType<GeneralTrackInfo>();

  public IEnumerable<VideoTrackInfo> GetVideoTracks() => Tracks.OfType<VideoTrackInfo>();

  public IEnumerable<AudioTrackInfo> GetAudioTracks() => Tracks.OfType<AudioTrackInfo>();

  public IEnumerable<TextTrackInfo> GetTextTracks() => Tracks.OfType<TextTrackInfo>();

}
