namespace _Project.Features.GameTimeModule {
    public class GamePauseService : IGamePauseService
    {
        private readonly GamePauseModel _model;
    
        public bool IsPaused => _model.IsPaused;
        public int PauseCounter => _model.PauseCounter;
    
        public GamePauseService(GamePauseModel model) =>
            _model = model;

        public void PauseTime() =>
            _model.IncrementPause();

        public void ResumeTime() =>
            _model.DecrementPause();

        public void ForceResumeTime()
        {
            _model.ForceResume();
        }
    }
}