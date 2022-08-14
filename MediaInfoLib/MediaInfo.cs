using System.Text;
using System.Text.Json;

using BLTools;
using BLTools.Json;
using BLTools.Processes;

namespace MediaInfoLib;
public static class MediaInfo {

  public static string AppLocation = @"c:\Portable applications\MediaInfo\MediaInfo.exe";

  public const string JSON_PROPERTY_MEDIA = "Media";
  public const string JSON_PROPERTY_MEDIA_TRACK = "Track";
  public const string JSON_PROPERTY_MEDIA_TRACK_TYPE = "@type";
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

  public static bool IsAvailable => File.Exists(AppLocation);

  public static async Task<string> GetMediaInfo(string target) {

    if (!File.Exists(target)) {
      throw new FileNotFoundException($"Missing target file {target.WithQuotes()}");
    }

    StringBuilder RetVal = new();

    await foreach (string OutputItem in ProcessHelper.ExecuteProcessAsync(AppLocation, $"--output=JSON {target.WithQuotes()}", Path.GetDirectoryName(AppLocation) ?? "").ConfigureAwait(false)) {
      RetVal.AppendLine(OutputItem);
    }

    return RetVal.ToString();
  }

  public static async Task<string> GetHelp() {

    StringBuilder RetVal = new();
    await foreach (string TextLine in ProcessHelper.ExecuteProcessAsync(AppLocation, "--help")) {
      RetVal.AppendLine(TextLine);
    }

    return RetVal.ToString();
  }

  public static async IAsyncEnumerable<AudioTrackInfo> GetAudioLanguages(string target) {

    string JsonData = await GetMediaInfo(target);

    JsonDocument Doc = JsonDocument.Parse(JsonData);

    foreach (JsonElement TrackItem in Doc.RootElement.SafeGetProperty(JSON_PROPERTY_MEDIA).SafeGetProperty(JSON_PROPERTY_MEDIA_TRACK).EnumerateArray()) {
      string TrackTypeValue = TrackItem.GetProperty(JSON_PROPERTY_MEDIA_TRACK_TYPE).GetString() ?? "";
      if (TrackTypeValue == JSON_PROPERTY_TRACK_AUDIO) {
        AudioTrackInfo RetVal = new() {
          Language = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_AUDIO_LANGUAGE, ""),
          Format = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_AUDIO_FORMAT, ""),
          FormatDescription = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_AUDIO_FORMAT_DESCRIPTION, ""),
          Title = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_AUDIO_TITLE, ""),
          IsDefault = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_AUDIO_IS_DEFAULT, "No").ToBool(),
          IsForced = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_AUDIO_IS_FORCED, "No").ToBool()
        };
        yield return RetVal;
      }
    }
  }

  public static async IAsyncEnumerable<TextTrackInfo> GetTextLanguages(string target) {

    string JsonData = await GetMediaInfo(target);

    JsonDocument Doc = JsonDocument.Parse(JsonData);

    foreach (JsonElement TrackItem in Doc.RootElement.SafeGetProperty(JSON_PROPERTY_MEDIA).SafeGetProperty(JSON_PROPERTY_MEDIA_TRACK).EnumerateArray()) {
      string TrackTypeValue = TrackItem.GetProperty(JSON_PROPERTY_MEDIA_TRACK_TYPE).GetString() ?? "";
      if (TrackTypeValue == JSON_PROPERTY_TRACK_TEXT) {
        TextTrackInfo RetVal = new() {
          Language = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_TEXT_LANGUAGE, ""),
          Format = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_TEXT_FORMAT, ""),
          Title = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_TEXT_TITLE, ""),
          IsDefault = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_TEXT_IS_DEFAULT, "No").ToBool(),
          IsForced = TrackItem.SafeGetPropertyValue(JSON_PROPERTY_TEXT_IS_FORCED, "No").ToBool()
        };
        yield return RetVal;
      }
    }
  }
}
