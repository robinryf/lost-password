#if UNITY_2020_1_OR_NEWER
using Unity.Collections;

namespace FlatBuffers
{
	public partial class ByteBuffer
	{
		public NativeArray<byte> ToNativeArray()
		{
			var nativeArraySize = Length - Position;
			var nativeArray = new NativeArray<byte>(nativeArraySize, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			NativeArray<byte>.Copy(_buffer.Buffer, Position, nativeArray, 0, nativeArraySize);
			return nativeArray;
		}
	}
}
#endif