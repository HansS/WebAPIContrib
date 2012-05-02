using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContrib.Formatting
{
    public class SimpleFormatter<T> : MediaTypeFormatter
    {
        private readonly Action<T, Stream> _write;
        private readonly Func<Stream, T> _read;

        public SimpleFormatter(MediaTypeHeaderValue mediatype, Func<Stream, T> read, Action<T, Stream> write)
        {
            _write = write;
            _read = read;
            SupportedMediaTypes.Add(mediatype);
        }

        protected override bool CanReadType(Type type)
        {
            return _read != null && type == typeof(T);
        }

        protected override bool CanWriteType(Type type)
        {
            return _write != null && type == typeof(T);
        }

        protected override System.Threading.Tasks.Task<object> OnReadFromStreamAsync(Type type, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext)
        {
            return new TaskFactory<object>().StartNew(() => _read(stream));
        }


        protected override Task OnWriteToStreamAsync(Type type, object value, Stream stream, HttpContentHeaders contentHeaders, FormatterContext formatterContext, TransportContext transportContext)
        {
            return new TaskFactory().StartNew(() => _write((T)value, stream));
        }


    }
}
