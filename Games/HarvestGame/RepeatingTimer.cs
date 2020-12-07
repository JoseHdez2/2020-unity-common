
/// <summary>
/// Basic timer class, for use in Monobehaviors.
/// </summary>
public class RepeatingTimer {

    public RepeatingTimer(float secsPerUpdate){
        this.secsPerUpdate = secsPerUpdate;
        Reset();
    }
    public float secsPerUpdate;
    private float secsToUpdate = 0f;

    public void Reset(){
        secsToUpdate = secsPerUpdate;
    }

    /// <summary>
    /// Update timer and return `true` if the time ended this frame, `false` otherwise.
    /// </summary>
    public bool UpdateAndCheck(float deltaTime) {
        secsToUpdate -= deltaTime;
        if(secsToUpdate <= 0){
            secsToUpdate += secsPerUpdate;
            return true;
        } else return false;
    }
}