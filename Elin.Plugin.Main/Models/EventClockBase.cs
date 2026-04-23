namespace Elin.Plugin.Main.Models
{
    public interface IEventClock
    {
        #region property

        int MinElapsed { get; }
        int TimeLimit { get; }

        #endregion
    }

    file abstract class EventClockBase<TZoneEvent> : IEventClock
        where TZoneEvent : ZoneEventQuest
    {
        protected EventClockBase(TZoneEvent zoneEvent)
        {
            ZoneEvent = zoneEvent;
        }

        #region property

        protected TZoneEvent ZoneEvent { get; }

        #endregion

        #region IEventClock

        public int MinElapsed => ZoneEvent.minElapsed;

        public abstract int TimeLimit { get; }

        #endregion
    }

    file sealed class HarvestEventClock : EventClockBase<ZoneEventHarvest>
    {
        public HarvestEventClock(ZoneEventHarvest zoneEvent)
            : base(zoneEvent)
        { }

        #region EventClockBase

        public override int TimeLimit => ZoneEvent.TimeLimit;

        #endregion
    }

    file sealed class MusicEventClock : EventClockBase<ZoneEventMusic>
    {
        public MusicEventClock(ZoneEventMusic zoneEvent)
            : base(zoneEvent)
        { }

        #region EventClockBase

        public override int TimeLimit => ZoneEvent.TimeLimit;

        #endregion
    }


    public static class EventClockFactory
    {
        #region function

        public static bool TryCreate(Zone zone, out IEventClock? result)
        {
            //EMono._zone

            var zoneEventHarvest = zone.events.GetEvent<ZoneEventHarvest>();
            if (zoneEventHarvest != null)
            {
                result = new HarvestEventClock(zoneEventHarvest);
                return true;
            }

            var zoneEventMusic = zone.events.GetEvent<ZoneEventMusic>();
            if (zoneEventMusic != null)
            {
                result = new MusicEventClock(zoneEventMusic);
                return true;
            }

            result = null;
            return false;
        }

        #endregion
    }

}
