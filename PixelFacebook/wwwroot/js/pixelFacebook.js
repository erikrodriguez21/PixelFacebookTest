



function pixelFacebook(eventName, userAgent, email, urlSource, monto) {

   
    //SDK Facebook
    //fbq('trackCustom', eventName, { value='' }, { eventID: '' });

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
            document.getElementById('json-request').innerHTML = data;
        },
        error: function (err) {
            alert(err);
        }
    });





    console.log(eventName, email, userAgent, urlSource, monto);

}




