
$(document).ready(function () {



    var keys = [];
    var konami = '38,38,40,40,37,39,37,39,66,65';

    $(document)
        .keydown(
            function (e) {
                keys.push(e.keyCode);
                if (keys.toString().indexOf(konami) >= 0) {
                    alert("Konami Mode enabled");
                    $("#menubar").hide();
                    $("#audiojs_wrapper0").hide();
                    $(".slides-container").hide();
                    $(".scale-with-grid").hide();
                    $(".rpt").append("<center><img src='/Content/NothingHere/sonicplane.gif' /><h1 style='font-family:Audiowide; color:white'>DERKS WEBZPAG</h1><img src='/Content/NothingHere/sonrun.gif' /></center>");

                    $(".rpt").append("<center><object width='480' height='385'> <param name='movie' value='http://www.youtube.com/v/NbVZPu_JM6I?version=3&autoplay=1'></param> <param name='allowFullScreen' value='true'></param> <param name='allowscriptaccess' value='always'></param><embed src='http://www.youtube.com/v/NbVZPu_JM6I?version=3&autoplay=1' width='480' height='385'  type='application/x-shockwave-flash' allowscriptaccess='always'  allowfullscreen='true'></embed></object></center>");

                    keys = [];
                }
            }
        );
});