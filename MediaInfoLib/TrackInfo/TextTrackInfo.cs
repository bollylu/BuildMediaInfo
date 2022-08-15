namespace MediaInfoLib;

public record TextTrackInfo : ITrackInfo {

  public ETrackInfo TrackType => ETrackInfo.Text;

  public string Language { get; set; } = "";
  public string Format { get; set; } = "";
  public string Title { get; set; } = "";
  public bool IsDefault { get; set; } = false;
  public bool IsForced { get; set; } = false;


  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{nameof(TrackType)} = {TrackType.ToString()}");
    RetVal.AppendLine($"{nameof(Language)} = {Language.WithQuotes()}");
    RetVal.AppendLine($"{nameof(Format)} = {Format.WithQuotes()}");
    RetVal.AppendLine($"{nameof(Title)} = {Title.WithQuotes()}");
    RetVal.AppendLine($"{nameof(IsDefault)} = {IsDefault}");
    RetVal.AppendLine($"{nameof(IsForced)} = {IsForced}");
    return RetVal.ToString();
  }
}
