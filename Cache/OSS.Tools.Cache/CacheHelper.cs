#region Copyright (C) 2016 Kevin (OSS开源系列) 公众号：OSSCore

/***************************************************************************
*　　	文件功能描述：全局插件 -  缓存插件辅助类
*
*　　	创建人： Kevin
*       创建人Email：1985088337@qq.com
*       
*       
*****************************************************************************/

#endregion

using System;
using System.Threading.Tasks;

namespace OSS.Tools.Cache
{
    /// <summary>
    /// 缓存的辅助类
    /// </summary>
    public static class CacheHelper
    {
        private static readonly DefaultToolCache defaultCache = new DefaultToolCache();

        /// <summary>
        /// 缓存模块提供者
        /// </summary>
        public static Func<string, IToolCache> CacheProvider { get; set; }

        /// <summary>
        /// 通过模块名称获取
        /// </summary>
        /// <param name="cacheModule"></param>
        /// <returns></returns>
        public static IToolCache GetCache(string cacheModule)
        {
            if (string.IsNullOrEmpty(cacheModule))
                cacheModule = "default";

            return CacheProvider?.Invoke(cacheModule) ?? defaultCache;
        }

        #region 缓存添加

        /// <summary> 
        /// 添加相对过期时间缓存，如果存在则更新
        /// </summary>
        /// <typeparam name="T">添加缓存对象类型</typeparam>
        /// <param name="key">添加对象的key</param>
        /// <param name="obj">值</param>
        /// <param name="slidingExpiration">相对过期的TimeSpan</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns>是否添加成功</returns>
        [Obsolete("请使用 SetAsync")]
        public static bool Set<T>(string key, T obj, TimeSpan slidingExpiration,string moduleName = "default")
        {
            return SetAsync(key, obj, slidingExpiration).Result;
        }

        /// <summary>
        /// 添加固定过期时间缓存，如果存在则更新
        /// </summary>
        /// <typeparam name="T">添加缓存对象类型</typeparam>
        /// <param name="key">添加对象的key</param>
        /// <param name="obj">值</param>
        /// <param name="absoluteExpiration"> 绝对过期时间 </param>
        /// <param name="moduleName">模块名称</param>
        /// <returns>是否添加成功</returns>
        [Obsolete("请使用 SetAbsoluteAsync")]
        public static bool Set<T>(string key, T obj, DateTime absoluteExpiration,string moduleName = "default")
        {
            return SetAbsoluteAsync(key,obj,TimeSpan.FromTicks((absoluteExpiration-DateTime.Now).Ticks)).Result;
        }

        /// <summary> 
        /// 添加相对过期时间缓存，如果存在则更新
        /// </summary>
        /// <typeparam name="T">添加缓存对象类型</typeparam>
        /// <param name="key">添加对象的key</param>
        /// <param name="obj">值</param>
        /// <param name="slidingExpiration">相对过期的TimeSpan</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns>是否添加成功</returns>
        public static Task<bool> SetAsync<T>(string key, T obj, TimeSpan slidingExpiration,
            string moduleName = "default")
        {
            return SetAsync(key, obj, slidingExpiration, null, moduleName);
        }

        /// <summary>
        /// 添加固定过期时间缓存，如果存在则更新
        /// </summary>
        /// <typeparam name="T">添加缓存对象类型</typeparam>
        /// <param name="key">添加对象的key</param>
        /// <param name="obj">值</param>
        /// <param name="absoluteExpiration"> 绝对过期时间 </param>
        /// <param name="moduleName">模块名称</param>
        /// <returns>是否添加成功</returns>
        public static Task<bool> SetAbsoluteAsync<T>(string key, T obj, TimeSpan absoluteExpiration,string moduleName = "default")
        {
            return SetAsync(key, obj, null, absoluteExpiration, moduleName);
        }

        #endregion

        #region 缓存获取

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">获取缓存对象类型</typeparam>
        /// <param name="key">key</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns>获取指定key对应的值 </returns>
        [Obsolete("请使用 GetAsync")]
        public static Task<T> Get<T>(string key, string moduleName = "default")
        {
            return GetAsync<T>(key);
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">获取缓存对象类型</typeparam>
        /// <param name="key">key</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns>获取指定key对应的值 </returns>
        public static Task<T> GetAsync<T>(string key, string moduleName = "default")
        {
            return GetCache(moduleName).GetAsync<T>(key);
        }

        /// <summary>
        /// 获取缓存数据，如果没有则添加
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="getFunc">如果不存在，通过此方法获取原始数据添加缓存</param>
        /// <param name="slidingExpiration">过期时长，访问后自动延长</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public static Task<RType> GetOrSetAsync<RType>(string cacheKey, Func<Task<RType>> getFunc,
            TimeSpan slidingExpiration, string moduleName = "default")
        {
            return Get(cacheKey, getFunc, slidingExpiration, null,  moduleName);
        }

