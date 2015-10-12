namespace liftkata
{
    public interface IListenToLifts
    {
        void LiftArrived(int stop);
        void LiftMovedDownwards();
        void LiftMovedUpwards();
    }
}