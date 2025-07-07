public class BuildingData
{
    public int id;
    public string displayName;
    public string imageName;
    public int raceId;
    public int gold;
    public int blueprint;

    public BuildingData(int id, string displayName, string imageName, int raceId, int gold, int blueprint)
    {
        this.id = id;
        this.displayName = displayName;
        this.imageName = imageName;
        this.raceId = raceId;
        this.gold = gold;
        this.blueprint = blueprint;
    }
}
