namespace MediaInfoLib;

public record GeneralTrackInfo : ITrackInfo {

  public ETrackInfo TrackType => ETrackInfo.General;

  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{nameof(TrackType)} = {TrackType.ToString()}");
    return RetVal.ToString();
  }



}
