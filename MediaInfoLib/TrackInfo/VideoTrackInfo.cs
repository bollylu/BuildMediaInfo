namespace MediaInfoLib;

public record VideoTrackInfo : ITrackInfo {

  public ETrackInfo TrackType => ETrackInfo.Video;

  public string Format { get; set; } = "";
  public string Title { get; set; } = "";
  public bool IsDefault { get; set; } = false;
  public bool IsForced { get; set; } = false;
  public int Width { get; set; } = 0;
  public int Height { get; set; } = 0;
  public float FrameRate { get; set; } = 0f;
  public int BitDepth { get; set; } = 0;
  public string Codec { get; set; } = "";


  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{nameof(TrackType)} = {TrackType.ToString()}");
    RetVal.AppendLine($"{nameof(Format)} = {Format.WithQuotes()}");
    RetVal.AppendLine($"{nameof(Title)} = {Title.WithQuotes()}");
    RetVal.AppendLine($"{nameof(IsDefault)} = {IsDefault}");
    RetVal.AppendLine($"{nameof(IsForced)} = {IsForced}");
    RetVal.AppendLine($"{nameof(Width)} = {Width}");
    RetVal.AppendLine($"{nameof(Height)} = {Height}");
    RetVal.AppendLine($"{nameof(FrameRate)} = {FrameRate}");
    RetVal.AppendLine($"{nameof(BitDepth)} = {BitDepth}");
    RetVal.AppendLine($"{nameof(Codec)} = {Codec}");
    return RetVal.ToString();
  }
}
