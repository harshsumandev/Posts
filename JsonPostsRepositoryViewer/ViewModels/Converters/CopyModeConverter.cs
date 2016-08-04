using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONPlaceHolder.ViewModels.Converters
{
    public class CopyModeConverter
    {
        /// <summary>
        /// Converts View level CopyMode to CopyMode at service level
        /// </summary>
        /// <param name="serviceCopyMode"></param>
        /// <returns></returns>
        public static JsonPostsRepositoryService.CopyMode Convert(CopyMode serviceCopyMode)
        {
            JsonPostsRepositoryService.CopyMode copyMode = JsonPostsRepositoryService.CopyMode.TEXT;

            switch (serviceCopyMode)
            {
                case CopyMode.HTML:
                    copyMode = JsonPostsRepositoryService.CopyMode.HTML;
                    break;
                case CopyMode.JSON:
                    copyMode = JsonPostsRepositoryService.CopyMode.JSON;
                    break;
            }

            return copyMode;
        }
    }
}
