using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace ControlLibrary.Tools
{
    public static class MicrosoftStreamExtensions
    {
        public static IRandomAccessStream AsRandomAccessStream(this Stream stream)
        {
            return new RandomStream(stream);
        }
    }

    class RandomStream : IRandomAccessStream
    {
        Stream internstream;
        public RandomStream(Stream underlyingstream)
        {
            internstream = underlyingstream;
        }

        public bool CanRead
        {
            get { throw new NotImplementedException(); }
        }

        public bool CanWrite
        {
            get { throw new NotImplementedException(); }
        }

        public IRandomAccessStream CloneStream()
        {
            throw new NotImplementedException();
        }

        public IInputStream GetInputStreamAt(ulong position)
        {
            internstream.Position = (long)position;
            return internstream.AsInputStream();
        }

        public IOutputStream GetOutputStreamAt(ulong position)
        {
            internstream.Position = (long)position;
            return internstream.AsOutputStream();
        }

        public ulong Position
        {
            get { throw new NotImplementedException(); }
        }

        public void Seek(ulong position)
        {
            throw new NotImplementedException();
        }

        public ulong Size
        {
            get
            {
                return (ulong)internstream.Length;
            }
            set
            {
                internstream.SetLength((long)value);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Windows.Foundation.IAsyncOperationWithProgress<IBuffer, uint> ReadAsync(IBuffer buffer, uint count, InputStreamOptions options)
        {
            throw new NotImplementedException();
        }

        public Windows.Foundation.IAsyncOperation<bool> FlushAsync()
        {
            throw new NotImplementedException();
        }

        public Windows.Foundation.IAsyncOperationWithProgress<uint, uint> WriteAsync(IBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
