using MediaInfoLib;

namespace MediaInfoTest;

[TestClass]
public class MediaInfoTests {

  [TestMethod]
  public void Validate_MediaInfo_Exists() {
    Message("Testing for existence of MediaInfo.exe");
    Assert.IsTrue(MediaInfo.IsAvailable);
    Ok();
  }

  [TestMethod]
  public async Task MediaInfo_Help() {
    Message("MediaInfo help");
    string HelpText = await MediaInfo.GetHelp();
    Dump(HelpText);
    Assert.IsNotNull(HelpText);
    Ok();
  }

  [TestMethod]
  public async Task MediaInfo_Analyse() {
    Message("MediaInfo analyse");
    string Source = "\\\\andromeda.sharenet.priv\\Films\\Action, Aventure\\[Art martiaux]\\Man of tai chi (2013)\\Man of tai chi (2013).mkv";
    string Target = await MediaInfo.GetMediaInfo(Source);
    Dump(Target);
    Assert.IsNotNull(Target);
    Ok();
  }

  [TestMethod]
  public async Task MediaInfo_GetAudioLanguages() {
    Message("MediaInfo analyse");
    string Source = "\\\\andromeda.sharenet.priv\\Films\\Action, Aventure\\[Aventuriers]\\Uncharted (2022)\\Uncharted (2022).mkv";
    int LanguageCount = 0;
    await foreach (AudioTrackInfo AudioTrackInfoItem in MediaInfo.GetAudioLanguages(Source)) {
      LanguageCount++;
      DumpWithMessage("Found audio", AudioTrackInfoItem);
    }
    Assert.IsTrue(LanguageCount > 0);
    Ok();
  }

  [TestMethod]
  public async Task MediaInfo_GetTextLanguages() {
    Message("MediaInfo analyse");
    string Source = "\\\\andromeda.sharenet.priv\\Films\\Action, Aventure\\[Aventuriers]\\Uncharted (2022)\\Uncharted (2022).mkv";
    int LanguageCount = 0;
    await foreach (TextTrackInfo TextTrackInfoItem in MediaInfo.GetTextLanguages(Source)) {
      LanguageCount++;
      DumpWithMessage("Found subtitle", TextTrackInfoItem);
    }
    Assert.IsTrue(LanguageCount > 0);
    Ok();
  }
}