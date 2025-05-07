using _Project.Features.MapGeneration.BSP;

namespace _Project.Features.MapGeneration {
    public interface IDungeonGeneratorService {
        public Dungeon GenerateDungeon(DungeonGenerationConfiguration config);
    }
}