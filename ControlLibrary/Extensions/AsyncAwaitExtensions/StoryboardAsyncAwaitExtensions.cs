using ControlLibrary.Tools.AwaitableUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace ControlLibrary.Extensions
{
    public static class StoryboardAsyncAwaitExtensions
    {
        /// <summary>
        /// 开始一个故事模版且等待它完成
        /// </summary>
        public static async Task BeginAsync(this Storyboard storyboard)
        {
            await EventAsync.FromEvent<object>(
                eh => storyboard.Completed += eh,
                eh => storyboard.Completed -= eh,
                storyboard.Begin);
        }
    }
}
