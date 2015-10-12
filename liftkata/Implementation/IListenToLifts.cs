namespace liftkata.Implementation
{
    public interface IListenToLifts
    {
        void LiftArrived(int stop);
        void LiftMovedDownwards();
        void LiftMovedUpwards();
    }
}