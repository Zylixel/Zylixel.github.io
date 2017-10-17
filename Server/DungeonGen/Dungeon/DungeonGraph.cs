/*
    Copyright (C) 2015 creepylava

    This file is part of RotMG Dungeon Generator.

    RotMG Dungeon Generator is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/

using DungeonGenerator.Templates;
using RotMG.Common.Rasterizer;

namespace DungeonGenerator.Dungeon {
	public class DungeonGraph {
		public DungeonTemplate Template { get; }

		public int Width { get; }
		public int Height { get;}

		public Room[] Rooms { get; }

		internal DungeonGraph(DungeonTemplate template, Room[] rooms) {
			Template = template;

			int dx = int.MaxValue, dy = int.MaxValue;
			int mx = int.MinValue, my = int.MinValue;

			foreach (Room t in rooms)
			{
			    var bounds = t.Bounds;

			    if (bounds.X < dx)
			        dx = bounds.X;
			    if (bounds.Y < dy)
			        dy = bounds.Y;

			    if (bounds.MaxX > mx)
			        mx = bounds.MaxX;
			    if (bounds.MaxY > my)
			        my = bounds.MaxY;
			}

			const int pad = 4;

			Width = mx - dx + pad * 2;
			Height = my - dy + pad * 2;

			for (int i = 0; i < rooms.Length; i++) {
				var room = rooms[i];
				var pos = room.Pos;
				room.Pos = new Point(pos.X - dx + pad, pos.Y - dy + pad);

				foreach (var edge in room.Edges) {
					if (edge.RoomA != room)
						continue;
					if (edge.Linkage.Direction == Direction.South || edge.Linkage.Direction == Direction.North)
						edge.Linkage = new Link(edge.Linkage.Direction, edge.Linkage.Offset - dx + pad);
					else if (edge.Linkage.Direction == Direction.East || edge.Linkage.Direction == Direction.West)
						edge.Linkage = new Link(edge.Linkage.Direction, edge.Linkage.Offset - dy + pad);
				}
			}
			Rooms = rooms;
		}
	}
}