namespace OpenTransitFlow.Infra.Assets
{
    public abstract class BaseSwitch
    {
        public BaseTrack InboundTrack { get; set; }
        public BaseTrack MainTrack { get; set; }
        public BaseTrack DivergingTrack { get; set; }
        public int ID;
        public bool IsDiverging { get; set; }
        public void SetStraight()
        {
            IsDiverging = false;
        }
        public void SetDiverging()
        {
            IsDiverging = true;
        }
        protected abstract BaseSwitch CreateNew();

        public List<T> CreateSwitch<T>(T inbound, T main, T secondary) where T : BaseTrack
        {
            var mainTrack = (T)inbound.Join(main, JoinPairEnum.EndToStart);
            var diverging = (T)inbound.Join(secondary, JoinPairEnum.EndToStart);

            return new List<T>() { mainTrack, diverging };
        }
    }
}
