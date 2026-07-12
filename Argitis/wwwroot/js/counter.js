// Attendre que jQuery soit chargé
$(document).ready(function() {
    function isElementInViewport($el) {
        // Vérifier si l'élément existe et a une position
        if (!$el || $el.length === 0) {
            return false;
        }
        
        try {
            var top = $el.offset().top;
            var bottom = top + $el.outerHeight();
            var viewportTop = $(window).scrollTop();
            var viewportBottom = viewportTop + $(window).height();
            return bottom > viewportTop && top < viewportBottom;
        } catch (e) {
            console.log('Erreur dans isElementInViewport:', e);
            return false;
        }
    }

    $(window).on("scroll", function () {
        var $odometer = $(".odometer");
        if ($odometer.length > 0 && isElementInViewport($odometer)) {
            setTimeout(function () {
                $(".style-1-1").html(20);
                $(".style-1-2").html(1.8);
                $(".style-1-3").html(1.7);
                $(".style-1-4").html(15);
            }, 0);
        }
    });
});
