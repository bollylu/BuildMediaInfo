namespace MediaInfoLib;

public class TListCounter {

  private Dictionary<string, int> Accumulator = new();

  public void Add(string key) {
    if (Accumulator.ContainsKey(key)) {
      Accumulator[key]++;
    } else {
      Accumulator.Add(key, 1);
    }
  }

  public void Clear() {
    Accumulator.Clear();
  }
  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    foreach (KeyValuePair<string, int> kvp in Accumulator.OrderBy(x => x.Key)) {
      RetVal.AppendLine($"{kvp.Key} = {kvp.Value}");
    }
    return RetVal.ToString();
  }

}
