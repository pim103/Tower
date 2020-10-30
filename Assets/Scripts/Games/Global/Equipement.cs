namespace Games.Global
{
    public enum EquipmentType
    {
        WEAPON,
        ARMOR
    }

    //Class for equipements
    public abstract class Equipement : Item
    {
        public int cost { get; set; }
        public string equipmentName { get; set; }
        
        public EquipmentType equipmentType;
    }
}