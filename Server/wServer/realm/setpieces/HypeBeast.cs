namespace wServer.realm.setpieces
{
    internal class HypeBeast : ISetPiece
    {
        public int Size => 5;

        public void RenderSetPiece(World world, IntPoint pos)
        {
            Entity boss = Entity.Resolve(world.Manager, "Hype Beast");
            boss.Move(pos.X + 2.5f, pos.Y + 2.5f);
            world.EnterWorld(boss);
        }
    }
}