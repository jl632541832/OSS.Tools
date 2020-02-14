using System;

namespace OSS.Tools.Cache
{
     public class CacheTimeOptions
    {
        private TimeSpan? _absoluteExpirationRelativeNow;
        private TimeSpan? _slidingExpiration;

        /// <summary>
        ///   模块名称
        /// </summary>
        public string ModuleName { get; set; } = "default";

        /// <summary>
        /// Gets or sets an absolute expiration time, relative to now.
        /// </summary>
        public TimeSpan? AbsoluteExpirationRelativeToNow
        {
            get => _absoluteExpirationRelativeNow;
            set
            {
                var a = value;

                if (a.HasValue&&a.Value<=TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException(nameof(AbsoluteExpirationRelativeToNow), value, "The relative expiration value must be positive.");
             
                _absoluteExpirationRelativeNow = value;
            }
        }

        /// <summary>
        /// Gets or sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed.
        /// This will not extend the entry lifetime beyond the absolute expiration (if set).
        /// </summary>
        public TimeSpan? SlidingExpiration
        {
            get => _slidingExpiration;
            set
            {
                var a = value;

                if (a.HasValue && a.Value <= TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException(nameof(SlidingExpiration), (object) value, "The relative expiration value must be positive.");

                _slidingExpiration = value;
            }
        }
    }
}
