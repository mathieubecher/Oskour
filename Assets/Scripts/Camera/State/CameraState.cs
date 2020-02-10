public abstract class CameraState
{
    protected readonly CameraPointer controller;

    protected CameraState(CameraPointer controller)
    {
        this.controller = controller;
    }
    public abstract void Click();
    public abstract void RightClick();
    public virtual void Release() { }
    public abstract void Hover();

    public virtual void PickState() { }
    public virtual void ConstructState(BuildController build) { controller.state = new Construct(controller, build); }

}
