using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary.Tools.Multimedia
{
    /// <summary>
    /// 视频格式
    /// </summary>
    public enum YouTubeQuality
    {
        /// <summary>
        /// .3gp MS
        /// </summary>
        Quality240P_3GP = 36,
        /// <summary>
        /// .mp4 SQ
        /// </summary>
        Quality360P_MP4 = 18,//常用
        /// <summary>
        /// .mp4 HD
        /// </summary>
        Quality720P_MP4 = 22,//常用
        /// <summary>
        /// .mp4 Full HD
        /// </summary>
        Quality1080P_MP4 = 37,//常用
        /// <summary>
        /// .flv HQ 44khz
        /// </summary>
        Quality480P_FLV_44KHZ = 35,//转码用
        /// <summary>
        /// .flv HQ 22khz
        /// </summary>
        Quality360P_FLV_22KHZ = 34,//转码用
        /// <summary>
        /// .flv LQ mp3.44khz
        /// </summary>
        QualityMP3_FLV_44KHZ = 6,//转码用(一般没有这种)
        /// <summary>
        /// .flv LQ mp3.22khz
        /// </summary>
        QualityMP3_FLV_22KHZ = 5,//转码用
    }

    /// <summary>
    /// 获取那些指定的视频格式
    /// </summary>
    public enum YouTubeFormat
    {
        All = 0,
        Mp4 = 1,
        Flv = 2,
        Mp3 = 3,
        YouTubeQuality = 4
    }
}