        /// <summary>
        /// 获取缓存数据，如果没有则添加
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="getFunc">没有数据时，通过此方法获取原始数据</param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public static Task<RType> GetOrSetAbsoluteAsync<RType>(string cacheKey, Func<Task<RType>> getFunc,
            TimeSpan absoluteExpiration, string moduleName = "default")
        {
            return Get(cacheKey, getFunc, null, absoluteExpiration,  moduleName);
        }

        #endregion

        #region 缓存获取（击穿保护）

        /// <summary>
        /// 获取缓存数据【同时添加缓存击穿保护】，如果没有则添加
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="getFunc">没有数据时，通过此方法获取原始数据</param>
        /// <param name="slidingExpiration">过期时长（相对滚动时间），访问后自动延长</param>
        /// <param name="failProtectCondition">缓存击穿保护触发条件</param>
        /// <param name="protectSeconds">缓存击穿保护秒数</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public static Task<RType> GetOrSetAsync<RType>(string cacheKey, Func<Task<RType>> getFunc,TimeSpan slidingExpiration,
            Func<RType, bool> failProtectCondition, int protectSeconds = 10, 
            string moduleName = "default")
        {
            return ProtectedGet(cacheKey, getFunc, slidingExpiration, null, failProtectCondition, protectSeconds, moduleName);
        }

        /// <summary>
        /// 获取缓存数据【同时添加缓存击穿保护】，如果没有则添加
        /// </summary>
        /// <typeparam name="RType"></typeparam>
        /// <param name="cacheKey">key</param>
        /// <param name="getFunc">如果不存在，通过此方法获取原始数据添加缓存</param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        /// <param name="failProtectCondition">缓存击穿保护触发条件</param>
        /// <param name="protectSeconds">缓存击穿保护秒数</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public static Task<RType> GetOrSetAbsoluteAsync<RType>(string cacheKey, Func<Task<RType>> getFunc,TimeSpan absoluteExpiration,
            Func<RType, bool> failProtectCondition, int protectSeconds = 10,
            string moduleName = "default")
        {
            return ProtectedGet(cacheKey, getFunc, TimeSpan.Zero, absoluteExpiration, failProtectCondition, protectSeconds, moduleName);
        }
        
        #endregion

        /// <summary>
        /// 移除缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="moduleName">模块名称</param>
        /// <returns>是否成功</returns>
        public static Task<bool> RemoveAsync(string key, string moduleName = "default")
        {
            return GetCache(moduleName).RemoveAsync(key);
        }

        [Obsolete("请使用 RemoveAsync")]
        public static bool Remove(string key, string moduleName = "default")
        {
            return RemoveAsync(key).Result;
        }

        private static async Task<RType> Get<RType>(string cacheKey, Func<Task<RType>> createFunc
            , TimeSpan? slidingExpiration, TimeSpan? absoluteExpiration, string moduleName)
        {
            var obj =await GetAsync<RType>(cacheKey);
            if (obj != null)
                return obj;

            if (createFunc == null)
                return default;

            var data = await createFunc.Invoke();
            if (data == null)
                return default;
            
            await SetAsync(cacheKey, data, absoluteExpiration, slidingExpiration, moduleName);
            return data;
        }

        private static async Task<RType> ProtectedGet<RType>(string cacheKey, Func<Task<RType>> getFunc
            , TimeSpan? slidingExpiration, TimeSpan? absoluteExpiration,
            Func<RType, bool> hitProtectCondition, int hitProtectSeconds, string moduleName)
        {
            if (getFunc == null)
                throw new ArgumentNullException("获取原始数据方法(getFunc)不能为空!");

            var obj = await GetAsync<ProtectCahceData<RType>>(cacheKey);
            if (obj != null)
                return obj.Data;

            var data = await getFunc();

            var hitTrigger = hitProtectCondition?.Invoke(data) ?? data == null;
            if (hitTrigger)
            {
                absoluteExpiration = TimeSpan.FromSeconds(hitProtectSeconds);
                slidingExpiration  = null;
            }

            var cacheData = new ProtectCahceData<RType>(data);
            await SetAsync(cacheKey, cacheData, absoluteExpiration, slidingExpiration, moduleName);

            return data;
        }

        private static Task<bool> SetAsync<T>(string key, T obj,
            TimeSpan? slidingExpiration, TimeSpan? absoluteExpiration, string moduleName = "default")
        {
            return GetCache(moduleName).SetAsync(key, obj, new CacheTimeOptions()
            {
                AbsoluteExpirationRelativeToNow = absoluteExpiration,
                SlidingExpiration = slidingExpiration ,
                ModuleName = moduleName
            });
        }
    }


    internal class ProtectCahceData<TT>
    {
        public ProtectCahceData(TT data)
        {
            Data = data;
        }

        public TT Data { get;  }
    }
}
