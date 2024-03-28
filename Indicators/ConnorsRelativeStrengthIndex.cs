namespace QuantConnect.Indicators;

/// <summary>
/// Represents a CRSI indicator created by Larry Connors
/// </summary>
public class ConnorsRelativeStrengthIndex : Indicator, IIndicatorWarmUpPeriodProvider
{
    private readonly RelativeStrengthIndex _rsi;
    private readonly RateOfChange _roc;
    private readonly StreakRelativeStrengthIndex _streakRsi;
    public ConnorsRelativeStrengthIndex(
        string name, 
        int rsiPeriod,
        int upDownLenght,
        int rocPeriod)
        : base(name)
    {
        _rsi = new RelativeStrengthIndex($"{name}_RSI", rsiPeriod);
        _streakRsi = new StreakRelativeStrengthIndex($"{name}_UpDownRSI", upDownLenght);
        _roc = new RateOfChange($"{name}_ROC", rocPeriod);
        
    }

    public override bool IsReady => _rsi.IsReady && _roc.IsReady && _streakRsi.IsReady;

    protected override decimal ComputeNextValue(IndicatorDataPoint input)
    {
        _rsi.Update(input);
        _streakRsi.Update(input);
        _roc.Update(input);
        
        return (_rsi.Current.Value + _streakRsi.Current.Value + _roc.Current.Value) / 3;
    }

    public int WarmUpPeriod { get; }
}
