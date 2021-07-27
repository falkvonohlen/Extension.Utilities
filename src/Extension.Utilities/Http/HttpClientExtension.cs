using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Extension.Utilities.Http
{
    /// <summary>
    /// Extension methods for the default .net HttpClient
    /// </summary>
    public static class HttpClientExtension
    {
        /// <summary>
        /// Downloads the content of a http response and calls the progress callback to give update about the download progress
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestUrl"></param>
        /// <param name="destination"></param>
        /// <param name="progress"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> DownloadDataAsync(this HttpClient client, string requestUrl, Stream destination, IProgress<long> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var response = await client.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead))
            {
                if (response.IsSuccessStatusCode)
                {
                    var contentLength = response.Content.Headers.ContentLength;
                    using (var download = await response.Content.ReadAsStreamAsync())
                    {
                        // no progress... no contentLength...
                        if (progress is null || !contentLength.HasValue)
                        {
                            await download.CopyToAsync(destination);
                            return response;
                        }
                        // Such progress and contentLength much reporting Wow!
                        var lastprogress = 0;
                        var progressWrapper = new Progress<long>(totalBytes => {
                            var value = GetProgressPercentage(totalBytes, contentLength.Value);
                            if (value > lastprogress)
                            {
                                progress.Report(value);
                                lastprogress = value;
                            }
                        });
                        await CopyToAsync(download, destination, 81920, progressWrapper, cancellationToken);
                        return response;
                    }
                }
                else
                {
                    return response;
                }
            }

            int GetProgressPercentage(double downloadedBytes, double allBytes) => (int)((downloadedBytes / allBytes) * 100);
        }

        /// <summary>
        /// Copy from one stream to another and report the progress
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="bufferSize"></param>
        /// <param name="progress"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        static async Task CopyToAsync(Stream source, Stream destination, int bufferSize, IProgress<long> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (!source.CanRead)
                throw new InvalidOperationException($"'{nameof(source)}' is not readable.");
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));
            if (!destination.CanWrite)
                throw new InvalidOperationException($"'{nameof(destination)}' is not writable.");

            var buffer = new byte[bufferSize];
            long totalBytesRead = 0;
            int bytesRead;
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
                totalBytesRead += bytesRead;
                progress?.Report(totalBytesRead);
            }
        }

    }
}
