

function pixelFacebook(eventName, userAgent, email, urlSource, guid, monto = null) {

    !function (f, b, e, v, n, t, s) {
        if (f.fbq) return; n = f.fbq = function () {
            n.callMethod ?
                n.callMethod.apply(n, arguments) : n.queue.push(arguments)
        };
        if (!f._fbq) f._fbq = n; n.push = n; n.loaded = !0; n.version = '2.0';
        n.queue = []; t = b.createElement(e); t.async = !0;
        t.src = v; s = b.getElementsByTagName(e)[0];
        s.parentNode.insertBefore(t, s)
    }(window, document, 'script',
        'https://connect.facebook.net/en_US/fbevents.js');
    //SDK
    sdkPixel(eventName, monto, guid)

    //api
    $.ajax({
        type: 'POST',
        url: '/Home/PixelFacebook',
        async: true,
        dataType: 'json',
        data: {
            eventName: eventName,
            userAgent: userAgent,
            email: email,
            urlSource: urlSource,
            monto: monto
        },
        success: function (data) {
            var json = JSON.parse(data);
            console.log('API Pixel Facebook');
        },
        error: function (err) {
            console.log("error API PixelFacebook: " + err.responseText);
        }
    });
}

function sdkPixel(eventName, monto, guid) {
    $.ajax({
        type: 'POST',
        url: '/Home/GetPixelId',
        async: true,
        dataType: 'json',
        success: function (pixelId) {
            //SDK Facebook    
            fbq('init', pixelId);
            fbq('trackCustom', eventName, { currency: monto != null ? 'mxn' : null, value: monto }, { eventID: eventName.replace('_', '') + guid });
            console.log('SDK Pixel Facebook');
        },
        error: function (err) {
            console.log("error SDK PixelFacebook: " + err.responseText);
        }
    });
}






