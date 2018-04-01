namespace wServer.realm.setpieces
{
    internal class LordOfTheLostLands : ISetPiece
    {
        public int Size => 5;

        public void RenderSetPiece(World world, IntPoint pos)
        {
            Entity lotl = Entity.Resolve(world.Manager, "Lord of the Lost Lands");
            lotl.Move(pos.X + 2.5f, pos.Y + 2.5f);
            world.EnterWorld(lotl);
        }
    }
}