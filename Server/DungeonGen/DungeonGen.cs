using DungeonGenerator.Templates;

namespace DungeonGenerator
{
    public class DungeonGen
    {
        private readonly int _seed;
        private readonly Generator _gen;
        private Rasterizer _ras;

        public DungeonGen(int seed, DungeonTemplate template)
        {
            this._seed = seed;

            _gen = new Generator(seed, template);
        }

        public void GenerateAsync()
        {
            _gen.Generate();
            if (_ras == null)
                _ras = new Rasterizer(_seed, _gen.ExportGraph());
            _ras.Rasterize();
        }

        public string ExportToJson() => JsonMap.Save(_ras.ExportMap());
    }
}
