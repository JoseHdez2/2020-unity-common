
using System;
/// <summary>
/// Basic timer class, for use in Monobehaviors.
/// </summary>
public abstract class AbsTimer {
    public float periodInSecs;
    protected float secondsLeft = 0f;

    public AbsTimer(float periodInSecs){
        this.periodInSecs = periodInSecs;
        Reset();
    }

    public void Reset(){
        secondsLeft = periodInSecs;
    }

    /// <summary>Update timer and return `true` if the time ended this frame, `false` otherwise.</summary>
    public bool UpdateAndCheck(float deltaTime) {
        secondsLeft -= deltaTime;
        if(secondsLeft <= 0){
            OnPeriodEnd();
            return true;
        } else return false;
    }

    protected abstract void OnPeriodEnd();
}

public class RepeatingTimer : AbsTimer {
    public RepeatingTimer(float periodInSecs) : base(periodInSecs) {}
    protected override void OnPeriodEnd() {
        secondsLeft += periodInSecs;
    }
}

public class OneTimeTimer : AbsTimer {
    public OneTimeTimer(float periodInSecs) : base(periodInSecs) {}
    protected override void OnPeriodEnd() {
        secondsLeft = float.MaxValue; // FIXME
    }
}

public class IncrementalTimer : AbsTimer {
    private float periodIncrement;

    public IncrementalTimer(float periodInSecs, float periodIncrement) : base(periodInSecs) {
        this.periodIncrement = periodIncrement;
    }
    protected override void OnPeriodEnd() {
        periodInSecs += periodIncrement;
        secondsLeft += periodInSecs;
    }
}

public class ExponentialTimer : AbsTimer {
    private float periodIncrRatio;

    public ExponentialTimer(float periodInSecs, float periodIncrRatio) : base(periodInSecs) {
        this.periodIncrRatio = periodIncrRatio;
    }
    protected override void OnPeriodEnd() {
        periodInSecs *= periodIncrRatio;
        secondsLeft += periodInSecs;
    }
}