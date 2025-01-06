namespace TimToolBox.DesignPattern.EventBus {
    public interface IEvent { }
    public struct TestEvent : IEvent {
    }
    public struct TestIntEvent : IEvent {
        public int Val;
    }
}