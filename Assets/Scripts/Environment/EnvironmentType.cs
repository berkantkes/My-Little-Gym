using UnityEngine;

public enum EnvironmentType
{
    SalesTable,
    Band1,
    BandUpgrade1,
    BandUpgrade2,
    BandUpgrade3,
    ChangingCubicle1,
    ChangingCubicle2,
    ChangingCubicle3,
    ChangingCubicle4,
    ChangingCubicle5,
    ChangingCubicle6,
    RunningMachine1,
    RunningMachine2,
    RunningMachine3,
    RunningMachine4,
    WC1,
    RunningLevelUp,
    BenchPressLevelUp,
    BenchPress1,
    BenchPress2,
    BenchPress3,
    BenchPress4,
}

[System.Serializable]
public class EnvironmentData
{
    public EnvironmentType environmentType;
    public bool isOpen;
    public bool isPaid;
    public int currentPrice;
    public int level;

    public EnvironmentData(EnvironmentType environmentType, bool isOpen, bool isPaid, int currentPrice, int level)
    {
        this.environmentType = environmentType;
        this.isOpen = isOpen;
        this.isPaid = isPaid;
        this.currentPrice = currentPrice;
        this.level = level;
    }
}