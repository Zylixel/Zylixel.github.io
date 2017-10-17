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

using System;
using System.IO;
using System.IO.Compression;

namespace DungeonGenerator {
	public static class Zlib {
		static uint Adler32(byte[] data) {
			const uint modulo = 0xfff1;
			uint a = 1, b = 0;
			for (int i = 0; i < data.Length; i++) {
				a = (a + data[i]) % modulo;
				b = (b + a) % modulo;
			}
			return (b << 16) | a;
		}

		public static byte[] Compress(byte[] buffer) {
			byte[] comp;
			using (var output = new MemoryStream()) {
				using (var deflate = new DeflateStream(output, CompressionMode.Compress))
					deflate.Write(buffer, 0, buffer.Length);
				comp = output.ToArray();
			}

			// Refer to http://www.ietf.org/rfc/rfc1950.txt for zlib format
			const byte cm = 8;
			const byte cinfo = 7;
			const byte cmf = cm | (cinfo << 4);
			const byte flg = 0xDA;

			byte[] result = new byte[comp.Length + 6];
			result[0] = cmf;
			result[1] = flg;
			Buffer.BlockCopy(comp, 0, result, 2, comp.Length);

			uint cksum = Adler32(buffer);
			var index = result.Length - 4;
			result[index++] = (byte)(cksum >> 24);
			result[index++] = (byte)(cksum >> 16);
			result[index++] = (byte)(cksum >> 8);
			result[index++] = (byte)(cksum >> 0);

			return result;
		}
	}
}