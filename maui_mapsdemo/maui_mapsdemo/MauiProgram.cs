﻿using Microsoft.Extensions.Logging;

namespace maui_mapsdemo;

public static class MauiProgram
{
    //	public static MauiApp CreateMauiApp()
    //	{
    //		var builder = MauiApp.CreateBuilder();
    //		builder
    //			.UseMauiApp<App>()
    //			.ConfigureFonts(fonts =>
    //			{
    //				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
    //				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
    //			})
    //            .UseMauiMaps();

    //#if DEBUG
    //		builder.Logging.AddDebug();
    //#endif

    //		return builder.Build();
    //	}

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().UseMauiMaps();
              
        builder.ConfigureMauiHandlers(handlers =>
        {
#if ANDROID
            //handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, CustomMapHandler>();
#endif
        });

        return builder.Build();
    }
}

