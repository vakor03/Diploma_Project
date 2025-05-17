namespace _Project.Features.GameTimeModule {
    public interface IGamePauseService
    {
        public bool IsPaused { get; }
        public int PauseCounter { get; }
    
        public void PauseTime();
        public void ResumeTime();
        public void ForceResumeTime();
    }
}