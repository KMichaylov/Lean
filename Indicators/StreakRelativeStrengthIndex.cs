namespace QuantConnect.Indicators;

public class StreakRelativeStrengthIndex : Indicator, IIndicatorWarmUpPeriodProvider
{
    private int _currentStreak;
    private IndicatorDataPoint _previousInput;
    private readonly RelativeStrengthIndex _rsi;
    public StreakRelativeStrengthIndex(
        string name, 
        int rsiPeriod)
        : base(name)
    {
        _rsi = new RelativeStrengthIndex($"{name}_StreakRSI", rsiPeriod);
        _currentStreak = 0;
        _previousInput = null;
    }

    public override bool IsReady { get => _rsi.IsReady; } 
    protected override decimal ComputeNextValue(IndicatorDataPoint input)
    {
        if (_previousInput is null)
        {
            _previousInput = input;
            return 0;
        }

        if (_previousInput > input && _currentStreak < 0)
        {
            _currentStreak++;
            _previousInput = input;
            _rsi.Update(input);
            return _rsi.Current.Value;
        }
        else if (_previousInput < input && _currentStreak > 0)
        {
            _currentStreak++;
            _previousInput = input;
            _rsi.Update(input);
            return _rsi.Current.Value;
        }
        else
        {
            _currentStreak = 0;
            _previousInput = input;
            _rsi.Reset();
            return 0;
        }
    }

    public int WarmUpPeriod { get => _rsi.WarmUpPeriod; }
}
