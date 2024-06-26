﻿
namespace RedFox.Compression.ZStandard
{
    /// <summary>
    /// Methods to decompress/compress ZStandard data
    /// </summary>
    public unsafe static class ZStandard
    {
        /// <summary>
        /// Calculates the maximum possible size for the given data size 
        /// </summary>
        /// <returns>Maximum possible size for the given data size </returns>
        public static int CompressBound(int size) => ZStandardInterop.ZSTD_compressBound(size);

        /// <summary>
        /// Decompresses the provided data
        /// </summary>
        /// <param name="inputBuffer">Input buffer</param>
        /// <param name="outputSize">Size of the data</param>
        /// <returns>Resulting buffer</returns>
        public static byte[] Decompress(byte[] inputBuffer, int outputSize)
        {
            if (inputBuffer is null)
                throw new ArgumentNullException(nameof(inputBuffer), "Input Buffer cannot be null");
            if (outputSize < 0)
                throw new ArgumentOutOfRangeException(nameof(outputSize), "Output size must be a non-negative number");

            fixed (byte* a = &inputBuffer[0])
            {
                var rawBufSize = outputSize;

                var rawBuf = new byte[rawBufSize];

                fixed (byte* b = &rawBuf[0])
                {
                    var result = ZStandardInterop.ZSTD_decompress(
                        b,
                        rawBufSize,
                        a,
                        inputBuffer.Length);

                    if (result != rawBufSize && result < 0)
                        throw new CompressionException($"Failed to decompress data: {new string(ZStandardInterop.ZSTD_getErrorName(result))}");
                }

                return rawBuf;
            }
        }

        /// <summary>
        /// Decompresses the provided data
        /// </summary>
        /// <param name="inputBuffer">Input buffer</param>
        /// <returns>Resulting buffer</returns>
        public static byte[] Decompress(byte[] inputBuffer)
        {
            if (inputBuffer is null)
                throw new ArgumentNullException(nameof(inputBuffer), "Input Buffer cannot be null");

            fixed (byte* a = &inputBuffer[0])
            {
                var rawBufSize = ZStandardInterop.ZSTD_getFrameContentSize(a, inputBuffer.Length);

                if (rawBufSize < 0)
                    throw new CompressionException($"Failed to get ZStandard Frame Content Size: {new string(ZStandardInterop.ZSTD_getErrorName(rawBufSize))}");

                var rawBuf = new byte[rawBufSize];

                fixed (byte* b = &rawBuf[0])
                {
                    var result = ZStandardInterop.ZSTD_decompress(
                        b,
                        rawBufSize,
                        a,
                        inputBuffer.Length);

                    if (result != rawBufSize && result < 0)
                        throw new CompressionException($"Failed to decompress data: {new string(ZStandardInterop.ZSTD_getErrorName(result))}");
                }

                return rawBuf;
            }
        }

        /// <summary>
        /// Decompresses the provided data
        /// </summary>
        /// <param name="inputBuffer">Input buffer</param>
        /// <param name="inputOffset">Offset within the buffer of the data</param>
        /// <param name="inputSize">Size of the data</param>
        /// <param name="outputBuffer">Buffer to output to</param>
        /// <param name="outputOffset">Offset within the buffer to place the data</param>
        /// <param name="outputSize">Size of the data</param>
        /// The total number of bytes placed into the buffer.
        /// This can be less than the number of bytes allocated in the buffer if that many bytes are not currently available,
        /// or zero (0) if the end of the data has been reached.</returns>
        public static int Decompress(byte[] inputBuffer, int inputOffset, int inputSize, byte[] outputBuffer, int outputOffset, int outputSize)
        {
            if (inputBuffer is null)
                throw new ArgumentNullException(nameof(inputBuffer), "Input Buffer cannot be null");
            if (inputOffset < 0)
                throw new ArgumentOutOfRangeException(nameof(inputOffset), "Input offset must be a non-negative number");
            if ((uint)inputSize > inputBuffer.Length - inputOffset)
                throw new ArgumentOutOfRangeException(nameof(inputSize), "Input size is outside the bounds of the buffer");

            if (outputBuffer is null)
                throw new ArgumentNullException(nameof(outputBuffer), "Output Buffer cannot be null");
            if (outputOffset < 0)
                throw new ArgumentOutOfRangeException(nameof(outputOffset), "Output offset must be a non-negative number");
            if (outputSize < 0 || outputSize > outputBuffer.Length - outputOffset)
                throw new ArgumentOutOfRangeException(nameof(outputSize), "Output size is outside the bounds of the buffer");


            fixed (byte* a = &inputBuffer[0])
            {
                var rawBufSize = outputSize;

                fixed (byte* b = &outputBuffer[0])
                {
                    var result = ZStandardInterop.ZSTD_decompress(
                        b + outputOffset,
                        rawBufSize,
                        a + inputOffset,
                        inputSize);

                    if (result < 0)
                        return 0;

                    return result;
                }
            }
        }

