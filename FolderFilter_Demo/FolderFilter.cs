using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Controls;
using Microsoft.WindowsAPICodePack.Controls.WindowsForms;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System.Runtime.InteropServices;
using ShellLib;

namespace PhotoGeoTagShell
{
    class FolderFilter : IFolderFilter
    {
        public int GetEnumFlags( [MarshalAs( UnmanagedType.Interface )] object psf, IntPtr pidlFolder, IntPtr phwnd, out uint pgrfFlags )
        {
            throw new NotImplementedException();
        }

        public int ShouldShow( [MarshalAs( UnmanagedType.Interface )] object psf, IntPtr pidlFolder, IntPtr pidlItem )
        {
            //bool fShow = true;
            //ShellItem spsi;
            //HRESULT hr = SHCreateItemWithParent(pidlFolder, psf, pidlItem,
            //                          IID_PPV_ARGS(&spsi));
            //if ( SUCCEEDED( hr ) )
            //{
            //    SFGAOF sfgaof;
            //    hr = spsi->GetAttributes( SFGAO_FILESYSTEM | SFGAO_FOLDER,
            //                             &sfgaof );
            //    if ( SUCCEEDED( hr ) && sfgaof == SFGAO_FILESYSTEM )
            //    {
            //        LPWSTR pszName;
            //        hr = spsi->GetDisplayName( SIGDN_PARENTRELATIVEPARSING,
            //                                     &pszName );
            //        if ( SUCCEEDED( hr ) )
            //        {
            //            fShow = CompareStringOrdinal(
            //                        PathFindExtensionW( pszName ), -1,
            //                        L".dll", -1, TRUE ) != CSTR_EQUAL;
            //            CoTaskMemFree( pszName );
            //        }
            //    }
            //}
            //if ( SUCCEEDED( hr ) ) hr = fShow ? S_OK : S_FALSE;
            //return hr;
            throw new NotImplementedException();
        }
    }
}
