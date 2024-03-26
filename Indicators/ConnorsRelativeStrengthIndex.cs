namespace QuantConnect.Indicators;

/// <summary>
/// Represents a CRSI indicator created by Larry Connors
/// </summary>
public class ConnorsRelativeStrengthIndex : Indicator, IIndicatorWarmUpPeriodProvider
{
    private readonly RelativeStrengthIndex _rsi;
    private readonly RateOfChange _roc;
    private readonly RelativeStrengthIndex _updownRsi;
    public ConnorsRelativeStrengthIndex(
        string name, 
        int rsiPeriod,
        int upDownLenght,
        int rocPeriod)
        : base(name)
    {
        _rsi = new RelativeStrengthIndex($"{name}_RSI", rsiPeriod);
        _updownRsi = new RelativeStrengthIndex($"{name}_UpDownRSI", upDownLenght);
        _roc = new RateOfChange($"{name}_ROC", rocPeriod);
    }

    public override bool IsReady { get; }
    protected override decimal ComputeNextValue(IndicatorDataPoint input)
    {
        _rsi.Update(input);
        _updownRsi.Update(input);
        _roc.Update(input);
        
        if (!_rsi.IsReady)
        {
            return 0;
        }

        if (!_updownRsi.IsReady)
        {
            return 0;
        }

        if (!_roc.IsReady)
        {
            return 0;
        }
        
        return (_rsi.Current.Value + _updownRsi.Current.Value + _roc.Current.Value) / 3;
    }

    public int WarmUpPeriod { get; }
}
