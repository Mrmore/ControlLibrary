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
        /// .3gp MS code 36
        /// </summary>
        Quality240P_3GP = 36,
        /// <summary>
        /// .mp4 SQ code 18
        /// </summary>
        Quality360P_MP4 = 18,//常用
        /// <summary>
        /// .mp4 HD code 22
        /// </summary>
        Quality720P_MP4 = 22,//常用
        /// <summary>
        /// .mp4 Full HD code 37
        /// </summary>
        Quality1080P_MP4 = 37,//常用
        /// <summary>
        /// .flv HQ 44khz code 35
        /// </summary>
        Quality480P_FLV_44KHZ = 35,//转码用
        /// <summary>
        /// .flv HQ 22khz code 34
        /// </summary>
        Quality360P_FLV_22KHZ = 34,//转码用
        /// <summary>
        /// .flv LQ mp3.44khz code 6
        /// </summary>
        QualityMP3_FLV_44KHZ = 6,//转码用(一般没有这种)
        /// <summary>
        /// .flv LQ mp3.22khz code 5
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
        YouTubeQuality = 4,
        MP4OrFlv_mp3
    }
}
