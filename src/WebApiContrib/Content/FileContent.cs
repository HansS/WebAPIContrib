using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContrib.Content
{
    public class FileContent : HttpContent
    {
        private readonly FileStream _Content;

        public FileContent(string filename)
        {
            _Content = new FileStream(filename, FileMode.Open);
        }


        protected override Task<Stream> CreateContentReadStreamAsync()
        {
            return Task.Factory.StartNew<Stream>(() => _Content);
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return Task.Factory.StartNew(() => _Content.CopyTo(stream));
        }



        protected override bool TryComputeLength(out long length)
        {
            length = _Content.Length;
            return true;
        }
    }
}