        /// <summary>
        /// Decompresses the provided data
        /// </summary>
        /// <param name="inputBuffer">Input buffer</param>
        /// <param name="inputOffset">Offset within the buffer of the data</param>
        /// <param name="inputSize">Size of the data</param>
        /// <param name="outputBuffer">Buffer to output to</param>
        /// <param name="outputOffset">Offset within the buffer to place the data</param>
        /// The total number of bytes placed into the buffer.
        /// This can be less than the number of bytes allocated in the buffer if that many bytes are not currently available,
        /// or zero (0) if the end of the data has been reached.</returns>
        public static int Decompress(byte[] inputBuffer, int inputOffset, int inputSize, byte[] outputBuffer, int outputOffset)
        {
            if (inputBuffer is null)
                throw new ArgumentNullException(nameof(inputBuffer), "Input Buffer cannot be null");
            if (inputOffset < 0)
                throw new ArgumentOutOfRangeException(nameof(inputSize), "Input offset must be a non-negative number");
            if (inputSize < 0 || inputSize > inputBuffer.Length - inputOffset)
                throw new ArgumentOutOfRangeException(nameof(inputOffset), "Input size is outside the bounds of the buffer");

            if (outputBuffer is null)
                throw new ArgumentNullException(nameof(outputBuffer), "Output Buffer cannot be null");
            if (outputOffset < 0)
                throw new ArgumentOutOfRangeException(nameof(outputOffset), "Output offset must be a non-negative number");

            fixed (byte* a = &inputBuffer[0])
            {
                var rawBufSize = ZStandardInterop.ZSTD_getFrameContentSize(a, inputBuffer.Length);

                if (rawBufSize < 0)
                    throw new CompressionException($"Failed to get ZStandard Frame Content Size: {new string(ZStandardInterop.ZSTD_getErrorName(rawBufSize))}");
                if (rawBufSize > outputBuffer.Length - outputOffset)
                    throw new ArgumentOutOfRangeException(nameof(rawBufSize), "Output size is outside the bounds of the buffer");

                fixed (byte* b = &outputBuffer[0])
                {
                    var result = ZStandardInterop.ZSTD_decompress(
                        b + outputOffset,
                        rawBufSize,
                        a + inputOffset,
                        inputSize);

                    if (result < 0)
                        return 0;

                    return result;
                }
            }
        }

        /// <summary>
        /// Compresses the provided data
        /// </summary>
        /// <param name="inputBuffer">Input buffer</param>
        /// <returns>Compressed buffer</returns>
        public static byte[] Compress(byte[] inputBuffer) =>
            Compress(inputBuffer, 8);

