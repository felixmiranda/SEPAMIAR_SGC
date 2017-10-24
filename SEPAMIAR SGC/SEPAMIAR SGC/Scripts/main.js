var storage = window.localStorage;

$(document).ready(function () {
    console.log(storage)

    if(storage.getItem("prospectos") == "true"){
        $("#form-prospectos").remove();
    }

    $("#results, #profile .card, .card, .client-notes-container").niceScroll({
        cursorcolor: "#ccc",
        cursorwidth: "10px"
    });

    $("a[href='#']").click(function (e) {
        e.preventDefault();

        alert("This link does not take you anywhere.");
    });

    $("*[data-role='link']").click(function () {
        window.location.href = $(this).data('href');
    });

    $("*[data-role='modal']").click(function (e) {
        e.preventDefault();

        blurElement("#page-content", 5);
        $($(this).data('target')).addClass('in').hide().fadeIn(500);
    });

    $("#create-cliente").click(function (e) {
        var form = $("#form-prospectos");

        if (form.length > 0) {
            var url = form.attr("action");
            var data = form.serialize();

            $.post(url, data, function (request) {
                if(request.status == 200){
                    console.log(request);
                    $("#form-clientes").submit();
                    storage.setItem("prospectos", "true");
                    storage.setItem("prospecto_1", request.p1_id);
                    storage.setItem("prospecto_2", request.p2_id);
                } else {
                    alert(request.message);
                }
            });
        } else {
            storage.clear();
            $("#form-clientes").submit();
        }
    });

    $(".overlay").click(function (e) {
        if (e.target.className == "overlay in") {
            var overlay = $(this);

            blurElement("#page-content", 0);
            overlay.fadeOut(500, function () {
                overlay.removeClass("in");
            });
        }
    });

    $(".overlay .btn-cancel").click(function (e) {
        console.log(e);

        var overlay = $(this).parents(".overlay.in");

        blurElement("#page-content", 0);
        overlay.fadeOut(500, function () {
            overlay.removeClass("in");
        });
    });

    $(".datepicker").datepicker({
        dateFormat : "yy-mm-dd"
    });

    $("#logo").click(function () {
        window.location.href = "/";
    });

    $(".box-globe-indicator").click(function (e) {
        e.preventDefault();

        var selector = $(this);
        var globeId = selector.data("globe");
        var globe = $(globeId);

        globe.css({
            top: (selector.offset().top - 18),
            left: (selector.offset().left + selector.width())
        }).fadeToggle(500);
    });

    $(".tooltip").each(function (index, element) {
        var tooltip = $(element);

        if (tooltip.attr('title') != '') {
            tooltip.append('<div class="tooltip-globe"> ' + tooltip.attr('title') + ' </div>');
            tooltip.append('<div class="tooltip-arrow">▼</div>');
            tooltip.removeAttr('title');
            tooltip.hover(function () {
                $(this).find('.tooltip-globe, .tooltip-arrow').fadeIn(200);
            }, function () {
                $(this).find('.tooltip-globe, .tooltip-arrow').fadeOut(200);
            });
        }
    });

    $("#show-deleted").change(function () {
        if ($(this).is(":checked")) {
            $(".deleted").show(0);
        } else {
            $(".deleted").hide(0);
        }
    });

    $(".user-notifications").click(function () {
        $(this).find(".notifications").fadeToggle(250);
    });

    var getNotifications = setInterval(function () {
        $.get("/SolicitudPermisos/GetNotifications", null, function (request) {
            if (request.notAttended > 0) {
                var audio = new Audio('/Audio/notification.mp3');
                audio.play();
            }

            $("#user-options .notifications .notifications-container").html(request.html);
        });
    }, 10000);
});

/* Animaciones Inicio */

function blurElement(element, size) {
    var filterVal = 'blur(' + size + 'px)';

    $(element)
        .css('filter', filterVal)
        .css('webkitFilter', filterVal)
        .css('mozFilter', filterVal)
        .css('oFilter', filterVal)
        .css('msFilter', filterVal)
        .css('transition', 'all 0.5s ease-out')
        .css('-webkit-transition', 'all 0.5s ease-out')
        .css('-moz-transition', 'all 0.5s ease-out')
        .css('-o-transition', 'all 0.5s ease-out');
}

/* Animaciones Fin */