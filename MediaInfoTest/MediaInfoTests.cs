namespace MediaInfoTest;

[TestClass]
public class MediaInfoTests {

  [TestMethod]
  public void Validate_MediaInfo_Exists() {
    Message("Testing for existence of MediaInfo.exe");
    Assert.IsTrue(TMediaInfo.IsAvailable);
    Ok();
  }

  [TestMethod]
  public async Task MediaInfo_Help() {
    Message("MediaInfo help");
    TMediaInfo MediaInfo = new();
    string HelpText = await MediaInfo.GetHelp();
    Dump(HelpText);
    Assert.IsNotNull(HelpText);
    Ok();
  }

  [TestMethod]
  public async Task MediaInfo_Analyse() {
    Message("MediaInfo analyse");
    string Source = DataSource.MoviePathnames.First();
    TMediaInfo MediaInfo = new(Source);
    JsonDocument? Target = await MediaInfo.ParseMediaInfo();
    Assert.IsNotNull(Target);
    Dump(Target.RootElement);
    Assert.IsNotNull(Target);
    Ok();
  }

  [TestMethod]
  public async Task MediaInfo_GetAudioTracks() {
    Message("Getting audio track");
    string Source = DataSource.MoviePathnames.First();
    int LanguageCount = 0;
    TMediaInfo MediaInfo = new(Source);
    await MediaInfo.GetTracks();
    foreach (AudioTrackInfo AudioTrackInfoItem in MediaInfo.GetAudioTracks()) {
      LanguageCount++;
      DumpWithMessage("Found audio", AudioTrackInfoItem);
    }
    Assert.IsTrue(LanguageCount > 0);
    Ok();
  }

  [TestMethod]
  public async Task MediaInfo_GetTextTracks() {
    Message("Getting subtitles");
    string Source = DataSource.MoviePathnames.First();
    int LanguageCount = 0;
    TMediaInfo MediaInfo = new(Source);
    await MediaInfo.GetTracks();
    foreach (TextTrackInfo TextTrackInfoItem in MediaInfo.GetTextTracks()) {
      LanguageCount++;
      DumpWithMessage("Found subtitle", TextTrackInfoItem);
    }
    Assert.IsTrue(LanguageCount > 0);
    Ok();
  }

  [TestMethod]
  public async Task MediaInfo_GetVideoTracks() {
    Message("Getting video tracks");
    string Source = DataSource.MoviePathnames.First();
    int LanguageCount = 0;
    TMediaInfo MediaInfo = new(Source);
    await MediaInfo.GetTracks();
    foreach (VideoTrackInfo VideoTrackInfoItem in MediaInfo.GetVideoTracks()) {
      LanguageCount++;
      DumpWithMessage("Found video track", VideoTrackInfoItem);
    }
    Assert.IsTrue(LanguageCount > 0);
    Ok();
  }

  [TestMethod]
  public async Task MediaInfo_GetAllTracks() {
    Message("Getting all tracks");
    string Source = DataSource.MoviePathnames.First();
    int LanguageCount = 0;
    TMediaInfo MediaInfo = new(Source);
    await MediaInfo.GetTracks();
    foreach (ITrackInfo TrackInfoItem in MediaInfo.Tracks) {
      LanguageCount++;
      DumpWithMessage("Found track", TrackInfoItem);
    }
    Assert.IsTrue(LanguageCount > 0);
    Ok();
  }
}