        /// <summary>
        /// Compresses the provided data
        /// </summary>
        /// <param name="inputBuffer">Input buffer</param>
        /// <param name="level">Compression level, this can affect compression speed and ratio, default is 8</param>
        /// <returns>Compressed buffer</returns>
        public static byte[] Compress(byte[] inputBuffer, int level)
        {
            if (inputBuffer is null)
                throw new ArgumentNullException(nameof(inputBuffer), "Input Buffer cannot be null");

            var temp = new byte[ZStandardInterop.ZSTD_compressBound(inputBuffer.Length)];

            fixed (byte* a = &inputBuffer[0])
            {
                fixed (byte* b = &temp[0])
                {
                    var result = ZStandardInterop.ZSTD_compress(
                        b,
                        temp.Length,
                        a,
                        inputBuffer.Length,
                        level);

                    if (result < 0)
                        throw new CompressionException($"Failed to compress data: {new string(ZStandardInterop.ZSTD_getErrorName(result))}");

                    var output = new byte[result];
                    Buffer.BlockCopy(temp, 0, output, 0, result);

                    return output;
                }
            }
        }

        /// <summary>
        /// Compresses the provided data
        /// </summary>
        /// <param name="inputBuffer">Input buffer</param>
        /// <param name="inputOffset">Offset within the buffer of the data</param>
        /// <param name="inputSize">Size of the data</param>
        /// <param name="outputBuffer">Buffer to output to</param>
        /// <param name="outputOffset">Offset within the buffer to place the data</param>
        /// <param name="outputSize">Size of the data</param>
        /// The total number of bytes placed into the buffer.
        /// This can be less than the number of bytes allocated in the buffer if that many bytes are not currently available,
        /// or zero (0) if the end of the data has been reached.</returns>
        public static int Compress(byte[] inputBuffer, int inputOffset, int inputSize, byte[] outputBuffer, int outputOffset, int outputSize) =>
            Compress(inputBuffer, inputOffset, inputSize, outputBuffer, outputOffset, outputSize, 8);

        /// <summary>
        /// Compresses the provided data
        /// </summary>
        /// <param name="inputBuffer">Input buffer</param>
        /// <param name="inputOffset">Offset within the buffer of the data</param>
        /// <param name="inputSize">Size of the data</param>
        /// <param name="outputBuffer">Buffer to output to</param>
        /// <param name="outputOffset">Offset within the buffer to place the data</param>
        /// <param name="outputSize">Size of the data</param>
        /// <param name="level">Compression level, this can affect compression speed and ratio, default is 8</param>
        /// The total number of bytes placed into the buffer.
        /// This can be less than the number of bytes allocated in the buffer if that many bytes are not currently available,
        /// or zero (0) if the end of the data has been reached.</returns>
        public static int Compress(byte[] inputBuffer, int inputOffset, int inputSize, byte[] outputBuffer, int outputOffset, int outputSize, int level)
        {
            if (inputBuffer is null)
                throw new ArgumentNullException(nameof(inputBuffer), "Input Buffer cannot be null");
            if (inputOffset < 0)
                throw new ArgumentOutOfRangeException(nameof(inputOffset), "Input offset must be a non-negative number");
            if (inputSize < 0 || inputSize > inputBuffer.Length - inputOffset)
                throw new ArgumentOutOfRangeException(nameof(inputSize), "Input size is outside the bounds of the buffer");

            if (outputBuffer is null)
                throw new ArgumentNullException(nameof(outputBuffer), "Output Buffer cannot be null");
            if (outputOffset < 0)
                throw new ArgumentOutOfRangeException(nameof(outputOffset), "Output offset must be a non-negative number");
            if (outputSize < 0 || outputSize > outputBuffer.Length - outputOffset)
                throw new ArgumentOutOfRangeException(nameof(outputSize), "Output size is outside the bounds of the buffer");

            fixed (byte* a = &inputBuffer[0])
            {
                fixed (byte* b = &outputBuffer[0])
                {
                    var result = ZStandardInterop.ZSTD_compress(
                        b + outputOffset,
                        outputSize,
                        a + inputOffset,
                        inputSize,
                        level);

                    if (result < 0)
                        return 0;

                    return result;
                }
            }
        }
    }
}
